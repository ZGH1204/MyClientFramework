 
namespace ZFramework.UI
{
    internal partial class UIManager
    {
        private partial class UIGroup
        {
            /// <summary>
            /// 界面组界面信息。
            /// </summary>
            private sealed class UIFormInfo
            {
                private readonly IUIForm m_UIForm;
                private bool m_Paused; 

                /// <summary>
                /// 初始化界面组界面信息的新实例。
                /// </summary>
                /// <param name="uiForm">界面。</param>
                public UIFormInfo(IUIForm uiForm)
                {
                    if (uiForm == null)
                    {
                        Log.Error("UI form is invalid.");
                    }

                    m_UIForm = uiForm;
                    m_Paused = true; 
                }

                /// <summary>
                /// 获取界面。
                /// </summary>
                public IUIForm UIForm
                {
                    get
                    {
                        return m_UIForm;
                    }
                }

                /// <summary>
                /// 获取或设置界面是否暂停。
                /// </summary>
                public bool Paused
                {
                    get
                    {
                        return m_Paused;
                    }
                    set
                    {
                        m_Paused = value;
                    }
                }
 
            }
        }
    }
}