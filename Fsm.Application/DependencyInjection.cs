using Fsm.Application.Mappers;
using Fsm.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fsm.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {


            // Registro de AutoMapper
            services.AddAutoMapper(typeof(AutoMapperProfilesApplication).Assembly); // Agregar AutoMapper

            // Registro de servicios
            services.AddScoped<ClienteService>(); // Registrar el servicio de cliente

            services.AddScoped<TecnicoService>(); // Registrar el servicio de tecnico
            services.AddScoped<ServicioService>(); // Registrar el servicio de servicio
            
            services.AddScoped<OrdenDeServicioService>(); // Registrar el servicio de orden de servicio


            return services;
        }
    }
}
