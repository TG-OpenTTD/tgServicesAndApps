﻿using OpenTTD.Network.Models.Enums;

namespace OpenTTD.Network.Models.Messages;

public class AdminServerClientUpdateMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerClientUpdate;

    public uint ClientId { get; }

    public string ClientName { get; }

    public byte PlayingAs { get; }

    public AdminServerClientUpdateMessage(uint clientId, string clientName, byte playingAs)
    {
        ClientId = clientId;
        ClientName = clientName;
        PlayingAs = playingAs;
    }
}