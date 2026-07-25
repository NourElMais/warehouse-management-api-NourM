using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationService.Application.IntegrationEvents;
using NotificationService.Application.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Warehouse.Notifications.Domain.Entities;

namespace NotificationService.Infrastructure.Messaging;

public class StockLowConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<StockLowConsumer> _logger;
    private readonly IConfiguration _configuration;
    public StockLowConsumer(IServiceScopeFactory scopeFactory, ILogger<StockLowConsumer> logger, IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await StartConsumerAsync(cancellationToken);

                await Task.Delay(Timeout.Infinite, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception exception)
            {
                _logger.LogWarning($"RabbitMQ is unavailable. Retrying in 5 seconds. Error: {exception.Message}");

                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }

        }
    }
    private async Task StartConsumerAsync(CancellationToken cancellationToken)
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:HostName"],
            UserName = _configuration["RabbitMQ:UserName"],
            Password = _configuration["RabbitMQ:Password"]
        };

        var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

        var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queue: "stock.low.queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            queue: "stock.low.queue",
            exchange: "warehouse.events",
            routingKey: "stock.low",
            cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += OnMessageReceivedAsync;

        await channel.BasicConsumeAsync(
            queue: "stock.low.queue",
            autoAck: true,
            consumer: consumer,
            cancellationToken: cancellationToken);
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

        _logger.LogInformation("Low-stock notification created for product {ProductName}. Quantity: {Quantity}",
            stockLowEvent.ProductName,
            stockLowEvent.QuantityInStock);
    }
}


    