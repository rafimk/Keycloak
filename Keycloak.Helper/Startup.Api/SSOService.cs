using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;

namespace Keycloak.Helper.Startup.Api
{
    public class SSOService : ISSOService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SSOService> _logger;
        private readonly AccessManagerOptions _options;


        public SSOService(HttpClient httpClient, AccessManagerOptions options, ILogger<SSOService> logger)
        {
            _httpClient = httpClient;
            _options = options;
            _logger = logger;
        }

        public Task<string> GetTokenAsync(string username, string password, string client_id, string grant_type)
        {
            try
            {
                HttpContent content = new FormUrlEncodedContent
                (
                    new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("username", "admin"),
                        new KeyValuePair<string, string>("password", "admin"),
                        new KeyValuePair<string, string>("client_id", "admin-cli"),
                        new KeyValuePair<string, string>("grant_type", "password")
                    }
                );

                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                var endPointUri = $"{_httpClient.BaseAddress.OriginalString}realms/master/protocol/openid-connect/token";

                var response = _httpClient.PostAsync(new Uri(endPointUri), content);

                var result = response.Result.Content.ReadAsStringAsync().Result;

                var jsonData = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

                return jsonData.Access_token;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while getting token" + ex.StackTrace);
            }
        }

        
        public Task<string> GetUserIdFromUserName(string username, string access_token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

                var endPointUri = $"{_httpClient.BaseAddress.OriginalStrin}admin/realms/master/users?username={username}";

                var response = _httpClient.GetAsync(new Uri(endPointUri));

                var result = response.Result.Content.ReadAsStringAsync().Result;

                var jsonData = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

                return jsonData.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while getting user id" + ex.StackTrace);
            }
        }

        public Task<bool> VerifyUserRole(string userId, string roleName, string access_token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

                var endPointUri = $"{_httpClient.BaseAddress.OriginalString}admin/realms/master/users/{userId}/role-mappings/realm";

                var response = _httpClient.GetAsync(new Uri(endPointUri));

                var result = response.Result.Content.ReadAsStringAsync().Result;

                JObject data = JObject.Parse(result);

                var firstPart = data.First;
                var firstPosition = firstPart.First;

                var ss = firstPosition.First;

                foreach (var rootLevel in firstPart.First)
                {
                    Object obj = JObject.Parse(rootLevel.ToString());
                    string role = (string)obj["name"];
                    if (obj.Contain(roleName))
                    {
                        return true;
                    }
                }

                var jsonData = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

                return jsonData.Name;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while verifying user role" + ex.StackTrace);
            }
        }


        public async Task<bool> AccessVerification(string token, string username, string groupId)
        {
            var client = new HttpClient(new HttpClientHandler());
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (_options.ServerUrl.StartsWith("https://"))
            {
                System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;   
            }

            var response = await client.GetAsync($"{_options.ServerUrl + username}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync().Result;
                var userProfile = JsonConvert.DeserializeObject<AdmisUserUserAuthorizationsModel>(result);

                if (userProfile is not null)
                {
                    return userProfile.Authorizedgroup.Any(x => x.code == groupId);
                }
            }

            return false;
        }
    }

    private class AccessToken
    {
        public string Access_token { get; set; }
        public int Expires_in { get; set; }
        public string Refresh_expires_in { get; set; }
        public string Refresh_token { get; set; }
        public string Token_type { get; set; }
        public string Session_state { get; set; }
        public string Scope { get; set; }
    }

    private class RealmMappings
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Composite { get; set; }
        public bool ClientRole { get; set; }
        public bool ContainerId { get; set; }
    }
}