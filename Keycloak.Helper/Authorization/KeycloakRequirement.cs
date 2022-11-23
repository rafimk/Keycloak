using Microsoft.AspNetCore.Authorization;

namespace Keycloak.Helper.Authorization
{
    public class KeycloakRequirement : IAuthorizationRequirement
    {
        public string PolicyName { get; set; }

        public KeycloakRequirement(string policyName)
        {
            PolicyName = policyName;
        }
    }
}