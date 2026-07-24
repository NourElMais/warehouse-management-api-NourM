using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Warehouse.Application.Interfaces;

namespace Warehouse.Infrastructure.Messaging;

public class RabbitMqPublisher : IRabbitMqPublisher
{
    private readonly ConnectionFactory _connectionFactory; //creates connections to RabbitMQ

    public RabbitMqPublisher()
    {
        _connectionFactory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "warehouse",
            Password = "warehouse"
        };
    }

    //method that the application will call
    public async Task PublishAsync<T>(string routingKey, T message, CancellationToken cancellationToken)
    {
        await using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);//the factory actually creates a connection.

        await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        //The exchange decides: Which queue(s) should receive this message
        await channel.ExchangeDeclareAsync(
            exchange: "warehouse.events",
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            cancellationToken: cancellationToken);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: "warehouse.events",
            routingKey: routingKey,
            body: body,
            cancellationToken: cancellationToken);
    }

}
