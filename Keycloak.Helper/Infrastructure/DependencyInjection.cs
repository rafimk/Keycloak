namespace Keycloak.Helper.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IMessageProcessor,EisEventProcessorService>();
            services.AddEISServices();
            return services;
        }
        
    }
}