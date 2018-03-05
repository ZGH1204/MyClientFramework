using ZFramework.Network;
using ProtoBuf;

namespace GGame.NetWork
{
    [ProtoContract]
    public class SCPacketHeader : PacketHeaderBase, IPacketHeader
    {
        public SCPacketHeader(int id)
            : base(PacketType.ServerToClient, id)
        {

        }

        public int PacketLength
        {
            get;
            set;
        }
    }
}
