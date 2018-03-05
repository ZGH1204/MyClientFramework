using ZFramework.Event;
using ZFramework.Network;

namespace ZFramework.Runtime
{
    /// <summary>
    /// 发送网络消息包事件。
    /// </summary>
    public sealed class NetworkSendPacketEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(NetworkSendPacketEventArgs).GetHashCode();

        /// <summary>
        /// 初始化发送网络消息包事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public NetworkSendPacketEventArgs(ZFramework.Network.NetworkSendPacketEventArgs e)
        {
            NetworkChannel = e.NetworkChannel;
            BytesSent = e.BytesSent;
            UserData = e.UserData;
        }

        /// <summary>
        /// 获取发送网络消息包事件编号。
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
        /// 获取已发送字节数。
        /// </summary>
        public int BytesSent
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