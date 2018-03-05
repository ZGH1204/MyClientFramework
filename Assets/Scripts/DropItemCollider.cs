using System.Collections.Generic;
using UnityEngine;
using ZFramework.Runtime;

public class DropItemCollider : MonoBehaviour
{
    public int treasureBoxId;

    [HideInInspector]
    public List<int> itemId;

    List<int> m_PickItemId = new List<int>();
    static object m_LockObj = new object();
    

    private void Start()
    {
       var info = GGame.GameEntry.DateTable.GetTypeById<TreasureBoxCSV>(DataTableName.TreasureBox, treasureBoxId);
        
        foreach (var item in info.ItemList)
        {
            itemId.Add(item);
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    /// <summary>
    /// 当进入触发器
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter(Collider collider)
    {
        PlayerComponent player = collider.gameObject.GetComponent<PlayerComponent>();

        if (player != null && player.playerUnitData.PlayerId == GGame.GameEntry.Network.GetNetworkChannel("Battle").LocalPort)
        {
            GGame.GameEntry.Event.Fire(this, new DropItemEventArgs(itemId));
        }
    }

    /// <summary>
    /// 当退出触发器
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerExit(Collider collider)
    {
        PlayerComponent player = collider.gameObject.GetComponent<PlayerComponent>();

        if (player != null && player.playerUnitData.PlayerId == GGame.GameEntry.Network.GetNetworkChannel("Battle").LocalPort)
        {
            GGame.GameEntry.Event.Fire(this, new DropItemEventArgs(null));
        }
    }

    /// <summary>
    /// 捡物品
    /// </summary>
    /// <param name="id"></param>
    public bool PickItem(int id)
    {
        lock (m_LockObj)
        {
            if (itemId.Contains(id))
            {
                m_PickItemId.Add(id);
                itemId.Remove(id);
            }
            else
            {
                Debug.Log("不存在物品id： " + id);
                return false;
            }
        }
        return true;
    }
}