using AutoMapper;
using Fsm.Application.Interfaces.Persistence;
using Fsm.Domain.Common;
using Fsm.Domain.Entities;

namespace Fsm.Adapters.Persistence.Repositories
{
    internal class TecnicoRepository : GenericRepository<Fsm.Domain.Entities.Tecnico, Fsm.Adapters.Persistence.Models.Tecnico>, ITecnicoRepository
    {
        public TecnicoRepository(ApplicationDbContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

     
    }
}