﻿using OpenTTD.Network.Models.Enums;

namespace OpenTTD.Network.Models.Messages;

public class AdminServerClientInfoMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerClientInfo;

    public uint ClientId { get; set; }

    public string Hostname { get; set; }

    public string ClientName { get; set; }

    public byte Language { get; set; }

    public OttdDate JoinDate { get; set; }

    public byte PlayingAs { get; set; }
}