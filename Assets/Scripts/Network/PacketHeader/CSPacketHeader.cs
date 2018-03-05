using ProtoBuf;

namespace GGame.NetWork
{
    [ProtoContract]
    public class CSPacketHeader : PacketHeaderBase
    {
        public CSPacketHeader(int id)
            : base(PacketType.ClientToServer, id)
        {

        }
    }
}
