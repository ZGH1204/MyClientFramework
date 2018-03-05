using ZFramework.Event;
using ZFramework.Network;

namespace ZFramework.Runtime
{
    /// <summary>
    /// 网络错误事件。
    /// </summary>
    public sealed class NetworkErrorEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(NetworkErrorEventArgs).GetHashCode();

        /// <summary>
        /// 初始化网络错误事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public NetworkErrorEventArgs(ZFramework.Network.NetworkErrorEventArgs e)
        {
            NetworkChannel = e.NetworkChannel;
            ErrorCode = e.ErrorCode;
            ErrorMessage = e.ErrorMessage;
        }

        /// <summary>
        /// 获取连接错误事件编号。
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
        /// 获取错误码。
        /// </summary>
        public NetworkErrorCode ErrorCode
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
        {
            get;
            private set;
        }
    }
}