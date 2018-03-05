using ProtoBuf;

namespace GGame.NetWork
{
    [ProtoContract]
    public class CSPosition : ClientToServerPacketBase
    {
        public override int Id
        {
            get
            {
                return (int)PacketId.Position;
            }
        }

        [ProtoMember(1)]
        public int PlayerId { get; set; }

        [ProtoMember(2)]
        public float X { get; set; }

        [ProtoMember(3)]
        public float Y { get; set; }

        [ProtoMember(4)]
        public float Z { get; set; }

        [ProtoMember(5)]
        public int Animation { get; set; } // 1 = idle; 2 = move; 3 = attack;
    }
}