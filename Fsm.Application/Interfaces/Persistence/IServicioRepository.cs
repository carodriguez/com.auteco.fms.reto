using Fsm.Domain.Common;
using Fsm.Domain.Entities;

using System.Linq.Expressions;


namespace Fsm.Application.Interfaces.Persistence
{
    public interface IServicioRepository
    {
        // Métodos básicos CRUD
        // Métodos heredados de GenericRepository

        Task<Result<Servicio>> GetByIdAsync(int id);
        Task<Result<Servicio>> GetByIdAsync(Guid id);
        Task<Result<IEnumerable<Servicio>>> GetAllAsync();


        Task<Result<IEnumerable<Servicio>>> GetAllAsync(
            Expression<Func<Servicio, bool>>? filter = null,
            Func<IQueryable<Servicio>, IOrderedQueryable<Servicio>>? orderBy = null,
            string? includeProperties = null);

        Task<Result<Servicio>> FirstOrDefaultAsync(
        Expression<Func<Servicio, bool>>? filter = null,
        Func<IQueryable<Servicio>, IOrderedQueryable<Servicio>>? orderBy = null,
        string? includeProperties = null);


        Task<Result> AddAsync(Servicio servicio);

        Task<Result> DeleteAsync(int id);
        Task<Result> DeleteAsync(Servicio servicio);

        Task<Result> UpdateAsync(Servicio servicio);
        Task<string> GeneratePublicoIdAsync();
    }
}
