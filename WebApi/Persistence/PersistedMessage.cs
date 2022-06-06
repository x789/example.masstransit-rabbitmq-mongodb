// Copyright 2022 by TillW
// Licensed to you under the MIT license.
using MongoDB.Bson.Serialization.Attributes;

internal sealed class PersistedMessage
{
#pragma warning disable 0649 // setters will be used by MongoDB's magic...
    [BsonId]
    public string? Id;

    [BsonElement("recipient")]
    public string? Recipient;

    [BsonElement("sender")]
    public string? Sender;

    [BsonElement("content")]
    public string? Content;
#pragma warning restore 0649
}