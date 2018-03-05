using System;

namespace ZFramework.Timer
{
    public interface ICooldownTimer
    {
        Action<object> Tick1 { get; set; }

        Action Tick { get; set; }

        /// <summary>
        /// 计时器id
        /// </summary>
        int timerId { get; }

        /// <summary>
        /// 定时器周期
        /// </summary>
        float cooldown { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        object param { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        bool active { get; set; }

        /// <summary>
        /// 是否是一次
        /// </summary>
        bool isOnce { get; set; }

        /// <summary>
        /// 游戏框架模块轮询。
        /// </summary> 
        void Update(float elapseSeconds, float realElapseSeconds);

        /// <summary>
        /// 暂停并记录已等待时间
        /// </summary>
        void Pause();

        /// <summary>
        /// 继续
        /// </summary>
        void Continue();

        /// <summary>
        /// 从头开启
        /// </summary>
        void Start();
         
  
    }
}