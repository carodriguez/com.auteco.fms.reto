using AutoMapper;
using Fsm.Application.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsm.Adapters.Persistence.Repositories
{
    internal class ServicioRepository : GenericRepository<Fsm.Domain.Entities.Servicio, Fsm.Adapters.Persistence.Models.Servicio>, IServicioRepository
    {
        public ServicioRepository(ApplicationDbContext context, IMapper mapper)
            : base(context, mapper)
        {
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