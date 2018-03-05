using ZFramework.Event;
using ZFramework.Network;

namespace ZFramework.Runtime
{
    /// <summary>
    /// 网络连接成功事件。
    /// </summary>
    public sealed class NetworkConnectedEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(NetworkConnectedEventArgs).GetHashCode();

        /// <summary>
        /// 初始化网络连接成功事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public NetworkConnectedEventArgs(ZFramework.Network.NetworkConnectedEventArgs e)
        {
            NetworkChannel = e.NetworkChannel;
            UserData = e.UserData;
        }

        /// <summary>
        /// 获取连接成功事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取网络频道。
        /// </summary>
        public INetworkChannel NetworkChannel
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }
    }
}