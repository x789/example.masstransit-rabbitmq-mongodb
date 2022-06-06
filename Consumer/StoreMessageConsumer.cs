// Copyright 2022 by TillW
// Licensed to you under the MIT license.
using MassTransit;
using Messages;
using MongoDB.Driver;

internal sealed class StoreMessageConsumer : IConsumer<StoreMessage>
{
    public async Task Consume(ConsumeContext<StoreMessage> context)
    {
        var mongoClient = new MongoClient("mongodb://admin:secret@mongodb:27017");
        var database = mongoClient.GetDatabase("local");
        var collection = database.GetCollection<PersistedMessage>("messages");

        var message = new PersistedMessage(context.Message);
        await collection.InsertOneAsync(message);
    }
}