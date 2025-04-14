using Fsm.Domain.Common;
using Fsm.Domain.Entities;
using System.Linq.Expressions;

namespace Fsm.Application.Interfaces.Persistence
{
    public interface ITecnicoRepository
    {
        Task<Result<IEnumerable<Tecnico>>> GetAllAsync(
         Expression<Func<Tecnico, bool>>? filter = null,
         Func<IQueryable<Tecnico>, IOrderedQueryable<Tecnico>>? orderBy = null,
         string? includeProperties = null);

        Task<Result<Tecnico>> GetByIdAsync(Guid id);

        Task<Result> AddAsync(Tecnico tecnico);

    }
}
