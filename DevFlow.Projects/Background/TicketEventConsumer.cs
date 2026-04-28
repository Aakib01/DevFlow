using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace DevFlow.Projects.Background
{  

    public class TicketEventConsumer : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync("ticket-events", false, false, false);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Received: {message}");
                await Task.CompletedTask; // 🔥 REQUIRED

                // 🔥 future: send email / notification
            };

            channel.BasicConsumeAsync(queue: "ticket-events",
                                 autoAck: true,
                                 consumer: consumer);

        }
    }
}
