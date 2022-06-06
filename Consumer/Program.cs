// Copyright 2022 by TillW
// Licensed to you under the MIT license.
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = new HostBuilder().ConfigureServices((hostContext, services) =>
{
    services.AddHostedService<MessageQueueService>();
});

await builder.RunConsoleAsync();