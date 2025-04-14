using Fsm.Domain.Common;
using Fsm.Domain.Entities;
using System.Linq.Expressions;


namespace Fsm.Application.Interfaces.Persistence
{
    public interface IClienteRepository 

    {
        // Métodos básicos CRUD
        // Métodos heredados de GenericRepository
        Task<Result<Cliente>> GetByIdAsync(Guid id);
        Task<Result<Cliente>> GetByIdAsync(int id);
        Task<Result<IEnumerable<Cliente>>> GetAllAsync();


        Task<Result<IEnumerable<Cliente>>> GetAllAsync(
            Expression<Func<Cliente, bool>>? filter = null,
            Func<IQueryable<Cliente>, IOrderedQueryable<Cliente>>? orderBy = null,
            string? includeProperties = null);

        Task<Result<Cliente>> FirstOrDefaultAsync(
        Expression<Func<Cliente, bool>>? filter = null,
        Func<IQueryable<Cliente>, IOrderedQueryable<Cliente>>? orderBy = null,
        string? includeProperties = null);


        Task<Result> AddAsync(Cliente cliente);

        Task<Result> DeleteAsync(int id);
        Task<Result> DeleteAsync(Cliente cliente);

        Task<Result> UpdateAsync(Cliente cliente);
        // Métodos de consulta

        Task<Result<Cliente>> GetByPublicId(string publicId);
       

      
        // Métodos de utilidad
        Task<string> GeneratePublicoIdAsync();

    }
}
