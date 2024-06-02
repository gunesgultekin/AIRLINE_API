using System.Net.Mail;
using System.Net;
using NotificationServiceAPI.Context;
using Microsoft.Azure.ServiceBus;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.HttpResults;

namespace NotificationServiceAPI
{
    public class MailService
    {
        private IConfiguration _configuration;
        private readonly QueueClient _queueClient;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _queueClient = new QueueClient(_configuration.GetConnectionString("SERVICE_BUS"),"mailbus");
        }

        public void RegisterOnMessageHandlerAndReceiveMessages()
        {
            _queueClient.RegisterMessageHandler(ProcessMessagesAsync,
                new MessageHandlerOptions(ExceptionReceivedHandler)
                {
                    MaxConcurrentCalls = 1,
                    AutoComplete = false
                });
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message
            string messageBody = Encoding.UTF8.GetString(message.Body);
            
            MailModel mailModel = JsonConvert.DeserializeObject<MailModel>(messageBody);

            try
            {
                await sendMail(mailModel);
            }
            catch (Exception ex)
            { 
            }
           
            // Complete the message
            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

        public async Task sendMail(MailModel model)
        {
            string sendermail = "-";
            string senderPassword = _configuration.GetConnectionString("MailClientPass");

            var smtpClient = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(sendermail, senderPassword)
            };

            MailMessage mail = new MailMessage(sendermail, model.to);
            mail.Subject = "Miles & Smiles ";
            mail.Body = $"Congratulations! You earned {model.bonus} TL Miles & Smiles bonus points from your {model.flightCode} flight today!";

            smtpClient.Send(mail);

        }
    }
}
