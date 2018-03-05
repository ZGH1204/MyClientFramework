using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobaFrame.SkillAction
{ 
    public abstract class BaseAction
    {
        public Action<BaseAction> OnActionStartCallback;

        public Action<BaseAction> OnActionEndCallback;

        public Action<BaseAction> OnActionStopCallback;

        private static List<GameObject> nodePool = new List<GameObject>();
        private static GameObject skillNodePool; 
        private static GameObject childSkillNodePool;

        private static readonly Stat s_stat = new BaseAction.Stat();
        private static int skill_action_id = 1000;

        /// <summary>
        /// 行为节点id
        /// </summary>
        public int actionId;




        protected BaseAction()
        {
            this.actionId = this.assign_id();
            BaseAction.s_stat.ctorCnt++;
        }

        /// <summary>
        /// 分配对象池
        /// </summary>
        public static void AllocNodePool()
        {
            if (BaseAction.skillNodePool == null)
            {
                BaseAction.skillNodePool = new GameObject("SkillNodePool");
            }
            if (BaseAction.childSkillNodePool == null)
            {
                BaseAction.childSkillNodePool = new GameObject("ChildSkillNodePool");
            }
            for (int i = 0; i < BaseAction.nodePool.Count; i++)
            {
                if (BaseAction.nodePool[i] != null)
                {
                    UnityEngine.Object.Destroy(BaseAction.nodePool[i]);
                }
            }
            BaseAction.nodePool.Clear();
            for (int j = 0; j < 1; j++)
            {
                BaseAction.ReleaseNode(BaseAction.AllocNode(NodeType.None, true));
            }
        }

        private static GameObject AllocNode(NodeType type, bool forceNew = false)
        {
            string text = null;
            if (BaseAction.nodePool.Count != 0 && !forceNew)
            {
                GameObject gameObject = BaseAction.nodePool[0];
                BaseAction.nodePool.RemoveAt(0);
                gameObject.SetActive(true);
                gameObject.transform.position = Vector3.zero;
                return gameObject;
            }
            switch (type)
            {
                case NodeType.None:
                    text = "Prefab/Skill/SkillNode";
                    break;
                case NodeType.SkillNode:
                    text = "Prefab/Skill/SkillNode";
                    break;
                case NodeType.EffectNode:
                    text = "Prefab/Skill/EffectNode";
                    break;
                case NodeType.DamageNode:
                    text = "Prefab/Skill/DamageNode";
                    break;
            }
            if (text != null)
            {
                UnityEngine.Object original = Resources.Load(text);
                GameObject gameObject2 = UnityEngine.Object.Instantiate(original) as GameObject;
                gameObject2.tag = "ActionNode";
                return gameObject2;
            }
            return null;
        }

        private static void ReleaseNode(GameObject go)
        {
            UnityEngine.Object.Destroy(go);
        }

        private int assign_id()
        {
            BaseAction.skill_action_id++;
            return BaseAction.skill_action_id;
        }












        private class Stat
        {
            public int ctorCnt;

            public int createNodeCnt;

            public int destroyCnt;

            public int destroyNodeCnt;

            public override string ToString()
            {
                return string.Format("{0}/{1} {2}/{3}", new object[]
                {
                    this.ctorCnt,
                    this.destroyCnt,
                    this.createNodeCnt,
                    this.destroyNodeCnt
                });
            }
        }

    }
}
