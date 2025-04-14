using Fsm.Adapters.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Fsm.Adapters.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; } = null!;

        public DbSet<Tecnico> Tecnicos { get; set; } = null!;

        public DbSet<OrdenDeServicio> OrdenesDeServicio { get; set; } = null!;

        public DbSet<OrdenDeServicioDetalle> OrdenesDeServicioDetalle { get; set; } = null!;

        public DbSet<Servicio> Servicios { get; set; } = null!;


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    }
}
