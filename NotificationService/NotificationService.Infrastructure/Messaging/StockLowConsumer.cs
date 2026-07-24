using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NotificationService.Infrastructure.Messaging;

public class StockLowConsumer : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var connectionFactory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "warehouse",
            Password = "warehouse"
        };

        await using var connection = await connectionFactory.CreateConnectionAsync(cancellationToken); //connects the Notification Service to RabbitMQ.

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

        Console.WriteLine(json);

        await Task.CompletedTask;
    }
}

    