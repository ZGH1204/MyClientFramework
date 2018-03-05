using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipCSV
{
	/// <summary>
	/// 装备id
	/// <summary>
	public int Id { get; set; }

	/// <summary>
	/// 装备信息
	/// <summary>
	public string Info { get; set; }

	/// <summary>
	/// 装备名字
	/// <summary>
	public string Name { get; set; }

	/// <summary>
	/// 购买此装备需要的积分
	/// <summary>
	public int NeedGrade { get; set; }

	/// <summary>
	/// 加成属性id
	/// <summary>
	public List<int> BenefitType { get; set; }

	/// <summary>
	/// 加成数量
	/// <summary>
	public List<int> BenefitNum { get; set; }
}