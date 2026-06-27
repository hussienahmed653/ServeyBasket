namespace ServeyBasket.Services.BackgroundJobNotification;

public interface INotificationService
{
    Task SendNewPollsNotification(int? pollId = null);
}
