using AutoMapper;
using Fsm.Adapters.Persistence.Repositories;
using Fsm.Application.Interfaces.Persistence;

namespace Fsm.Adapters.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private IClienteRepository _clienteRepository;
        private IOrdenDeServicioRepository _ordenDeServicioRepository;
        private ITecnicoRepository _tecnicoRepository;
        private IServicioRepository _servicioRepository;

        public UnitOfWork(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IClienteRepository Clientes =>
        _clienteRepository ??= new ClienteRepository(_context, _mapper);

        public IOrdenDeServicioRepository OrdenesDeServicio =>
            _ordenDeServicioRepository ??= new OrdenDeServicioRepository(_context, _mapper);

        public ITecnicoRepository Tecnicos =>
               _tecnicoRepository ??= new TecnicoRepository(_context, _mapper);

        public IServicioRepository Servicios =>
            _servicioRepository ??= new ServicioRepository(_context, _mapper);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}