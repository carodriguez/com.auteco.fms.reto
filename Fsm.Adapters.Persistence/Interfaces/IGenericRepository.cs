using Fsm.Domain.Common;
using System.Linq.Expressions;

namespace Fsm.Adapters.Persistence.Interfaces
{
    public interface IGenericRepository<TDomain, TPersistence>
            where TDomain : class
            where TPersistence : class
    {
        Task<Result<TDomain>> GetByIdAsync(int id);
        Task<Result<TDomain>> GetByIdAsync(Guid id);

        Task<Result<TDomain>> GetFirstOrDefaultAsync(
            Expression<Func<TPersistence, bool>>? filter = null,
            Func<IQueryable<TPersistence>, IOrderedQueryable<TPersistence>>? orderBy = null,
            string? includeProperties = null);

        // Método GetAllAsync sin parámetros
        Task<Result<IEnumerable<TDomain>>> GetAllAsync();

     

        Task<Result> AddAsync(TDomain domainEntity);
        Task<Result> UpdateAsync(TDomain domainEntity);
        Task<Result> DeleteAsync(int id);
    }
}