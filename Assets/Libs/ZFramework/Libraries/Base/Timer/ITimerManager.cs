using System;

namespace ZFramework.Timer
{
    /// <summary>
    /// 定时器管理器
    /// </summary>
    public interface ITimerManager
    {
        /// <summary>
        /// 增加定时器
        /// </summary>
        /// <param name="timerId"></param>
        /// <param name="cd"></param>
        /// <param name="action"></param>
        ICooldownTimer AddTimer(int timerId, float cd, Action action);

        /// <summary>
        /// 增加定时器
        /// </summary>
        /// <param name="timerId"></param>
        /// <param name="cd"></param>
        /// <param name="action"></param>
        ICooldownTimer AddTimer(int timerId, float cd, Action<object> action, object param);

        /// <summary>
        /// 增加定时器
        /// </summary>
        /// <param name="timerId"></param>
        /// <param name="cd"></param>
        /// <param name="action"></param>
        ICooldownTimer AddOnceTimer(int timerId, float cd, Action action);

        /// <summary>
        /// 增加定时器
        /// </summary>
        /// <param name="timerId"></param>
        /// <param name="cd"></param>
        /// <param name="action"></param>
        ICooldownTimer AddOnceTimer(int timerId, float cd, Action<object> action, object param);

        /// <summary>
        /// 检查是否存在定时器
        /// </summary>
        bool HasTimer(int timerId);

        /// <summary>
        /// 修改定时器
        /// </summary>
        /// <param name="timerId"></param>
        /// <param name="cd"></param>
        /// <param name="action"></param>
        void ModifyTimer(int timerId, float cd, Action action);

        /// <summary>
        /// 修改定时器
        /// </summary>
        /// <param name="timerId"></param>
        /// <param name="cd"></param>
        /// <param name="action"></param>
        void ModifyTimer(int timerId, float cd, Action<object> action, object param);

        /// <summary>
        /// 开始定时器
        /// </summary>
        /// <param name="timerId"></param>
        void StartTimer(int timerId);

        /// <summary>
        /// 暂停计时器
        /// </summary>
        /// <param name="timerId"></param>
        void PauseTimer(int timerId);

        /// <summary>
        /// 继续定时器
        /// </summary>
        /// <param name="timerId"></param>
        void ContinueTimer(int timerId);

        /// <summary>
        /// 移除定时器
        /// </summary>
        /// <param name="timerId"></param>
        void RemoveTimer(int timerId);

        /// <summary>
        /// 移除所有定时器
        /// </summary>
        void RemoveAllTimer();
    }
}