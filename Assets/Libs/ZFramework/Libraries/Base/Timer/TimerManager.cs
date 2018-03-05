
using System;
using System.Collections.Generic;

namespace ZFramework.Timer
{
    internal sealed class TimerManager : ZFrameworkModule, ITimerManager
    {
        private Dictionary<int, ICooldownTimer> m_CooldownTimerDic;

        /// <summary>
        /// 初始化有限状态机管理器的新实例。
        /// </summary>
        public TimerManager()
        {
            m_CooldownTimerDic = new Dictionary<int, ICooldownTimer>();
        }

        /// <summary>
        /// 获取游戏框架模块优先级。
        /// </summary>
        /// <remarks>优先级较高的模块会优先轮询，并且关闭操作会后进行。</remarks>
        internal override int Priority
        {
            get
            {
                return 100;
            }
        }
         
        /// <summary>
        /// 有限状态机管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            List<int> list = new List<int>(m_CooldownTimerDic.Keys);
             
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    m_CooldownTimerDic[item].Update(elapseSeconds, realElapseSeconds);
                }
            }
        }

        /// <summary>
        /// 关闭并清理有限状态机管理器。
        /// </summary>
        internal override void Shutdown()
        {
            RemoveAllTimer();
        }


        public ICooldownTimer AddTimer(int timerId, float cd, Action action)
        {
            ICooldownTimer cooldownTimer = new CooldownTimer(timerId, cd);
            cooldownTimer.Tick = action; 

            if (!m_CooldownTimerDic.ContainsKey(timerId))
            {
                m_CooldownTimerDic.Add(timerId, cooldownTimer);
            }

            return cooldownTimer;
        }

        public ICooldownTimer AddTimer(int timerId, float cd, Action<object> action, object param)
        {
            ICooldownTimer cooldownTimer = new CooldownTimer(timerId, cd);
            cooldownTimer.Tick1 = action;
            cooldownTimer.param = param; 

            if (!m_CooldownTimerDic.ContainsKey(timerId))
            {
                m_CooldownTimerDic.Add(timerId, cooldownTimer);
            }

            return cooldownTimer;
        }

        public ICooldownTimer AddOnceTimer(int timerId, float cd, Action action)
        {
            ICooldownTimer cooldownTimer = new CooldownTimer(timerId, cd);
            cooldownTimer.Tick = action;
            cooldownTimer.Tick += () => {
                RemoveTimer(timerId);
            };

            if (!m_CooldownTimerDic.ContainsKey(timerId))
            {
                m_CooldownTimerDic.Add(timerId, cooldownTimer);
            }

            return cooldownTimer;
        }

        public ICooldownTimer AddOnceTimer(int timerId, float cd, Action<object> action, object param)
        {
            ICooldownTimer cooldownTimer = new CooldownTimer(timerId, cd);
            cooldownTimer.Tick1 += action;
            cooldownTimer.Tick1 += (arg) => {
                RemoveTimer(timerId);
            };
            cooldownTimer.param = param;

            if (!m_CooldownTimerDic.ContainsKey(timerId))
            {
                m_CooldownTimerDic.Add(timerId, cooldownTimer);
            }
             
            return cooldownTimer;
        }

        /// <summary>
        /// 检查是否存在定时器
        /// </summary>
        public bool HasTimer(int timerId)
        {
            if (m_CooldownTimerDic.ContainsKey(timerId))
            {
                return true; 
            }

            return false;
        }

        public void ModifyTimer(int timerId, float cd, Action action = null)
        {
            if (!m_CooldownTimerDic.ContainsKey(timerId))
            {
                Log.Error("要修改的定时器不存在！");
            }

            ICooldownTimer cooldownTimer = m_CooldownTimerDic[timerId];
            cooldownTimer.active = false;
            cooldownTimer.cooldown = cd;
            if (action != null)
            {
                cooldownTimer.Tick = action;
            }
        }

        public void ModifyTimer(int timerId, float cd, Action<object> action, object param)
        {
            if (!m_CooldownTimerDic.ContainsKey(timerId))
            {
                Log.Error("要修改的定时器不存在！");
            }

            ICooldownTimer cooldownTimer = m_CooldownTimerDic[timerId];
            cooldownTimer.active = false;
            cooldownTimer.cooldown = cd;
            cooldownTimer.param = param;

            if (action != null)
            {
                cooldownTimer.Tick1 = action;
            }
        }

        /// <summary>
        /// 开始定时器
        /// </summary>
        /// <param name="timerId"></param>
        public void StartTimer(int timerId)
        {
            if (!m_CooldownTimerDic.ContainsKey(timerId))
            {
                Log.Error("要开始的定时器不存在！");
            }

            m_CooldownTimerDic[timerId].Start();
        }

        /// <summary>
        /// 暂停计时器
        /// </summary>
        /// <param name="timerId"></param>
        public void PauseTimer(int timerId)
        {
            if (!m_CooldownTimerDic.ContainsKey(timerId))
            {
                Log.Error("要暂停的定时器不存在！");
            }

            m_CooldownTimerDic[timerId].Pause();
        }

        /// <summary>
        /// 继续定时器
        /// </summary>
        /// <param name="timerId"></param>
        public void ContinueTimer(int timerId)
        {
            if (!m_CooldownTimerDic.ContainsKey(timerId))
            {
                Log.Error("要继续的定时器不存在！");
            }

            m_CooldownTimerDic[timerId].Continue();
        }

        public void RemoveAllTimer()
        {
            m_CooldownTimerDic.Clear();
        }

        public void RemoveTimer(int timerId)
        {
            if (!m_CooldownTimerDic.ContainsKey(timerId))
            {
                Log.Error("要移除的定时器不存在！");
            }

            m_CooldownTimerDic.Remove(timerId);
        }
    }

}
