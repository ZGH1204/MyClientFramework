using GGame;
using System;
using UnityEngine;
using ZFramework;
using ZFramework.Fsm;

namespace MobaFrame.AI
{
    public class NPCDefendState : FsmState<NPCComponent>
    {
        float m_Radius;
        Vector3 birthPos;
        Vector3 eulerAngles;
        bool backBirthPos = false; 
        Vector3 m_MoveToTarget;

        public override void OnInit(IFsm<NPCComponent> fsm)
        {
            base.OnInit(fsm); 

            m_Radius = this.fsm.Owner.defendCollider.colliderRadius;
            birthPos = this.fsm.Owner.transform.position;
            eulerAngles = this.fsm.Owner.transform.eulerAngles;
        }

        public override void OnEnter()
        {
            base.OnEnter(); 

            //ToPosBack(); 
            backBirthPos = true;
          
        }

        public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds); 

            if (backBirthPos)
            {
                RunToTarget(birthPos, () => {
                    this.fsm.Owner.transform.eulerAngles = eulerAngles;
                    backBirthPos = false; 
                });
            }
 
        }

        public override void OnLeave(bool isShutdown)
        {
            base.OnLeave(isShutdown); 
             
            //播放动画
            this.fsm.Owner.PlayIdle();
        }

        public override void OnDestroy()
        {
            base.OnDestroy(); 
             
            //播放动画
            this.fsm.Owner.PlayIdle();
        }

       
        void ToPosBack()
        {
            m_MoveToTarget = GetRandomPos();
        }

        Vector3 GetRandomPos()
        {
            float x = UnityEngine.Random.Range(m_Radius * -1f, m_Radius);
            float z = UnityEngine.Random.Range(m_Radius * -1f, m_Radius);
            Vector3 pos = birthPos + new Vector3(x, 0, z);

            return pos;
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="target"></param>
        void RunToTarget(Vector3 pos, Action back)
        {
            //重新选择目标点
            if (Vector3.Distance(pos, this.fsm.Owner.transform.position) < 1.0f)
            {
                if (back != null)
                {
                    back.Invoke();
                    //播放动画
                    this.fsm.Owner.PlayIdle();
                    return;
                }
            }

            //转向
            Quaternion lookAtRot = Quaternion.LookRotation(pos - this.fsm.Owner.transform.position);
            this.fsm.Owner.transform.localEulerAngles = lookAtRot.eulerAngles;
            //移动
            this.fsm.Owner.npcCharacterController.Move(Vector3.Normalize(pos - this.fsm.Owner.transform.position) / 40f);
            //播放动画
            this.fsm.Owner.PlayMove();
        }
    }
}