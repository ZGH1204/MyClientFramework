using System;
using System.Collections.Generic;
using ZFramework.Resource;

namespace ZFramework.UI
{
    /// <summary>
    /// 界面管理器。
    /// </summary>
    internal sealed partial class UIManager : ZFrameworkModule, IUIManager
    {
        private readonly Dictionary<string, UIGroup> m_UIGroups;
        private readonly List<int> m_UIFormsBeingLoaded;
        private readonly List<string> m_UIFormAssetNamesBeingLoaded;
        private readonly HashSet<int> m_UIFormsToReleaseOnLoad;
        private readonly LinkedList<IUIForm> m_RecycleQueue;
        private readonly LoadAssetCallbackEvent m_LoadAssetCallbacks;
        private IUIFormHelper m_UIFormHelper;
        private IResourceManager m_ResourceManager; 
        private EventHandler<OpenUIFormSuccessEventArgs> m_OpenUIFormSuccessEventHandler;
        private EventHandler<OpenUIFormFailureEventArgs> m_OpenUIFormFailureEventHandler;
        private EventHandler<OpenUIFormUpdateEventArgs> m_OpenUIFormUpdateEventHandler;
        private EventHandler<OpenUIFormDependencyAssetEventArgs> m_OpenUIFormDependencyAssetEventHandler;
        private EventHandler<CloseUIFormCompleteEventArgs> m_CloseUIFormCompleteEventHandler;
         
        /// <summary>
        /// 初始化界面管理器的新实例。
        /// </summary>
        public UIManager()
        {
            m_UIGroups = new Dictionary<string, UIGroup>();
            m_UIFormsBeingLoaded = new List<int>();
            m_UIFormAssetNamesBeingLoaded = new List<string>();
            m_UIFormsToReleaseOnLoad = new HashSet<int>();
            m_RecycleQueue = new LinkedList<IUIForm>();
            m_LoadAssetCallbacks = new LoadAssetCallbackEvent(LoadUIFormSuccessCallback, LoadUIFormFailureCallback, LoadUIFormUpdateCallback, LoadUIFormDependencyAssetCallback);
            m_UIFormHelper = null;
            m_ResourceManager = null; 
            m_OpenUIFormSuccessEventHandler = null;
            m_OpenUIFormFailureEventHandler = null;
            m_OpenUIFormUpdateEventHandler = null;
            m_OpenUIFormDependencyAssetEventHandler = null;
            m_CloseUIFormCompleteEventHandler = null;
        }

        /// <summary>
        /// 获取界面组数量。
        /// </summary>
        public int UIGroupCount
        {
            get
            {
                return m_UIGroups.Count;
            }
        }

        /// <summary>
        /// 打开界面成功事件。
        /// </summary>
        public event EventHandler<OpenUIFormSuccessEventArgs> OpenUIFormSuccess
        {
            add
            {
                m_OpenUIFormSuccessEventHandler += value;
            }
            remove
            {
                m_OpenUIFormSuccessEventHandler -= value;
            }
        }

        /// <summary>
        /// 打开界面失败事件。
        /// </summary>
        public event EventHandler<OpenUIFormFailureEventArgs> OpenUIFormFailure
        {
            add
            {
                m_OpenUIFormFailureEventHandler += value;
            }
            remove
            {
                m_OpenUIFormFailureEventHandler -= value;
            }
        }

        /// <summary>
        /// 打开界面更新事件。
        /// </summary>
        public event EventHandler<OpenUIFormUpdateEventArgs> OpenUIFormUpdate
        {
            add
            {
                m_OpenUIFormUpdateEventHandler += value;
            }
            remove
            {
                m_OpenUIFormUpdateEventHandler -= value;
            }
        }

        /// <summary>
        /// 打开界面时加载依赖资源事件。
        /// </summary>
        public event EventHandler<OpenUIFormDependencyAssetEventArgs> OpenUIFormDependencyAsset
        {
            add
            {
                m_OpenUIFormDependencyAssetEventHandler += value;
            }
            remove
            {
                m_OpenUIFormDependencyAssetEventHandler -= value;
            }
        }

        /// <summary>
        /// 关闭界面完成事件。
        /// </summary>
        public event EventHandler<CloseUIFormCompleteEventArgs> CloseUIFormComplete
        {
            add
            {
                m_CloseUIFormCompleteEventHandler += value;
            }
            remove
            {
                m_CloseUIFormCompleteEventHandler -= value;
            }
        }

