using System.Text;
using System.Text.Json;
using Domain.DomainEvents;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EventBooking.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> logger;
    private readonly IConfiguration configuration;

    public Worker(IConfiguration configuration, ILogger<Worker> logger)
    {
        this.configuration = configuration;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory { HostName = configuration["RabbitMq:Host"]!, UserName = configuration["RabbitMq:UserName"]!, Password = configuration["RabbitMq:Password"]! };
        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(exchange: "booking_events", type: ExchangeType.Direct, durable: true);
        await channel.QueueDeclareAsync(queue: "booking_created_queue", durable: true);
        await channel.QueueBindAsync(queue: "booking_created_queue", exchange: "booking_events", routingKey: "booking.created");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = JsonSerializer.Deserialize<BookingCreatedDomainEvent>(Encoding.UTF8.GetString(body));
            logger.LogInformation("Booking created: {BookingId}, seats: {Seats}", message!.BookingId, message.Seats);
            await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
        };

        await channel.BasicConsumeAsync(queue: "booking_created_queue", autoAck: false, consumer: consumer);
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}