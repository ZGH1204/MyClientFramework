using ProtoBuf;

namespace GGame.NetWork
{
    [ProtoContract]
    public class CSCreatePlayer : ClientToServerPacketBase
    {
        public override int Id
        {
            get
            {
                return (int)PacketId.CreatePlayer;
            }
        }

        [ProtoMember(1)]
        public int PlayerId { get; set; }

        [ProtoMember(2)]
        public uint PositionId { get; set; }

        [ProtoMember(3)]
        public float x { get; set; }

        [ProtoMember(4)]
        public float y { get; set; }

        [ProtoMember(5)]
        public float z { get; set; }
    }
}