using Azure.Messaging.ServiceBus;
using FlightServiceAPI.Context;
using Hangfire;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace FlightServiceAPI.Repositories
{
    public class Scheduler
    {
        //private IConfiguration _configuration;
        
        public static async Task sendToQueue(MailModel model)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            // AZURE SERVICE BUS CONNECTION
            await using (ServiceBusClient client = new ServiceBusClient(configuration.GetConnectionString("SERVICE_BUS")))
            {
                ServiceBusSender sender = client.CreateSender("mailbus");

                var mailContent = new MailModel
                {
                    to = model.to,
                    bonus = model.bonus,
                    flightCode = model.flightCode
                };

                string messageBody = JsonSerializer.Serialize(mailContent); 

                ServiceBusMessage message = new ServiceBusMessage(messageBody);

                // SEND MESSAGE TO QUEUE
                await sender.SendMessageAsync(message);
            }
        }

        // SCHEDULE MAIL (12 HOUR)
        public static void scheduleMail(MailModel model)
        {
            BackgroundJob.Schedule(
                () => sendToQueue(model),
                TimeSpan.FromHours(12)
                );
        }
    }
}

