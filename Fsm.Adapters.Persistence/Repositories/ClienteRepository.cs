using AutoMapper;
using Fsm.Application.Interfaces.Persistence;
using Fsm.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Fsm.Adapters.Persistence.Repositories
{
    public class ClienteRepository : GenericRepository<Fsm.Domain.Entities.Cliente, Fsm.Adapters.Persistence.Models.Cliente>, IClienteRepository
    {
        public ClienteRepository(ApplicationDbContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
             

        public override async Task<Result> AddAsync(Fsm.Domain.Entities.Cliente domainEntity)
        {

            domainEntity.PublicoId = await GeneratePublicoIdAsync();

            return await base.AddAsync(domainEntity);

        }
        public override async Task<Result> UpdateAsync(Fsm.Domain.Entities.Cliente domainEntity)
        {
            // Actualizar la fecha de modificación
            domainEntity.FechaModificacion = DateTime.Now;

            // Llamar al método base para realizar la actualización
            return await base.UpdateAsync(domainEntity);
        }


        public async Task<string> GeneratePublicoIdAsync()
        {
            const string PREFIX = "CLI";

            // Filtrar para evitar que se traigan registros con PublicoId nulo o vacío
            var lastCliente = await _dbSet
                .Where(c => !string.IsNullOrEmpty(c.PublicoId))
                .OrderByDescending(c => c.PublicoId)
                .FirstOrDefaultAsync();

            int nextNumber = 1;

            if (lastCliente != null && lastCliente.PublicoId.StartsWith(PREFIX))
            {
                // Extraer la parte numérica del PublicoId
                string numberPart = lastCliente.PublicoId.Substring(PREFIX.Length);
                if (int.TryParse(numberPart, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            // Retornar el nuevo PublicoId formateado
            return $"{PREFIX}{nextNumber:D8}";
        }

    }
}
