using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Outbound.UpdateFrequency;

public sealed record UpdateFrequencyMessage : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_ADMIN_UPDATE_FREQUENCY;
    
    public AdminUpdateType UpdateType { get; init; }
    public Enums.UpdateFrequency UpdateFrequency { get; init; }
}