namespace RealWorldConduit_Infrastructure.Services.NotificationService
{
    public interface IEmailNotificationService
    {
        Task SendEmailAsync(String emailAddress, String emailEvent, List<String> subjects, List<String> contents);
        Task SendEmailAsync(String emailAddress, String emailEvent);
    }
}
