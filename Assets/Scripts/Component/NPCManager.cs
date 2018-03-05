using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFramework;
using ZFramework.Runtime;


public class NPCManager : Singleton<NPCManager>
{

    public BattleUIForm battleUIForm;

    public NPCManager()
    {

    }

    /// <summary>
    /// 增加血条
    /// </summary>
    /// <returns></returns>
    public HPComponent AddNPCHPUI(NPCComponent npc)
    { 
        var hp = battleUIForm.AddNPCHPUI(npc);

        return hp;
    }

    public void DeadNPC(NPCComponent npc)
    {
        npc.SetEnable(false);

        if (npc.attackHero.GetComponent<PlayerComponent>().playerUnitData.PlayerId == PlayerManager.Instance.GetHeroPlayer().playerUnitData.PlayerId)
        {
            ItemData.Instance.killNum++;
            ItemData.Instance.grade += 50;

            GGame.GameEntry.Event.Fire(this, new BattleUIEventArgs());
        }
       
        GGame.GameEntry.Cooldown.AddTimer(npc.GetHashCode(), 15f, () => {
            npc.SetEnable(true);

            GGame.GameEntry.Cooldown.RemoveTimer(npc.GetHashCode());
        }).Start();
    }
}
