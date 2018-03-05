using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaFrame.SkillAction
{
    public enum NodeType
    {
        /// <summary>
        /// null节点
        /// </summary>
        None,

        /// <summary>
        /// 技能节点
        /// </summary>
		SkillNode,

        /// <summary>
        /// 特效节点
        /// </summary>
		EffectNode,

        /// <summary>
        /// 伤害节点
        /// </summary>
		DamageNode
    }
}
