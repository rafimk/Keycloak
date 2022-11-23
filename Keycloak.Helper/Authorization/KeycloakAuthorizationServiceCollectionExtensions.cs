using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Keycloak.Helper.Authorization
{
    public static class KeycloakAuthorizationServiceCollectionExtensions
    {
        public static IServiceCollection AddKeycloakAuthorization(this IServiceCollection services, Action<KeycloakAuthorizationOptions> configureOptions)
        {
            services.Configure(configureOptions);
            services.AddHttpContextAccessor();
            services.AddSingleton<IAuthorizationHandler, KeycloakAuthorizationHandler>();
            
            return services;
        }
    }
}