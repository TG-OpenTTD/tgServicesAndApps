﻿using OpenTTD.Network.Models.Enums;

namespace OpenTTD.Network.Models.Messages;

public class AdminServerConsoleMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerConsole;

    public string Origin { get; }

    public string Message { get; }

    public AdminServerConsoleMessage(string origin, string message)
    {
        Origin = origin;
        Message = message;
    }
}