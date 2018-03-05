
using ZFramework.Network;
using ProtoBuf;
using System;

namespace GGame.NetWork
{
    [ProtoContract]
    public class CSLogin : ClientToServerPacketBase
    { 
        public override int Id
        {
            get
            {
                return (int)PacketId.Login;
            }
        }
        
        [ProtoMember(1)]
        public int PacketType { get; set; }

        [ProtoMember(2)] 
        public string PlayerId { get; set; }

        [ProtoMember(3)]
        public string HPValue { get; set; }

        [ProtoMember(4)]
        public string Postion { get; set; }

        [ProtoMember(5)]
        public bool IsAddSpeed { get; set; }

        [ProtoMember(6)]
        public bool IsAttack { get; set; }

        [ProtoMember(7)]
        public int JiFen { get; set; }
    }
}
