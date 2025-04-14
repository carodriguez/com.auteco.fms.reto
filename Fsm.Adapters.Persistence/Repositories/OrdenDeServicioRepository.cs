using AutoMapper;
using Fsm.Application.Interfaces.Persistence;
using Fsm.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Fsm.Adapters.Persistence.Repositories
{
    public class OrdenDeServicioRepository : GenericRepository<Fsm.Domain.Entities.OrdenDeServicio, Fsm.Adapters.Persistence.Models.OrdenDeServicio>, IOrdenDeServicioRepository
    {
        public OrdenDeServicioRepository(ApplicationDbContext context, IMapper mapper)
            : base(context, mapper)
        {
        }


        public override async Task<Result> AddAsync(Fsm.Domain.Entities.OrdenDeServicio domainEntity)
        {

            domainEntity.PublicoId = await GeneratePublicoIdAsync();

            return await base.AddAsync(domainEntity);

        }
        public async Task<string> GeneratePublicoIdAsync()
        {
            // Filtrar para evitar que se traigan registros con PublicoId nulo o vacío
            var lastServicio = await _dbSet
                .Where(s => !string.IsNullOrEmpty(s.PublicoId))
                .OrderByDescending(s => s.PublicoId)
                .FirstOrDefaultAsync();

            int nextNumber = 1;

            if (lastServicio != null)
            {
                // Intentar convertir directamente el PublicoId a número
                if (int.TryParse(lastServicio.PublicoId, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            // Retornar solo el número formateado
            return nextNumber.ToString("D8");
        }
    }
}
