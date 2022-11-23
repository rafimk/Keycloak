using Microsoft.AspNetCore.Authorization;

namespace Keycloak.Helper.Authorization
{
    public static class KeycloakAuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequiresKeycloakEntitlement(this AuthorizationPolicyBuilder builder, 
            string resource, string scope)
        {
            builder.AddRequirements(new KeycloakRequirement($"{resource}#{scope}"));
            return builder;
        }
    }
}