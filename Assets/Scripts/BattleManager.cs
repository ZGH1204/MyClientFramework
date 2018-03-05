using GGame.NetWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFramework;
using ZFramework.Network;
using ZFramework.Runtime;


public class BattleManager : Singleton<BattleManager>
{

    INetworkChannel channel01;

    public BattleManager()
    {
        channel01 = GGame.GameEntry.Network.GetNetworkChannel("Battle");
    }

    /// <summary>
    /// 攻击
    /// </summary>
    public void Attack(PlayerComponent player1, PlayerComponent player2)
    { 
        player2.hPComponent.AddAttackHp(200);
        float curHp = player2.hPComponent.GetCurHp();

        channel01.Send<CSHurtInfo>(new CSHurtInfo() {
            AttackerId = player1.playerUnitData.PlayerId,
            HurterId = player2.playerUnitData.PlayerId,
            HurterCurHp = curHp
        });
    }

    /// <summary>
    /// 攻击
    /// </summary>
    public void Attack(PlayerComponent player, NPCComponent npc)
    {
        npc.hPComponent.AddAttackHp(200);
        float curHp = npc.hPComponent.GetCurHp();
         
        channel01.Send<CSHurtInfo>(new CSHurtInfo()
        {
            AttackerId = player.playerUnitData.PlayerId,
            HurterId = npc.nPCId,
            HurterCurHp = curHp
        });
    }

    /// <summary>
    /// 攻击
    /// </summary>
    public void Attack(NPCComponent npc, PlayerComponent player)
    {
        player.hPComponent.AddAttackHp(200);
        float curHp = player.hPComponent.GetCurHp();
         
        channel01.Send<CSHurtInfo>(new CSHurtInfo()
        {
            AttackerId = npc.nPCId,
            HurterId = player.playerUnitData.PlayerId,
            HurterCurHp = curHp
        });
    }

}