        /// <summary>
        /// 界面管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            while (m_RecycleQueue.Count > 0)
            {
                IUIForm uiForm = m_RecycleQueue.First.Value;
                m_RecycleQueue.RemoveFirst();
                uiForm.OnRecycle();
            }

            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                uiGroup.Value.Update(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 关闭并清理界面管理器。
        /// </summary>
        internal override void Shutdown()
        {
            CloseAllLoadedUIForms();
            m_UIGroups.Clear();
            m_UIFormsBeingLoaded.Clear();
            m_UIFormAssetNamesBeingLoaded.Clear();
            m_UIFormsToReleaseOnLoad.Clear();
            m_RecycleQueue.Clear();
        }

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceManager">资源管理器。</param>
        public void SetResourceManager(IResourceManager resourceManager)
        {
            if (resourceManager == null)
            {
                Log.Error("Resource manager is invalid.");
            }

            m_ResourceManager = resourceManager;
        }

        /// <summary>
        /// 设置界面辅助器。
        /// </summary>
        /// <param name="uiFormHelper">界面辅助器。</param>
        public void SetUIFormHelper(IUIFormHelper uiFormHelper)
        {
            if (uiFormHelper == null)
            {
                Log.Error("UI form helper is invalid.");
            }

            m_UIFormHelper = uiFormHelper;
        }

        /// <summary>
        /// 是否存在界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>是否存在界面组。</returns>
        public bool HasUIGroup(string uiGroupName)
        {
            if (string.IsNullOrEmpty(uiGroupName))
            {
                Log.Error("UI group name is invalid.");
            }

            return m_UIGroups.ContainsKey(uiGroupName);
        }

        /// <summary>
        /// 获取界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>要获取的界面组。</returns>
        public IUIGroup GetUIGroup(string uiGroupName)
        {
            if (string.IsNullOrEmpty(uiGroupName))
            {
                Log.Error("UI group name is invalid.");
            }

            UIGroup uiGroup = null;
            if (m_UIGroups.TryGetValue(uiGroupName, out uiGroup))
            {
                return uiGroup;
            }

            return null;
        }

        /// <summary>
        /// 获取所有界面组。
        /// </summary>
        /// <returns>所有界面组。</returns>
        public IUIGroup[] GetAllUIGroups()
        {
            int index = 0;
            IUIGroup[] uiGroups = new IUIGroup[m_UIGroups.Count];
            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                uiGroups[index++] = uiGroup.Value;
            }

            return uiGroups;
        }

        /// <summary>
        /// 增加界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="uiGroupHelper">界面组辅助器。</param>
        /// <returns>是否增加界面组成功。</returns>
        public bool AddUIGroup(string uiGroupName, IUIGroupHelper uiGroupHelper)
        {
            return AddUIGroup(uiGroupName, 0, uiGroupHelper);
        }

        /// <summary>
        /// 增加界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="uiGroupDepth">界面组深度。</param>
        /// <param name="uiGroupHelper">界面组辅助器。</param>
        /// <returns>是否增加界面组成功。</returns>
        public bool AddUIGroup(string uiGroupName, int uiGroupDepth, IUIGroupHelper uiGroupHelper)
        {
            if (string.IsNullOrEmpty(uiGroupName))
            {
                Log.Error("UI group name is invalid.");
            }

            if (uiGroupHelper == null)
            {
                Log.Error("UI group helper is invalid.");
            }

            if (HasUIGroup(uiGroupName))
            {
                return false;
            }

            m_UIGroups.Add(uiGroupName, new UIGroup(uiGroupName, uiGroupDepth, uiGroupHelper));

            return true;
        }

