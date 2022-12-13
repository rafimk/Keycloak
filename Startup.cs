
using System.Security.Authentication.ExtendedProtection;
using System.Buffers;
using System.ComponentModel.Design;
var jwtOptions = Configuration.GetSection("JwtBearer").Get<Keycloak.AspNetCore.JwtBearerOptions>();

service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options => 
       {
            options.Authority = jwtOptions.Authority;
            options.Audience = jwtOptions.Audience;
            options.RequiredHttpsMetadata = false;
            options.TokenValidationParameters.ValidateLifetime = true;
            options.TokenValidationParameters.NameClaimType = "preferred_username";
            options.TokenValidationParameters.RoleClaimType = "role";

            // options.Events = new AccessManagerJwtBearerEvents(accessManagerOptions);
       });

service.AddTransient<IClaimsTransformation>(_ => new KeyloackRolesClaimsTransformation("role", jwtOptions.Audience));

service.AddAuthorization(options =>
{
    #region 
    options.AddPolicy(Ploicies.CanCreateItem, policy => policy.RequiresKeycloakEntitlement("ITEM-MANAGEMENT", "ADD"));
    options.AddPolicy(Ploicies.CanCreateItem, policy => policy.RequiresKeycloakEntitlement("ITEM-MANAGEMENT", "UPDATE"));
    options.AddPolicy(Ploicies.CanCreateItem, policy => policy.RequiresKeycloakEntitlement("ITEM-MANAGEMENT", "DELETE"));
    #endregion
});


