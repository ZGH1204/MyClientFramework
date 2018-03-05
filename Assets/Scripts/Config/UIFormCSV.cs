public class UIFormCSV
{
    /// <summary>
    /// 界面编号
    /// <summary>
    public int Id { get; set; }

    /// <summary>
    /// 备注
    /// <summary>
    public string Info { get; set; }

    /// <summary>
    /// 资源名称
    /// <summary>
    public string AssetName { get; set; }

    /// <summary>
    /// 界面组名称
    /// <summary>
    public string UIGroupName { get; set; }

    /// <summary>
    /// 是否允许多个界面实例
    /// <summary>
    public bool AllowMultiInstance { get; set; }

    /// <summary>
    /// 是否暂停被其覆盖的界面
    /// <summary>
    public bool PauseCoveredUIForm { get; set; }
}