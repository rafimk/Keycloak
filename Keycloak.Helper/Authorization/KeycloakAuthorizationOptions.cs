using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace Keycloak.Helper.Authorization
{
    public class KeycloakAuthorizationOptions
    {
        public string RequiredScheme { get; set; } = JwtBearerDefaults.AuthenticationScheme;
        public string TokenEndpoint { get; set; } = string.Empty;
        public HttpMessageHandler BackchannelHandler { get; set; } = new HttpClientHandler();
        public string Audience { get; set; } = string.Empty;
    }
}