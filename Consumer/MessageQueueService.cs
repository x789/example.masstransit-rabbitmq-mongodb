// Copyright 2022 by TillW
// Licensed to you under the MIT license.
using MassTransit;
using Microsoft.Extensions.Hosting;

internal sealed class MessageQueueService : BackgroundService
{
    private readonly IBusControl bus;

    public MessageQueueService()
    {
        bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host("rabbitmq", "/", h =>
            {
                h.Username("guest");
                h.Password("guest");
            });

            cfg.ReceiveEndpoint("storeMessage", e => e.Consumer<StoreMessageConsumer>());
        });
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return bus.StartAsync(stoppingToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.WhenAll(base.StopAsync(cancellationToken), bus.StopAsync(cancellationToken));
    }
}