// Copyright 2022 by TillW
// Licensed to you under the MIT license.
namespace Messages;

public interface StoreMessage
{
    System.Guid Id { get; }
    string Sender { get; }
    string Recipient { get; }
    string Content { get; }
}