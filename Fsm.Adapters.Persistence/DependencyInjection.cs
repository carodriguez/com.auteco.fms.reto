using Fsm.Adapters.Persistence.Mappers;
using Fsm.Application.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fsm.Adapters.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {

            // Registro de DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


            // Registro de servicio de autoMapper
            services.AddAutoMapper(typeof(AutoMapperProfilesPersistence).Assembly); // Agregar AutoMapper

            // Registro los repositorios
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
