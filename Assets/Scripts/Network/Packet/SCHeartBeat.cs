using ProtoBuf;
using System;

namespace GGame.NetWork
{
    [Serializable, ProtoContract(Name = @"SCHeartBeat")]
    public partial class SCHeartBeat : ServerToClientPacketBase
    { 
        public override int Id
        {
            get
            {
                return (int)PacketId.HeartBeat;
            }
        }
    }
}
