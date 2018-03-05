using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFramework.Event;

public class DropItemEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(DropItemEventArgs).GetHashCode();

    /// <summary>
    ///  捡东西事件
    /// </summary>
    /// <param name="e">内部事件。</param>
    public DropItemEventArgs(List<int> itemId)
    {
        ItemId = itemId;
    }

    /// <summary>
    /// 事件id
    /// </summary>
    public override int Id
    {
        get
        {
            return EventId;
        }
    }

    /// <summary>
    /// 物品id
    /// </summary>
    public List<int> ItemId
    {
        get;
        set;
    }
}