
namespace NotificationServiceAPI
{
    public class BackgroundMailProcessor : BackgroundService
    {
        private readonly MailService _mailService;

        public BackgroundMailProcessor(MailService mailService)
        {
            _mailService = mailService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _mailService.RegisterOnMessageHandlerAndReceiveMessages();
            return Task.CompletedTask;
        }
    }
}
