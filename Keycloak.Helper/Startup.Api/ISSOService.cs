namespace Keycloak.Helper;

public interface ISSOService
{
    Task<string> GetTokenAsync(string username, string password, string client_id, string grant_type);
    Task<string> GetUserIdFromUserName(string username, string access_token);
    Task<bool> VerifyUserRole(string userId, string roleName, string access_token);
    Task<bool> AccessVerification(string token, string username, string groupId);
}