        /// <summary>
        /// 是否存在界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>是否存在界面。</returns>
        public bool HasUIForm(int serialId)
        {
            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                if (uiGroup.Value.HasUIForm(serialId))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 是否存在界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>是否存在界面。</returns>
        public bool HasUIForm(string uiFormAssetName)
        {
            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                Log.Error("UI form asset name is invalid.");
            }

            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                if (uiGroup.Value.HasUIForm(uiFormAssetName))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>要获取的界面。</returns>
        public IUIForm GetUIForm(int serialId)
        {
            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                IUIForm uiForm = uiGroup.Value.GetUIForm(serialId);
                if (uiForm != null)
                {
                    return uiForm;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>要获取的界面。</returns>
        public IUIForm GetUIForm(string uiFormAssetName)
        {
            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                Log.Error("UI form asset name is invalid.");
            }

            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                IUIForm uiForm = uiGroup.Value.GetUIForm(uiFormAssetName);
                if (uiForm != null)
                {
                    return uiForm;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>要获取的界面。</returns>
        public IUIForm[] GetUIForms(string uiFormAssetName)
        {
            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                Log.Error("UI form asset name is invalid.");
            }

            List<IUIForm> uiForms = new List<IUIForm>();
            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                uiForms.AddRange(uiGroup.Value.GetUIForms(uiFormAssetName));
            }

            return uiForms.ToArray();
        }

        /// <summary>
        /// 获取所有已加载的界面。
        /// </summary>
        /// <returns>所有已加载的界面。</returns>
        public IUIForm[] GetAllLoadedUIForms()
        {
            List<IUIForm> uiForms = new List<IUIForm>();
            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                uiForms.AddRange(uiGroup.Value.GetAllUIForms());
            }

            return uiForms.ToArray();
        }

        /// <summary>
        /// 获取所有正在加载界面的序列编号。
        /// </summary>
        /// <returns>所有正在加载界面的序列编号。</returns>
        public int[] GetAllLoadingUIFormSerialIds()
        {
            return m_UIFormsBeingLoaded.ToArray();
        }

        /// <summary>
        /// 是否正在加载界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>是否正在加载界面。</returns>
        public bool IsLoadingUIForm(int serialId)
        {
            return m_UIFormsBeingLoaded.Contains(serialId);
        }

        /// <summary>
        /// 是否正在加载界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>是否正在加载界面。</returns>
        public bool IsLoadingUIForm(string uiFormAssetName)
        {
            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                Log.Error("UI form asset name is invalid.");
            }

            return m_UIFormAssetNamesBeingLoaded.Contains(uiFormAssetName);
        }

