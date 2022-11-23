namespace Keycloak.Helper.Notification
{
    public interface INotificationService
    {
         public Task SendNotificationAsync(Notification notification);
    }
}