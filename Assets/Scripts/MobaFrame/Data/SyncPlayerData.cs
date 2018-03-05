using GGame.NetWork;
using System;
using ZFramework;
using ZFramework.Runtime;

public class SyncPlayerData : Singleton<SyncPlayerData>
{
    /// <summary>
    /// 位置
    /// </summary>
    public Action<SCPositon> syncPositionBack;
    
    public SCPositon position;

    /// <summary>
    /// 同步位置
    /// </summary> 
    public void SyncPosition(SCPositon position)
    {
        if (syncPositionBack != null)
        {
            syncPositionBack.Invoke(position);
        }
    }

    /// <summary>
    /// 创建角色
    /// </summary>
    public Action<SCCreatePlayer> CreatePlayerBack;
     
    /// <summary>
    /// 创建角色
    /// </summary> 
    public void CreatePlayer(SCCreatePlayer player)
    {
        if (CreatePlayerBack != null)
        {
            CreatePlayerBack.Invoke(player);
        }
    }

    /// <summary>
    /// 伤害信息
    /// </summary>
    public Action<SCHurtInfo> HurtInfoBack;

    /// <summary>
    /// 伤害信息
    /// </summary>
    public void HurtInfo(SCHurtInfo info)
    {
        if (HurtInfoBack != null)
        {
            HurtInfoBack.Invoke(info);
        }

    }


    /// <summary>
    /// 玩家信息
    /// </summary>
    public Action<SCPlayerInfo> PlayerInfoBack;

    /// <summary>
    /// 玩家信息
    /// </summary>
    public void PlayerInfo(SCPlayerInfo info)
    {
        if (PlayerInfoBack != null)
        {
            PlayerInfoBack.Invoke(info);
        }

    }

}