using System.Text;
using System.Text.Json;
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

public class ProductCreatedConsumer : BackgroundService
{
     private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ProductCreatedConsumer> _logger;
    private readonly IConfiguration _configuration;
    public ProductCreatedConsumer(IServiceScopeFactory scopeFactory, ILogger<ProductCreatedConsumer> logger, IConfiguration configuration)
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
        var connectionFactory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMQ:HostName"],
            UserName = _configuration["RabbitMQ:UserName"],
            Password = _configuration["RabbitMQ:Password"]
        };

        var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

        var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queue: "product.created.queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            queue: "product.created.queue",
            exchange: "warehouse.events",
            routingKey: "product.created",
            cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += OnMessageReceivedAsync;

        await channel.BasicConsumeAsync(
            queue: "product.created.queue",
            autoAck: true,
            consumer: consumer,
            cancellationToken: cancellationToken);
    }

   

    private async Task OnMessageReceivedAsync(object sender, BasicDeliverEventArgs eventArgs)
    {
        byte[] body = eventArgs.Body.ToArray();

        string json = Encoding.UTF8.GetString(body);

        ProductCreatedEvent? prodCreatedEvent = JsonSerializer.Deserialize<ProductCreatedEvent>(json);//converts the JSON text into a real C# object, so we can access its properties

        if (prodCreatedEvent is null)
            return;
        
        using var scope = _scopeFactory.CreateScope();

        var repository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
        if (await repository.ExistsByEventIdAsync(prodCreatedEvent.EventId, CancellationToken.None))
        {
            return;
        }
        var preferenceRepository = scope.ServiceProvider.GetRequiredService<INotificationPreferenceRepository>();
        var preference = await preferenceRepository.GetByTypeAsync("ProductCreated", CancellationToken.None);
        var notification = new Notification
        {
            EventId = prodCreatedEvent.EventId,
            Type = "ProductCreated",
            Title = "Product Created",
            Message = $"Product {prodCreatedEvent.ProductName} has been created",
            Severity = preference?.Severity ?? NotificationSeverity.Information,
            RelatedEntityId = prodCreatedEvent.ProductId.ToString(),
            RelatedEntityType = "Product"
        };
        await repository.AddAsync(notification, CancellationToken.None);

        _logger.LogInformation("Product {ProductName} with id {id} has been created",  prodCreatedEvent.ProductName, prodCreatedEvent.ProductId);
    }
}