﻿namespace OpenTTD.API.Network.AdminPort;

public class AdminServerDateMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerDate;

    public OttdDate Date { get; }

    public AdminServerDateMessage(uint date)
    {
        Date = new OttdDate(date);
    }
}