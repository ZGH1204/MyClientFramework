using ProtoBuf;

namespace GGame.NetWork
{
    [ProtoContract]
    public class SCPlayerInfo : ServerToClientPacketBase
    {
        public override int Id
        {
            get
            {
                return (int)PacketId.PlayerInfo;
            }
        }

        [ProtoMember(1)]
        public int PlayerId { get; set; }

        [ProtoMember(2)]
        public float CurHP { get; set; }

        [ProtoMember(3)]
        public int KillPlayerNum { get; set; }

        [ProtoMember(4)]
        public int KillNPCNum { get; set; }

        [ProtoMember(5)]
        public int Grade { get; set; }
         
        [ProtoMember(6)]
        public float MaxHP { get; set; }
    }
}