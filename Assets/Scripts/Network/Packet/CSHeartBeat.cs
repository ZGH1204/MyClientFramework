using ProtoBuf;
using System;

namespace GGame.NetWork
{
    [Serializable, ProtoContract(Name = @"CSHeartBeat")]
    public partial class CSHeartBeat : ClientToServerPacketBase
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
