using System;
using System.Collections.Generic;

namespace ZFramework.Fsm
{
    /// <summary>
    /// 有限状态机状态基类。
    /// </summary>
    /// <typeparam name="T">有限状态机持有者类型。</typeparam>
    public abstract class FsmState<T> where T : class
    {
        public IFsm<T> fsm;
        private readonly Dictionary<int, FsmEventHandler<T>> m_EventHandlers;

        /// <summary>
        /// 初始化有限状态机状态基类的新实例。
        /// </summary>
        public FsmState()
        {
            m_EventHandlers = new Dictionary<int, FsmEventHandler<T>>();
        }

        /// <summary>
        /// 有限状态机状态初始化时调用。
        /// </summary>
        /// <param name="fsm">有限状态机引用。</param>
        public virtual void OnInit(IFsm<T> fsm)
        {
            this.fsm = fsm;
        }

        /// <summary>
        /// 有限状态机状态进入时调用。
        /// </summary>
        /// <param name="fsm">有限状态机引用。</param>
        public virtual void OnEnter()
        {
        }

        /// <summary>
        /// 有限状态机状态轮询时调用。
        /// </summary>
        /// <param name="fsm">有限状态机引用。</param>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 有限状态机状态离开时调用。
        /// </summary>
        /// <param name="fsm">有限状态机引用。</param>
        /// <param name="isShutdown">是否是关闭有限状态机时触发。</param>
        public virtual void OnLeave(bool isShutdown)
        {
        }

        /// <summary>
        /// 有限状态机状态销毁时调用。
        /// </summary>
        /// <param name="fsm">有限状态机引用。</param>
        public virtual void OnDestroy()
        {
            m_EventHandlers.Clear();
        }

        /// <summary>
        /// 订阅有限状态机事件。
        /// </summary>
        /// <param name="eventId">事件编号。</param>
        /// <param name="eventHandler">有限状态机事件响应函数。</param>
        public void SubscribeEvent(int eventId, FsmEventHandler<T> eventHandler)
        {
            if (eventHandler == null)
            {
                throw new Exception("Event handler is invalid.");
            }

            if (!m_EventHandlers.ContainsKey(eventId))
            {
                m_EventHandlers[eventId] = eventHandler;
            }
            else
            {
                m_EventHandlers[eventId] += eventHandler;
            }
        }

        /// <summary>
        /// 取消订阅有限状态机事件。
        /// </summary>
        /// <param name="eventId">事件编号。</param>
        /// <param name="eventHandler">有限状态机事件响应函数。</param>
        public void UnsubscribeEvent(int eventId, FsmEventHandler<T> eventHandler)
        {
            if (eventHandler == null)
            {
                throw new Exception("Event handler is invalid.");
            }

            if (m_EventHandlers.ContainsKey(eventId))
            {
                m_EventHandlers[eventId] -= eventHandler;
            }
        }

        /// <summary>
        /// 切换当前有限状态机状态。
        /// </summary>
        /// <typeparam name="TState">要切换到的有限状态机状态类型。</typeparam>
        /// <param name="fsm">有限状态机引用。</param>
        public void ChangeState<TState>() where TState : FsmState<T>
        {
            if (fsm == null)
            {
                throw new Exception("FSM is invalid.");
            }

            fsm.ChangeState<TState>();
        }

        /// <summary>
        /// 切换当前有限状态机状态。
        /// </summary>
        /// <param name="fsm">有限状态机引用。</param>
        /// <param name="stateType">要切换到的有限状态机状态类型。</param>
        public void ChangeState(Type stateType)
        {
            if (fsm == null)
            {
                throw new Exception("FSM is invalid.");
            }
            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new Exception(string.Format("State type '{0}' is invalid.", stateType.FullName));
            }

            fsm.ChangeState(stateType);
        }

        /// <summary>
        /// 响应有限状态机事件时调用。
        /// </summary>
        /// <param name="fsm">有限状态机引用。</param>
        /// <param name="sender">事件源。</param>
        /// <param name="eventId">事件编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OnEvent(object sender, int eventId, object userData)
        {
            FsmEventHandler<T> eventHandlers = null;
            if (m_EventHandlers.TryGetValue(eventId, out eventHandlers))
            {
                if (eventHandlers != null)
                {
                    eventHandlers(fsm, sender, userData);
                }
            }
        }
    }
}