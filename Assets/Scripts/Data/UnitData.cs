using System.Collections.Generic;
using UnityEngine;
using ZFramework;
using ZFramework.Runtime;

public class UnitData : Singleton<UnitData>
{ 
    /// <summary>
    /// PlayerId
    /// </summary>
    public int PlayerId { get; set; }

    /// <summary>
    /// 血量
    /// </summary>
    public int HPValue { get; set; }

    /// <summary>
    /// 位置
    /// </summary>
    public Vector3 Postion { get; set; }

    public Vector3 eulerAngles { get; set; }

    /// <summary>
    /// 是否加速
    /// </summary>
    public bool IsAddSpeed { get; set; }

    /// <summary>
    /// 是否攻击
    /// </summary>
    public bool IsAttack { get; set; }

    /// <summary>
    /// 积分
    /// </summary>
    public int JiFen { get; set; }
}

public class UnitDatas : Singleton<UnitDatas>
{
    public Dictionary<int, UnitData> allPlayer;

    public UnitData heroUnitData { get; set; }

    public UnitDatas()
    {
        allPlayer = new Dictionary<int, UnitData>();
    }


    public void AddPlayerUnit(UnitData unit)
    {
        if (allPlayer.ContainsKey(unit.PlayerId))
        {
            Log.Info("该玩家已经存在");
        }

        allPlayer.Add(unit.PlayerId, unit);
    }

}