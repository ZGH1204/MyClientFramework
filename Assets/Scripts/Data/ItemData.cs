using GGame.NetWork;
using System.Collections.Generic;
using ZFramework;
using ZFramework.Runtime;

public class ItemData : Singleton<ItemData>
{
    private Dictionary<int, int> m_allItemDic;
    private Dictionary<int, int> m_EquipDic;
    private int m_Grade;
    private int m_KillNum;


    /// <summary>
    /// 所有的物品
    /// </summary>
    public Dictionary<int, int> allItemDic
    {
        get
        {
            return m_allItemDic;
        }
    }

    /// <summary>
    /// 所有的物品
    /// </summary>
    public Dictionary<int, int> allEquipDic
    {
        get
        {
            return m_EquipDic;
        }
    }

    /// <summary>
    /// 积分
    /// </summary>
    public int grade
    {
        get { return m_Grade; }
        set { m_Grade = value; }
    }

    /// <summary>
    /// 击杀数量
    /// </summary>
    public int killNum
    {
        get { return m_KillNum; }
        set { m_KillNum = value; }
    }

    public ItemData()
    {
        m_allItemDic = new Dictionary<int, int>();
        m_EquipDic = new Dictionary<int, int>();

        m_Grade = 0;
    }
     
    public void AddItem(int id, int num = 1)
    {
        if (m_allItemDic.ContainsKey(id))
        {
            m_allItemDic[id] += num;
        }
        else
        {
            m_allItemDic.Add(id, num);
        }
    }

    public void AddItem(Dictionary<int, int> items)
    {
        foreach (var item in items.Keys)
        {
            AddItem(item, items[item]);
        }
    }

    public int GetRareItemNum()
    {
        int num = 0;
        foreach (var id in m_allItemDic.Keys)
        {
            var info = GGame.GameEntry.DateTable.GetTypeById<DropItemCSV>(DataTableName.DropItem, id);
            if (info.Level == 2)
            {
                num += m_allItemDic[id];
            }
        }

        return num;
    }

    public int GetItemNum()
    {
        int num = 0;
        foreach (var id in m_allItemDic.Keys)
        {
            num += m_allItemDic[id];
        }

        return num;
    }

    public void AddGrade(int num)
    {
        m_Grade += num;
    }

    public void AddKillNum(int num)
    {
        m_KillNum += num;
    }

    public void AddEquip(int id, int num = 1)
    {
        if (m_EquipDic.ContainsKey(id))
        {
            m_EquipDic[id] += num;
        }
        else
        {
            m_EquipDic.Add(id, num);
        }
         
        GetAddHPByEquip();
    }

    int GetAddHPByEquip()
    {
        int num = 0;
        foreach (var item in m_EquipDic.Keys)
        {
            var info = GGame.GameEntry.DateTable.GetTypeById<EquipCSV>(DataTableName.Equip, item);
            num += info.BenefitNum[0] * m_EquipDic[item];
        }

        var player = PlayerManager.Instance.GetHeroPlayer();
        GGame.GameEntry.Network.GetNetworkChannel("Battle").Send<CSPlayerInfo>(new CSPlayerInfo()
        {
            PlayerId = player.playerUnitData.PlayerId,
            CurHP = player.hPComponent.GetCurHp(),
            Grade = 0,
            KillNPCNum = 0,
            KillPlayerNum = 0,
            MaxHP = num + 1000f
        });
         
        return num;
    }

    public void ExchangeGrade()
    {
        int num = 0;
        foreach (var item in m_allItemDic.Keys)
        {
            var info = GGame.GameEntry.DateTable.GetTypeById<DropItemCSV>(DataTableName.DropItem, item);
            num += info.Grade * m_allItemDic[item];
        }

        grade += num;

        m_allItemDic.Clear();
    }
     
}