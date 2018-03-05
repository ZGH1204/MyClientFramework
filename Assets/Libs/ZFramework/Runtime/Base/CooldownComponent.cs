using System;
using ZFramework;
using ZFramework.Runtime;
using ZFramework.Timer;

public class CooldownComponent : ZFrameworkComponent
{
    private ITimerManager m_ITimerManager = null;

    /// <summary>
    /// 游戏框架组件初始化。
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        m_ITimerManager = ZFrameworkEntry.GetModule<ITimerManager>();
        if (m_ITimerManager == null)
        {
            Log.Fatal("FSM manager is invalid.");
            return;
        }
    }

    /// <summary>
    /// 增加定时器
    /// </summary>
    /// <param name="timerId"></param>
    /// <param name="cd"></param>
    /// <param name="action"></param> 
    public ICooldownTimer AddTimer(int timerId, float cd, Action action)
    {  
        return m_ITimerManager.AddTimer(timerId, cd, action);
    }

    /// <summary>
    /// 增加定时器
    /// </summary>
    /// <param name="timerId"></param>
    /// <param name="cd"></param>
    /// <param name="action"></param>
    /// <param name="param"></param>
    public ICooldownTimer AddTimer(int timerId, float cd, Action<object> action, object param)
    {
        return m_ITimerManager.AddTimer(timerId, cd, action, param);
    }

    /// <summary>
    /// 增加定时器
    /// </summary>
    /// <param name="timerId"></param>
    /// <param name="cd"></param>
    /// <param name="action"></param> 
    public ICooldownTimer AddOnceTimer(int timerId, float cd, Action action)
    {
        return m_ITimerManager.AddOnceTimer(timerId, cd, action);
    }

    /// <summary>
    /// 增加定时器
    /// </summary>
    /// <param name="timerId"></param>
    /// <param name="cd"></param>
    /// <param name="action"></param>
    /// <param name="param"></param>
    public ICooldownTimer AddOnceTimer(int timerId, float cd, Action<object> action, object param)
    {
        return m_ITimerManager.AddOnceTimer(timerId, cd, action, param);
    }

    /// <summary>
    /// 检查是否存在定时器
    /// </summary>
    public bool HasTimer(int timerId)
    {
        return m_ITimerManager.HasTimer(timerId);
    }


    /// <summary>
    /// 修改定时器
    /// </summary>
    /// <param name="timerId"></param>
    /// <param name="cd"></param>
    /// <param name="action"></param> 
    public void ModifyTimer(int timerId, float cd, Action action = null)
    {
        m_ITimerManager.ModifyTimer(timerId, cd, action);
    }

    /// <summary>
    /// 修改定时器
    /// </summary>
    /// <param name="timerId"></param>
    /// <param name="cd"></param>
    /// <param name="action"></param>
    /// <param name="param"></param>
    public void ModifyTimer(int timerId, float cd, Action<object> action, object param)
    {
        m_ITimerManager.ModifyTimer(timerId, cd, action, param);
    }

    /// <summary>
    /// 开始定时器
    /// </summary>
    /// <param name="timerId"></param>
    public void StartTimer(int timerId)
    {
        m_ITimerManager.StartTimer(timerId);
    }

    /// <summary>
    /// 暂停计时器
    /// </summary>
    /// <param name="timerId"></param>
    public void PauseTimer(int timerId)
    {
        m_ITimerManager.PauseTimer(timerId);
    }

    /// <summary>
    /// 继续定时器
    /// </summary>
    /// <param name="timerId"></param>
    public void ContinueTimer(int timerId)
    {
        m_ITimerManager.ContinueTimer(timerId);
    }

    /// <summary>
    /// 移除所有定时器
    /// </summary>
    /// <param name="timerId"></param>
    public void RemoveAllTimer()
    {
        m_ITimerManager.RemoveAllTimer();
    }

    /// <summary>
    /// 移除定时器
    /// </summary>
    /// <param name="timerId"></param>
    public void RemoveTimer(int timerId)
    {
        m_ITimerManager.RemoveTimer(timerId);
    }


}