        /// <summary>
        /// 是否是合法的界面。
        /// </summary>
        /// <param name="uiForm">界面。</param>
        /// <returns>界面是否合法。</returns>
        public bool IsValidUIForm(IUIForm uiForm)
        {
            if (uiForm == null)
            {
                return false;
            }

            return HasUIForm(uiForm.SerialId);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>界面的序列编号。</returns>
        public bool OpenUIForm(string uiFormAssetName, string uiGroupName)
        {
            return OpenUIForm(uiFormAssetName, uiGroupName, false, null);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        /// <returns>界面的序列编号。</returns>
        public bool OpenUIForm(string uiFormAssetName, string uiGroupName, bool pauseCoveredUIForm)
        {
            return OpenUIForm(uiFormAssetName, uiGroupName, pauseCoveredUIForm, null);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        public bool OpenUIForm(string uiFormAssetName, string uiGroupName, object userData)
        {
            return OpenUIForm(uiFormAssetName, uiGroupName, false, userData);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        public bool OpenUIForm(string uiFormAssetName, string uiGroupName, bool pauseCoveredUIForm, object userData)
        {
            if (m_ResourceManager == null)
            {
                Log.Error("You must set resource manager first.");
            }

            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                Log.Error("UI form asset name is invalid.");
            }

            if (string.IsNullOrEmpty(uiGroupName))
            {
                Log.Error("UI group name is invalid.");
            }
             
            if (IsLoadingUIForm(uiFormAssetName))
            {
                Log.Error("UI group name is loading.");
                return false;
            }

            if (HasUIForm(uiFormAssetName))
            { 
                IUIGroup uiGroup1 = GetUIGroup(uiGroupName);
                IUIForm form = uiGroup1.GetUIForm(uiFormAssetName);
                if (uiGroup1.CurrentUIForm != null && uiGroup1.CurrentUIForm.SerialId != form.SerialId)
                {
                    uiGroup1.CurrentUIForm.OnClose(null);
                }
                uiGroup1.RefocusUIForm(form, userData);
                uiGroup1.Refresh(); 
                return true;
            }
            
            UIGroup uiGroup = (UIGroup)GetUIGroup(uiGroupName);
            if (uiGroup == null)
            {
                Log.Error(string.Format("UI group '{0}' is not exist.", uiGroupName));
            }
            
            //资源是否缓存 
            // ... ... 
            UIFormInstanceObject uiFormInstanceObject = null;
            if (uiFormInstanceObject == null)
            {
                m_UIFormsBeingLoaded.Add(uiFormAssetName.GetHashCode());
                m_UIFormAssetNamesBeingLoaded.Add(uiFormAssetName);
                m_ResourceManager.LoadAsset(uiFormAssetName, m_LoadAssetCallbacks, new OpenUIFormInfo(uiFormAssetName.GetHashCode(), uiGroup, userData));
            }
            else
            {
                InternalOpenUIForm(uiFormAssetName.GetHashCode(), uiFormAssetName, uiGroup, uiFormInstanceObject.Target, false, 0f, userData);
            }

            return true;
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="serialId">要关闭界面的序列编号。</param>
        public void CloseUIForm(int serialId)
        {
            CloseUIForm(serialId, null);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="serialId">要关闭界面的序列编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void CloseUIForm(int serialId, object userData)
        {
            if (IsLoadingUIForm(serialId))
            {
                m_UIFormsToReleaseOnLoad.Add(serialId);
                return;
            }

            IUIForm uiForm = GetUIForm(serialId);
            if (uiForm == null)
            {
                Log.Error(string.Format("Can not find UI form '{0}'.", serialId.ToString()));
            }

            CloseUIForm(uiForm, userData);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiForm">要关闭的界面。</param>
        public void CloseUIForm(IUIForm uiForm)
        {
            CloseUIForm(uiForm, null);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiForm">要关闭的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void CloseUIForm(IUIForm uiForm, object userData)
        {
            if (uiForm == null)
            {
                Log.Error("UI form is invalid.");
            }

            UIGroup uiGroup = (UIGroup)uiForm.UIGroup;
            if (uiGroup == null)
            {
                Log.Error("UI group is invalid.");
            }

            uiGroup.RemoveUIForm(uiForm);
            uiForm.OnClose(userData);
            uiGroup.Refresh();

            if (m_CloseUIFormCompleteEventHandler != null)
            {
                m_CloseUIFormCompleteEventHandler(this, new CloseUIFormCompleteEventArgs(uiForm.SerialId, uiForm.UIFormAssetName, uiGroup, userData));
            }

            m_RecycleQueue.AddLast(uiForm);
        }

        /// <summary>
        /// 关闭所有已加载的界面。
        /// </summary>
        public void CloseAllLoadedUIForms()
        {
            CloseAllLoadedUIForms(null);
        }

        /// <summary>
        /// 关闭所有已加载的界面。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        public void CloseAllLoadedUIForms(object userData)
        {
            IUIForm[] uiForms = GetAllLoadedUIForms();
            foreach (IUIForm uiForm in uiForms)
            {
                CloseUIForm(uiForm, userData);
            }
        }

        /// <summary>
        /// 关闭所有正在加载的界面。
        /// </summary>
        public void CloseAllLoadingUIForms()
        {
            foreach (int serialId in m_UIFormsBeingLoaded)
            {
                m_UIFormsToReleaseOnLoad.Add(serialId);
            }
        }
  
        /// <summary>
        /// 立即打开界面
        /// </summary>
        /// <param name="serialId"></param>
        /// <param name="uiFormAssetName"></param>
        /// <param name="uiGroup"></param>
        /// <param name="uiFormInstance"></param>
        /// <param name="pauseCoveredUIForm"></param>
        /// <param name="isNewInstance"></param>
        /// <param name="duration"></param>
        /// <param name="userData"></param>
        private void InternalOpenUIForm(int serialId, string uiFormAssetName, UIGroup uiGroup, object uiFormInstance, bool isNewInstance, float duration, object userData)
        {
            try
            {
                IUIForm uiForm = m_UIFormHelper.CreateUIForm(uiFormInstance, uiGroup, userData);
                if (uiForm == null)
                {
                    Log.Error("Can not create UI form in helper.");
                }
                if (uiGroup.CurrentUIForm != null && uiGroup.CurrentUIForm.SerialId != uiForm.SerialId)
                {
                    uiGroup.CurrentUIForm.OnClose(null);
                }
            
                uiForm.OnInit(serialId, uiFormAssetName, uiGroup, isNewInstance, userData);
                uiGroup.AddUIForm(uiForm);
                uiForm.OnOpen(userData); 
                uiGroup.Refresh();

                if (m_OpenUIFormSuccessEventHandler != null)
                {
                    m_OpenUIFormSuccessEventHandler(this, new OpenUIFormSuccessEventArgs(uiForm, duration, userData));
                }
            }
            catch (System.Exception exception)
            {
                if (m_OpenUIFormFailureEventHandler != null)
                {
                    m_OpenUIFormFailureEventHandler(this, new OpenUIFormFailureEventArgs(serialId, uiFormAssetName, uiGroup.Name, exception.ToString(), userData));
                    return;
                }

                throw;
            }
        }

        private void LoadUIFormSuccessCallback(string uiFormAssetName, object uiFormAsset, float duration, object userData)
        {
            OpenUIFormInfo openUIFormInfo = (OpenUIFormInfo)userData;
            if (openUIFormInfo == null)
            {
                Log.Error("Open UI form info is invalid.");
            }

            UIFormInstanceObject uiFormInstanceObject = new UIFormInstanceObject(uiFormAssetName, uiFormAsset, m_UIFormHelper.InstantiateUIForm(uiFormAsset), m_UIFormHelper);

            m_UIFormsBeingLoaded.Remove(openUIFormInfo.SerialId);
            m_UIFormAssetNamesBeingLoaded.Remove(uiFormAssetName);
            if (m_UIFormsToReleaseOnLoad.Contains(openUIFormInfo.SerialId))
            {
                Log.Debug("Release UI form '{0}' on loading success.", openUIFormInfo.SerialId.ToString());
                m_UIFormsToReleaseOnLoad.Remove(openUIFormInfo.SerialId);
                m_UIFormHelper.ReleaseUIForm(uiFormAsset, null);
                return;
            }

            InternalOpenUIForm(openUIFormInfo.SerialId, uiFormAssetName, openUIFormInfo.UIGroup, uiFormInstanceObject.Target, true, duration, openUIFormInfo.UserData);
        }

        private void LoadUIFormFailureCallback(string uiFormAssetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            OpenUIFormInfo openUIFormInfo = (OpenUIFormInfo)userData;
            if (openUIFormInfo == null)
            {
                Log.Error("Open UI form info is invalid.");
            }

            m_UIFormsBeingLoaded.Remove(openUIFormInfo.SerialId);
            m_UIFormAssetNamesBeingLoaded.Remove(uiFormAssetName);
            m_UIFormsToReleaseOnLoad.Remove(openUIFormInfo.SerialId);
            string appendErrorMessage = string.Format("Load UI form failure, asset name '{0}', status '{1}', error message '{2}'.", uiFormAssetName, status.ToString(), errorMessage);
            if (m_OpenUIFormFailureEventHandler != null)
            {
                m_OpenUIFormFailureEventHandler(this, new OpenUIFormFailureEventArgs(openUIFormInfo.SerialId, uiFormAssetName, openUIFormInfo.UIGroup.Name, appendErrorMessage, openUIFormInfo.UserData));
                return;
            }

            Log.Error(appendErrorMessage);
        }

        private void LoadUIFormUpdateCallback(string uiFormAssetName, float progress, object userData)
        {
            OpenUIFormInfo openUIFormInfo = (OpenUIFormInfo)userData;
            if (openUIFormInfo == null)
            {
                Log.Error("Open UI form info is invalid.");
            }

            if (m_OpenUIFormUpdateEventHandler != null)
            {
                m_OpenUIFormUpdateEventHandler(this, new OpenUIFormUpdateEventArgs(openUIFormInfo.SerialId, uiFormAssetName, openUIFormInfo.UIGroup.Name, progress, openUIFormInfo.UserData));
            }
        }

        private void LoadUIFormDependencyAssetCallback(string uiFormAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            OpenUIFormInfo openUIFormInfo = (OpenUIFormInfo)userData;
            if (openUIFormInfo == null)
            {
                Log.Error("Open UI form info is invalid.");
            }

            if (m_OpenUIFormDependencyAssetEventHandler != null)
            {
                m_OpenUIFormDependencyAssetEventHandler(this, new OpenUIFormDependencyAssetEventArgs(openUIFormInfo.SerialId, uiFormAssetName, openUIFormInfo.UIGroup.Name, dependencyAssetName, loadedCount, totalCount, openUIFormInfo.UserData));
            }
        }
    }
}