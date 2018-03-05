namespace ZFramework.UI
{
    internal partial class UIManager
    {
        /// <summary>
        /// 界面实例对象。
        /// </summary>
        private sealed class UIFormInstanceObject
        {
            private readonly string m_Name;
            private readonly object m_Target;
            private readonly object m_UIFormAsset;
            private readonly IUIFormHelper m_UIFormHelper;

            /// <summary>
            /// 获取对象名称。
            /// </summary>
            public string Name
            {
                get
                {
                    return m_Name;
                }
            }

            /// <summary>
            /// 获取对象。
            /// </summary>
            public object Target
            {
                get
                {
                    return m_Target;
                }
            }

            public UIFormInstanceObject(string name, object uiFormAsset, object uiFormInstance, IUIFormHelper uiFormHelper)
            {
                if (uiFormAsset == null)
                {
                    Log.Error("UI form asset is invalid.");
                }

                if (uiFormHelper == null)
                {
                    Log.Error("UI form helper is invalid.");
                }

                m_Name = name;
                m_Target = uiFormInstance;
                m_UIFormAsset = uiFormAsset;
                m_UIFormHelper = uiFormHelper;
            }

            public void Release()
            {
                m_UIFormHelper.ReleaseUIForm(m_UIFormAsset, Target);
            }
        }
    }
}