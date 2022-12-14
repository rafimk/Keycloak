using System.Security.Cryptography.X509Certificates;
using System;

namespace Keycloak.Helper;

public class JwtBearerOptions
{
    public string Authority { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}
