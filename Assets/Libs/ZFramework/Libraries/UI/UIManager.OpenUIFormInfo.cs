 
namespace ZFramework.UI
{
    internal partial class UIManager
    {
        private sealed class OpenUIFormInfo
        {
            private readonly int m_SerialId;
            private readonly UIGroup m_UIGroup; 
            private readonly object m_UserData;

            public OpenUIFormInfo(int serialId, UIGroup uiGroup, object userData)
            {
                m_SerialId = serialId;
                m_UIGroup = uiGroup; 
                m_UserData = userData;
            }

            public int SerialId
            {
                get
                {
                    return m_SerialId;
                }
            }

            public UIGroup UIGroup
            {
                get
                {
                    return m_UIGroup;
                }
            }
             
            public object UserData
            {
                get
                {
                    return m_UserData;
                }
            }
        }
    }
}