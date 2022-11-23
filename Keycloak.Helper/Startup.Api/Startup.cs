using System.ComponentModel.Design;
using System.Buffers;
namespace Keycloak.Helper.Startup.Api
{
    public class Startup
    {
          var eisPublisherStatus = Configuration["eis:PublisherStatus"];
        EISConstants.PublisherStatus = bool.Parse(eisPublisherStatus);
        _logger.LogInformation("EIS applied");

        var jwtOptions = Configuration.GetSection("JwtBearer").Get<JwtNearerOptions>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = jwtOptions.Authority;
                options.Audience = jwtOptions.Audience;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters.ValidateLifetime = true;
                options.TokenValidationParameters.NameclaimType = "preferred_username";
                options.TokenValidationParameters.RoleClaimType = "roles";

            });

        services.AddTransient<IClaimsTransformation>(_ => new KeycloakRolesClaimsTransformation("role", jwtOption.Audience));;

        services.AddAuthorization(options =>
        {
            #region Item 
            options.AddPolicy(Policies.CanViewItem, policy => policy.RequiresKeyloakEntitlement("item", "view"));
            #endregion
        }).AddKeycloakAuthorization(option =>
        {
            option.Audience = jwtOptions.Audience;
            option.TokenEndpoint = $"{jwtOptions.Authority}/protocol/openid-connect/token";
        });

        _logger.LogInformation("Keycloak completed");

        services.AddHttpClient<INotificationService, NotificationService>(client =>
        {
            client.BaseAddress = new Uri(Configuration["NotificationService:BaseAddress"]);
        }).SetHandlerLifetime(TimeSpan.FromMinutes(5))
        .AddPolicyHandler(GetRetryPolicy())
        .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddHttpClient<IEISPublisherService, EISPublisherService>(client =>
        {
            client.BaseAddress = new Uri(Configuration["EISPublisherService:BaseAddress"]);
        }).SetHandlerLifetime(TimeSpan.FromMinutes(5))
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
}