﻿using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.ServerClientUpdate;

public sealed record ServerClientUpdateMessage: IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CLIENT_UPDATE;
    
    public uint ClientId { get; init; }
    public string ClientName { get; init; }
    public byte CompanyId { get; init; }
}