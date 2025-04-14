using Fsm.Domain.Common;
using Fsm.Domain.Entities;
using System.Linq.Expressions;

namespace Fsm.Application.Interfaces.Persistence
{
    public interface IOrdenDeServicioRepository
    {
        // Métodos básicos CRUD
        Task<Result<OrdenDeServicio>> GetByIdAsync(Guid id);
        Task<Result<OrdenDeServicio>> GetByIdAsync(int id);
        Task<Result<IEnumerable<OrdenDeServicio>>> GetAllAsync();

        Task<Result<IEnumerable<OrdenDeServicio>>> GetAllAsync(
            Expression<Func<OrdenDeServicio, bool>>? filter = null,
            Func<IQueryable<OrdenDeServicio>, IOrderedQueryable<OrdenDeServicio>>? orderBy = null,
            string? includeProperties = null);

        Task<Result<OrdenDeServicio>> FirstOrDefaultAsync(
            Expression<Func<OrdenDeServicio, bool>>? filter = null,
            Func<IQueryable<OrdenDeServicio>, IOrderedQueryable<OrdenDeServicio>>? orderBy = null,
            string? includeProperties = null);

        Task<Result> AddAsync(OrdenDeServicio ordenDeServicio);

        Task<Result> DeleteAsync(int id);
        Task<Result> DeleteAsync(OrdenDeServicio ordenDeServicio);

        Task<Result> UpdateAsync(OrdenDeServicio ordenDeServicio);

        Task<Result> UpdatePartialAsync(Guid id, Dictionary<string, object> propertiesToUpdate);


        // Métodos de consulta
        Task<Result<OrdenDeServicio>> GetByPublicId(string publicId);
        

        Task<string> GeneratePublicoIdAsync();

    }
}
