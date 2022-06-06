// Copyright 2022 by TillW
// Licensed to you under the MIT license.
using MassTransit;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public sealed class MessagesController : ControllerBase
{
    private readonly ISendEndpointProvider sendProvider;

    public MessagesController(ISendEndpointProvider sendProvider)
    {
        this.sendProvider = sendProvider;
    }

    [HttpGet]
    public IAsyncEnumerable<Message> GetMessages()
    {
        var repo = new MessageRepository();
        return repo.GetAll();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMessage([FromRoute] Guid id)
    {
        var repo = new MessageRepository();
        try
        {
            return Ok(await repo.Get(id));
        }
        catch (MessageNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] IncomingMessageDto incomingMessage)
    {
        var command = new StoreMessageCommand(Guid.NewGuid(), incomingMessage.Recipient, incomingMessage.Sender, incomingMessage.Content);
        var endpoint = await sendProvider.GetSendEndpoint(new Uri("queue:storeMessage"));
        await endpoint.Send<Messages.StoreMessage>(command);
        return CreatedAtAction(nameof(GetMessage), new { id = command.Id }, null);
    }

    private record StoreMessageCommand(Guid Id, String Recipient, String Sender, String Content) : Messages.StoreMessage;
}