using System.Text;
using System.Text.Json;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Infrastructure.Messaging
{
    public class RabbitMqPublisher : IMessagePublisher
    {
        private readonly IConfiguration configuration;

        public RabbitMqPublisher(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
        {
            var factory = new ConnectionFactory { HostName = configuration["RabbitMq:Host"]!, UserName = configuration["RabbitMq:UserName"]!, Password = configuration["RabbitMq:Password"]! };
            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(exchange: "booking_events", type: ExchangeType.Direct, durable: true);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(exchange: "booking_events", routingKey: "booking.created", body: body);
        }
    }
}