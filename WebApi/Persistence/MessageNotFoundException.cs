// Copyright 2022 by TillW
// Licensed to you under the MIT license.
internal class MessageNotFoundException : Exception
{
    public MessageNotFoundException(Guid id) : base($"Cannot find message with id '{id}'.")
    {
    }
}