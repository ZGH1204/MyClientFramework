namespace ZFramework
{
    public static partial class Log
    {
        /// <summary>
        /// 日志辅助器接口。
        /// </summary>
        public interface ILogHelper
        {
            /// <summary>
            /// 记录日志。
            /// </summary>
            /// <param name="level">日志等级。</param>
            /// <param name="message">日志内容。</param>
            void Log(LogLevel level, object message);
        }
    }
}