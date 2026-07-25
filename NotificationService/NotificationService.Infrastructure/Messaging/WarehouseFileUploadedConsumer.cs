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

public class WarehouseFileUploadedConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<WarehouseFileUploadedConsumer> _logger;
    private readonly IConfiguration _configuration;

    public WarehouseFileUploadedConsumer(IServiceScopeFactory scopeFactory, ILogger<WarehouseFileUploadedConsumer> logger, IConfiguration configuration)
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
            queue: "file.uploaded.queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            queue: "file.uploaded.queue",
            exchange: "warehouse.events",
            routingKey: "file.uploaded",
            cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += OnMessageReceivedAsync;

        await channel.BasicConsumeAsync(
            queue: "file.uploaded.queue",
            autoAck: true,
            consumer: consumer,
            cancellationToken: cancellationToken);
    }

    private async Task OnMessageReceivedAsync(object sender, BasicDeliverEventArgs eventArgs)
    {
        byte[] body = eventArgs.Body.ToArray();

        string json = Encoding.UTF8.GetString(body);

        WarehouseFileUploadedEvent? fileUploadedEvent = JsonSerializer.Deserialize<WarehouseFileUploadedEvent>(json);

        if (fileUploadedEvent is null)
        {
            return;
        }

        using var scope = _scopeFactory.CreateScope();

        var notificationRepository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

        var preferenceRepository = scope.ServiceProvider.GetRequiredService<INotificationPreferenceRepository>();

        bool alreadyExists = await notificationRepository.ExistsByEventIdAsync(fileUploadedEvent.EventId, CancellationToken.None);

        if (alreadyExists)
        {
            return;
        }

        var preference = await preferenceRepository.GetByTypeAsync("FileUploaded", CancellationToken.None);

        var notification = new Notification
        {
            EventId = fileUploadedEvent.EventId,
            Type = "FileUploaded",
            Title = "Warehouse File Uploaded",
            Message = $"File '{fileUploadedEvent.FileName}' was uploaded for {fileUploadedEvent.RelatedEntityType}.",
            Severity =preference?.Severity ?? NotificationSeverity.Information,
            RelatedEntityId = fileUploadedEvent.RelatedEntityId,
            RelatedEntityType = fileUploadedEvent.RelatedEntityType
        };

        await notificationRepository.AddAsync(notification, CancellationToken.None);

        _logger.LogInformation(
            "File {FileName} was uploaded for {RelatedEntityType}",
            fileUploadedEvent.FileName,
            fileUploadedEvent.RelatedEntityType);
    }
}