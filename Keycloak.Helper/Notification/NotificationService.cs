namespace Keycloak.Helper.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(HttpClient httpClient, ILogger<NotificationService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task SendNotificationAsync(Notification notification)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(notification);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsJsonAsync(new Uri(_httpClient.BaseAddress.OriginalString + "sendNotification"), content);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error sending notification: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending notification");
            }
        }
        
    }
}