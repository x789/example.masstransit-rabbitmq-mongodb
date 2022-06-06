// Copyright 2022 by TillW
// Licensed to you under the MIT license.
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

internal sealed class PersistedMessage
{
    public PersistedMessage(Messages.StoreMessage command)
    {
        Id = command.Id.ToString();
        Recipient = command.Recipient;
        Sender = command.Sender;
        Content = command.Content;
    }

    [BsonId]
    public string? Id;

    [BsonElement("recipient")]
    public string? Recipient;

    [BsonElement("sender")]
    public string? Sender;

    [BsonElement("content")]
    public string? Content;
}