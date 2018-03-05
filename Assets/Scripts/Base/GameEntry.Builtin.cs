using UnityEngine;
using ZFramework.Runtime;

namespace GGame
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        /// <summary>
        /// 获取游戏基础组件。
        /// </summary>
        public static BaseComponent Base
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取事件组件。
        /// </summary>
        public static EventComponent Event
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取网络组件。
        /// </summary>
        public static NetworkComponent Network
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取有限状态机组件。
        /// </summary>
        public static FsmComponent Fsm
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取配置组件。
        /// </summary>
        public static SettingComponent Setting
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取UI组件。
        /// </summary>
        public static UIComponent UI
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取配置表组件。
        /// </summary>
        public static DateTableComponent DateTable
        {
            get;
            private set;
        }
         
        /// <summary>
        /// 获取资源组件。
        /// </summary>
        public static ResourceComponent Resource
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取配置表组件。
        /// </summary>
        public static CooldownComponent Cooldown
        {
            get;
            private set;
        }

        private void InitBuiltinComponents()
        {
            Base = ZFramework.Runtime.GameEntry.GetComponent<BaseComponent>();
            Event = ZFramework.Runtime.GameEntry.GetComponent<EventComponent>();
            Network = ZFramework.Runtime.GameEntry.GetComponent<NetworkComponent>();
            Fsm = ZFramework.Runtime.GameEntry.GetComponent<FsmComponent>();
            Setting = ZFramework.Runtime.GameEntry.GetComponent<SettingComponent>();
            UI = ZFramework.Runtime.GameEntry.GetComponent<UIComponent>();
            DateTable = ZFramework.Runtime.GameEntry.GetComponent<DateTableComponent>();
            Resource = ZFramework.Runtime.GameEntry.GetComponent<ResourceComponent>();
            Cooldown = ZFramework.Runtime.GameEntry.GetComponent<CooldownComponent>();
        }
    }
}