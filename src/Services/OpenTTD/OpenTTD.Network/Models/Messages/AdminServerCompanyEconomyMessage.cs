﻿using OpenTTD.Network.Models.Enums;

namespace OpenTTD.Network.Models.Messages;

public class AdminServerCompanyEconomyMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerCompanyEconomy;

    public byte CompanyId { get; internal set; }

    public ulong Money { get; internal set; }

    public ulong CurrentLoan { get; internal set; }

    public ulong Income { get; internal set; }

    public ushort DeliveredCargo { get; internal set; }

    // there is also data for last 2 quarters - let's skip it for now


}