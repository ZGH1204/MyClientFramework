using System;

namespace ZFramework.Network
{
    /// <summary>
    /// 网络连接关闭事件。
    /// </summary>
    public sealed class NetworkClosedEventArgs : EventArgs
    {
        /// <summary>
        /// 初始化网络连接关闭事件的新实例。
        /// </summary>
        /// <param name="networkChannel">网络频道。</param>
        public NetworkClosedEventArgs(INetworkChannel networkChannel)
        {
            NetworkChannel = networkChannel;
        }

        /// <summary>
        /// 获取网络频道。
        /// </summary>
        public INetworkChannel NetworkChannel
        {
            get;
            private set;
        }
    }
}