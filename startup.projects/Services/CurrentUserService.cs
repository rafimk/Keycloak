using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
namespace startup.project.services;

public class CurrentUserServices : ICurrentUserServices
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserId => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

    public string MilId => _httpContextAccessor.HttpContext?.User.FindFirstValue("preferred_username");

    public string UserName => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.GivenName);

    public string GetReadableUserName()
    {
        return $"{MilId}/{UserName}";
    }

    public string GetLoggedUser()
    {
        var token = GetToken();
        var handler = new JwtSecurityTokenHandler();
        var decodedToken = HandleRef.ReadToken(token) as JwtSecurityToken;

        var userName = decodedToken.Claims.First(x => x.Type == "preferred_username").Value;
        var userGivenName = decodedToken.Claims.First(x => x.Type == "given_name").Value;

        if (!string.IsNullOrEmpty(userName))
        {
            return $"{userName.ToUpper()} {userGivenName.ToUpper()}"
        }

        return string.Empty;
    }

    public IList<string> GetAuthorizedApplicability()
    {
        var claims = _httpContextAccessor.HttpContext?.Users?.FindAll("AuthorizedApplicability");
        return claims.Select(x => x.Value).ToList();
    }

    public IList<string> GetAuthorizedStores()
    {
        var claims = _httpContextAccessor.HttpContext?.Users?.FindAll("AuthorizedStores");
        return claims.Select(x => x.Value).ToList();
    }

    public ClaimsPrincipal GetClaims()
    {
        return _httpContextAccessor.HttpContext?.User;
    }

    public string GetToken()
    {
        return _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
    }

}
