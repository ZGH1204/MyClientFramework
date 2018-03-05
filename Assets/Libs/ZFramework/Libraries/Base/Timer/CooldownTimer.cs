using System;

namespace ZFramework.Timer
{
    internal class CooldownTimer : ICooldownTimer
    {
        private Action<object> m_Tick1;
        private Action m_Tick;
        private int m_TimerId;
        private float m_Cooldown;
        private object m_Param;
        private bool m_active;
        private bool m_isOnce;

        private float m_timeSum;

        public CooldownTimer(int id, float second)
        {
            m_TimerId = id;
            m_Cooldown = second;
            m_active = false;
            m_isOnce = false;

            m_timeSum = 0;
        }

        /// <summary>
        /// 定时器响应事件
        /// </summary>
        public Action<object> Tick1
        {
            get { return m_Tick1; }
            set { m_Tick1 = value; }
        }

        /// <summary>
        /// 定时器响应事件
        /// </summary>
        public Action Tick
        {
            get { return m_Tick; }
            set { m_Tick = value; }
        }

        /// <summary>
        /// 计时器id
        /// </summary>
        public int timerId
        {
            get
            {
                return m_TimerId;
            }
        }

        /// <summary>
        /// 计时器周期
        /// </summary>
        public float cooldown
        {
            get { return m_Cooldown; }
            set { m_Cooldown = value; }
        }

        /// <summary>
        /// 参数
        /// </summary>
        public object param
        {
            get { return m_Param; }
            set { m_Param = value; }
        }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool active
        {
            get { return m_active; }
            set { m_active = value; }
        }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool isOnce
        {
            get { return m_isOnce; }
            set { m_isOnce = value; }
        }

        /// <summary>
        ///  游戏框架模块轮询。
        /// </summary>
        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (!active)
            {
                return;
            }

            m_timeSum += elapseSeconds;
            if (m_timeSum > m_Cooldown)
            {
                m_timeSum = 0;
                if (m_Tick != null)
                {
                    m_Tick.Invoke();
                }
                if (m_Tick1 != null)
                {
                    m_Tick1.Invoke(param);
                }

                if (isOnce)
                {

                }
            }
        }

        /// <summary>
        /// 从头开启
        /// </summary>
        public void Start()
        {
            m_timeSum = 0;
            m_active = true;
        }

        /// <summary>
        /// 暂停并记录已等待时间
        /// </summary>
        public void Pause()
        {
            m_active = false;
        }

        /// <summary>
        /// 继续
        /// </summary>
        public void Continue()
        {
            m_active = true;
        }
 
    }
}