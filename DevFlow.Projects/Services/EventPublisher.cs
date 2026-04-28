using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace DevFlow.Projects.Services
{
    public class EventPublisher
    {
        private readonly IConfiguration _config;

        public EventPublisher(IConfiguration config)
        {
            _config = config;

        }

        public async Task PublishAsync(string queueName, object message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queueName, durable: false, exclusive: false, autoDelete: false);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            await channel.BasicPublishAsync(exchange: "",
                                            routingKey: queueName,
                                            body: body);
        }
    }
}
