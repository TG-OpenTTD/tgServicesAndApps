using Networking.Common;
using Networking.Enums;
using Networking.Messages.Inbound;
using Networking.Messages.Outbound;

namespace Networking.Messages;

public sealed class PacketService : IPacketService
{
    private readonly IEnumerable<IPacketTransformer> _packetTransformers;
    private readonly IEnumerable<IMessageTransformer> _messageTransformers;

    public PacketService(
        IEnumerable<IPacketTransformer> packetTransformers,
        IEnumerable<IMessageTransformer> messageTransformers) =>
        (_packetTransformers, _messageTransformers) =
        (packetTransformers, messageTransformers);

    public IMessage ReadPacket(Packet packet)
    {
        var type = packet.ReadByte();

        return Enum.IsDefined(typeof(PacketType), (int) type)
            ? TransformPacket((PacketType)type, packet)
            : new GenericMessage { PacketType = PacketType.INVALID_ADMIN_PACKET };
    }

    public Packet CreatePacket(IMessage message)
    {
        var transformer = _messageTransformers.FirstOrDefault(mt => mt.PacketType == message.PacketType);
        return transformer!.Transform(message);
    }

    private IMessage TransformPacket(PacketType packetType, Packet packet)
    {
        var transformer = _packetTransformers.FirstOrDefault(pt => pt.PacketType == packetType);

        return transformer is not null
            ? transformer.Transform(packet)
            : new GenericMessage { PacketType = packetType };
    }
}