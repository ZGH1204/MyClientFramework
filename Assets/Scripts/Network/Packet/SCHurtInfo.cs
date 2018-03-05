using ProtoBuf;

namespace GGame.NetWork
{
    [ProtoContract]
    public class SCHurtInfo : ServerToClientPacketBase
    {
        public override int Id
        {
            get
            {
                return (int)PacketId.HurtInfo;
            }
        }

        [ProtoMember(1)]
        public int AttackerId { get; set; }

        [ProtoMember(2)]
        public int HurterId { get; set; }

        [ProtoMember(3)]
        public float HurterCurHp { get; set; }
         
    }
}