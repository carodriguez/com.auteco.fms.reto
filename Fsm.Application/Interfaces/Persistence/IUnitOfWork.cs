namespace Fsm.Application.Interfaces.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IClienteRepository Clientes { get; }
        ITecnicoRepository Tecnicos { get; }
        IOrdenDeServicioRepository OrdenesDeServicio { get; }

        IServicioRepository Servicios { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}