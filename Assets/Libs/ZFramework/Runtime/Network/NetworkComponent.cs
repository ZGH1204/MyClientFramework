using UnityEngine;
using ZFramework.Network;

namespace ZFramework.Runtime
{
    /// <summary>
    /// 网络组件。
    /// </summary>
    [DisallowMultipleComponent]
    public sealed partial class NetworkComponent : ZFrameworkComponent
    {
        private INetworkManager m_NetworkManager = null;
        private EventComponent m_EventComponent = null;

        /// <summary>
        /// 获取网络频道数量。
        /// </summary>
        public int NetworkChannelCount
        {
            get
            {
                return m_NetworkManager.NetworkChannelCount;
            }
        }

        /// <summary>
        /// 设置事件组件
        /// </summary>
        public EventComponent EventComponent
        {
            set
            {
                m_EventComponent = value;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_NetworkManager = ZFrameworkEntry.GetModule<INetworkManager>();
            if (m_NetworkManager == null)
            {
                Log.Fatal("Network manager is invalid.");
                return;
            }

            m_NetworkManager.NetworkConnected += OnNetworkConnected;
            m_NetworkManager.NetworkClosed += OnNetworkClosed;
            m_NetworkManager.NetworkSendPacket += OnNetworkSendPacket;
            m_NetworkManager.NetworkMissHeartBeat += OnNetworkMissHeartBeat;
            m_NetworkManager.NetworkError += OnNetworkError;
            m_NetworkManager.NetworkCustomError += OnNetworkCustomError;
        }

        private void Start()
        {
        }

        /// <summary>
        /// 检查是否存在网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>是否存在网络频道。</returns>
        public bool HasNetworkChannel(string name)
        {
            return m_NetworkManager.HasNetworkChannel(name);
        }

        /// <summary>
        /// 获取网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>要获取的网络频道。</returns>
        public INetworkChannel GetNetworkChannel(string name)
        {
            return m_NetworkManager.GetNetworkChannel(name);
        }

        /// <summary>
        /// 获取所有网络频道。
        /// </summary>
        /// <returns>所有网络频道。</returns>
        public INetworkChannel[] GetAllNetworkChannels()
        {
            return m_NetworkManager.GetAllNetworkChannels();
        }

        /// <summary>
        /// 创建网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <param name="networkChannelHelper">网络频道辅助器。</param>
        /// <returns>要创建的网络频道。</returns>
        public INetworkChannel CreateNetworkChannel(string name, INetworkChannelHelper networkChannelHelper)
        {
            return m_NetworkManager.CreateNetworkChannel(name, networkChannelHelper);
        }

        /// <summary>
        /// 销毁网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>是否销毁网络频道成功。</returns>
        public bool DestroyNetworkChannel(string name)
        {
            return m_NetworkManager.DestroyNetworkChannel(name);
        }

        private void OnNetworkConnected(object sender, ZFramework.Network.NetworkConnectedEventArgs e)
        {
            m_EventComponent.Fire(this, new NetworkConnectedEventArgs(e));
        }

        private void OnNetworkClosed(object sender, ZFramework.Network.NetworkClosedEventArgs e)
        {
            m_EventComponent.Fire(this, new NetworkClosedEventArgs(e));
        }

        private void OnNetworkSendPacket(object sender, ZFramework.Network.NetworkSendPacketEventArgs e)
        {
            m_EventComponent.Fire(this, new NetworkSendPacketEventArgs(e));
        }

        private void OnNetworkMissHeartBeat(object sender, ZFramework.Network.NetworkMissHeartBeatEventArgs e)
        {
            m_EventComponent.Fire(this, new NetworkMissHeartBeatEventArgs(e));
        }

        private void OnNetworkError(object sender, ZFramework.Network.NetworkErrorEventArgs e)
        {
            m_EventComponent.Fire(this, new NetworkErrorEventArgs(e));
        }

        private void OnNetworkCustomError(object sender, ZFramework.Network.NetworkCustomErrorEventArgs e)
        {
            m_EventComponent.Fire(this, new NetworkCustomErrorEventArgs(e));
        }

        /// <summary>
        /// 销毁网络
        /// </summary>
        private void OnDestroy()
        {
           var all = GetAllNetworkChannels();
            for (int i = 0; i < all.Length; i++)
            {
                DestroyNetworkChannel(all[i].Name);
            }
        } 
    }
}