// Copyright 2022 by TillW
// Licensed to you under the MIT license.
using MongoDB.Bson;
using MongoDB.Driver;

internal sealed class MessageRepository
{
	private readonly IMongoCollection<PersistedMessage> collection;

	public MessageRepository()
	{
		var mongoClient = new MongoClient("mongodb://admin:secret@mongodb:27017");
		var database = mongoClient.GetDatabase("local");
		collection = database.GetCollection<PersistedMessage>("messages");
	}

	public async Task<Message> Get(Guid id)
	{
		// A cursor can only be used once. Therefore we cannot check with AnyAsync before we use it because that would dispose the cursor.
		using var cursor = await collection.FindAsync(x => x.Id == id.ToString());
		await cursor.MoveNextAsync();
		var persistedMessage = cursor.Current?.FirstOrDefault();
		if (persistedMessage != default)
		{
			return new Message(Guid.Parse(persistedMessage.Id!), persistedMessage.Sender!, persistedMessage.Recipient!, persistedMessage.Content!);
        }
		else
        {
			throw new MessageNotFoundException(id);
        }
    }

	/// <summary>
    /// Gets all stored messages.
    /// </summary>
    /// <returns>An async-enumerable containing all messages.</returns>
    /// <remarks>The returned object can only be used once, because it is only a thin wrapper around <see cref="IAsyncCursor{TDocument}"/>.</remarks>
    public async IAsyncEnumerable<Message> GetAll()
    {
		using var cursor = await collection.FindAsync(new BsonDocument());
		while (cursor.MoveNext())
		{
			foreach (var doc in cursor.Current)
			{
				yield return new Message(Guid.Parse(doc.Id!), doc.Sender!, doc.Recipient!, doc.Content!); ;
			}
		}
    }
}