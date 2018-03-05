using ZFramework;
using ZFramework.Network;

namespace GGame.NetWork
{
    /// <summary>
    /// 心跳返回
    /// </summary>
    public class SCHeartBeatHandler : PacketHandlerBase
    {
        public override int Id
        {
            get
            {
                return (int)PacketId.HeartBeat;
            }
        }

        public override void Handle(object sender, Packet packet)
        {
            SCHeartBeat packetImpl = (SCHeartBeat)packet;
            Log.Info("Receive packet '{0}'.", packetImpl.Id.ToString());
        }
    }

    /// <summary>
    /// 登录返回
    /// </summary>
    public class SCLoginHandler : PacketHandlerBase
    {
        public override int Id
        {
            get
            {
                return (int)PacketId.Login;
            }
        }

        public override void Handle(object sender, Packet packet)
        {
            SCLogin packetImpl = (SCLogin)packet;
            Log.Info(packetImpl.PlayerId);
        }
    }

    /// <summary>
    /// 位置同步返回
    /// </summary>
    public class SCPositonHandler : PacketHandlerBase
    {
        public override int Id
        {
            get
            {
                return (int)PacketId.Position;
            }
        }

        public override void Handle(object sender, Packet packet)
        {
            SCPositon packetImpl = (SCPositon)packet; 
            SyncPlayerData.Instance.SyncPosition(packetImpl);
        }
    }

    /// <summary>
    /// 创建角色
    /// </summary>
    public class SCCreatePlayerHandler : PacketHandlerBase
    {
        public override int Id
        {
            get
            {
                return (int)PacketId.CreatePlayer;
            }
        }

        public override void Handle(object sender, Packet packet)
        {
            SCCreatePlayer info = (SCCreatePlayer)packet;

            SyncPlayerData.Instance.CreatePlayer(info);
        }
    }

    /// <summary>
    /// 伤害信息  PlayerInfo
    /// </summary>
    public class SCHurtInfoHandler : PacketHandlerBase
    {
        public override int Id
        {
            get
            {
                return (int)PacketId.HurtInfo;
            }
        }

        public override void Handle(object sender, Packet packet)
        {
            SCHurtInfo info = (SCHurtInfo)packet;

            SyncPlayerData.Instance.HurtInfo(info);
        }
    }

    /// <summary>
    /// 玩家信息  
    /// </summary>
    public class SCPlayerInfoHandler : PacketHandlerBase
    {
        public override int Id
        {
            get
            {
                return (int)PacketId.PlayerInfo;
            }
        }

        public override void Handle(object sender, Packet packet)
        {
            SCPlayerInfo info = (SCPlayerInfo)packet;

            SyncPlayerData.Instance.PlayerInfo(info);
        }
    }
}
