using System;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBoxCSV
{
	/// <summary>
	/// 宝箱id
	/// <summary>
	public int Id { get; set; }

	/// <summary>
	/// 宝箱描述
	/// <summary>
	public string Info { get; set; }

	/// <summary>
	/// 宝箱名字
	/// <summary>
	public string Name { get; set; }

	/// <summary>
	/// 宝箱位置编号
	/// <summary>
	public int Position { get; set; }

	/// <summary>
	/// 包含的掉落物品id
	/// <summary>
	public List<int> ItemList { get; set; }
}