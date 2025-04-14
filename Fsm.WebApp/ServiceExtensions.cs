using Fsm.WebApp.Helper;

namespace Fsm.WebApp
{
    public static class ServiceExtensions
    {
        public static void AddHelperServices(this IServiceCollection services)
        {
            services.AddTransient<INotificacionServiceHelper, NotificacionServiceHelper>();
           

        }
    }
}
