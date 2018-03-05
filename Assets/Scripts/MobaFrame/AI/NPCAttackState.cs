using GGame;
using UnityEngine;
using ZFramework;
using ZFramework.Fsm;

namespace MobaFrame.AI
{
    public class NPCAttackState : FsmState<NPCComponent>
    {
        public override void OnInit(IFsm<NPCComponent> fsm)
        {
            base.OnInit(fsm); 
        }

        public override void OnEnter()
        {
            base.OnEnter(); 

        }

        public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds); 

            float dis = Vector3.Distance(this.fsm.Owner.attackHero.position, this.fsm.Owner.transform.position);
            if (dis > 1.5f)
            {
                MoveToTarget();
            }
            else
            {
                //播放动画
                this.fsm.Owner.PlayAttack();
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
         
        void MoveToTarget()
        {
            Quaternion lookAtRot = Quaternion.LookRotation(this.fsm.Owner.attackHero.position - this.fsm.Owner.transform.position);
            this.fsm.Owner.transform.localEulerAngles = lookAtRot.eulerAngles;

            this.fsm.Owner.npcCharacterController.Move(Vector3.Normalize(this.fsm.Owner.attackHero.position - this.fsm.Owner.transform.position) / 40f);
            //播放动画
            this.fsm.Owner.PlayMove();
        }
    }
}