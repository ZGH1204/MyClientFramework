using System;
using System.Collections.Generic;
using UnityEngine;

public class DropItemCSV
{
	/// <summary>
	/// 物品id
	/// <summary>
	public int Id { get; set; }

	/// <summary>
	/// 物品信息
	/// <summary>
	public string Info { get; set; }

	/// <summary>
	/// 物品名字
	/// <summary>
	public string Name { get; set; }

	/// <summary>
	/// icon路径
	/// <summary>
	public string IconPath { get; set; }

	/// <summary>
	/// 可兑换积分
	/// <summary>
	public int Grade { get; set; }

	/// <summary>
	/// 等级
	/// <summary>
	public int Level { get; set; }
}