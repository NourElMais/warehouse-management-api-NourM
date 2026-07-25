using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificationService.Application.IntegrationEvents;
using NotificationService.Application.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Warehouse.Notifications.Domain.Entities;

namespace NotificationService.Infrastructure.Messaging;

public class StockLowConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    public StockLowConsumer(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var connectionFactory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "warehouse",
            Password = "warehouse"
        };

        await using var
            connection =
                await connectionFactory
                    .CreateConnectionAsync(cancellationToken); //connects the Notification Service to RabbitMQ.

        await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        //creates a queue called: stock.low.queue
        await channel.QueueDeclareAsync(
            queue: "stock.low.queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        //we bind the queue to the exchange
        //Whenever the exchange warehouse.events receives a message with routing key stock.low, put it into stock.low.queue
        await channel.QueueBindAsync(
            queue: "stock.low.queue",
            exchange: "warehouse.events",
            routingKey: "stock.low",
            cancellationToken: cancellationToken);
        var consumer = new AsyncEventingBasicConsumer(channel); //this creates the rabbitmq consumer.
        consumer.ReceivedAsync += OnMessageReceivedAsync;

        await channel.BasicConsumeAsync(
            queue: "stock.low.queue",
            autoAck: true,
            consumer: consumer,
            cancellationToken: cancellationToken);
        
        await Task.Delay(Timeout.Infinite, cancellationToken); //keeps the background service alive
    }

    private async Task OnMessageReceivedAsync(object sender, BasicDeliverEventArgs eventArgs)
    {
        byte[] body = eventArgs.Body.ToArray();

        string json = Encoding.UTF8.GetString(body);

        LowStockDetectedEvent? stockLowEvent = JsonSerializer.Deserialize<LowStockDetectedEvent>(json);//converts the JSON text into a real C# object, so we can access its properties

        if (stockLowEvent is null)
            return;
        
        using var scope = _scopeFactory.CreateScope();

        var repository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
        if (await repository.ExistsByEventIdAsync(stockLowEvent.EventId, CancellationToken.None))
        {
            return;
        }
        var notification = new Notification
        {
            EventId = stockLowEvent.EventId,
            Type = "LowStock",
            Title = "Low Stock Alert",
            Message = $"Product {stockLowEvent.ProductName} is low in stock. Quantity: {stockLowEvent.QuantityInStock}",
            Severity = "Warning",
            RelatedEntityId = stockLowEvent.ProductId.ToString(),
            RelatedEntityType = "Product"
        };

        await repository.AddAsync(notification, CancellationToken.None);

        Console.WriteLine(stockLowEvent.ProductName);
        Console.WriteLine(stockLowEvent.QuantityInStock);

        await Task.CompletedTask;
    }
}


    