using GGame;
using MobaFrame.AI;
using UnityEngine;
using ZFramework.Fsm;

public class NPCComponent : MonoBehaviour
{
    /// <summary>
    /// 怪物id
    /// </summary>
    public int nPCId;

    /// <summary>
    /// 防卫圈
    /// </summary>
    public SphereColliderComponent defendCollider;

    /// <summary>
    /// 血条
    /// </summary>
    public HPComponent hPComponent;

    /// <summary>
    /// 角色控制器
    /// </summary>
    [HideInInspector]
    public CharacterController npcCharacterController;

    /// <summary>
    /// 要攻击的角色
    /// </summary>
    [HideInInspector]
    public Transform attackHero;
      
    /// <summary>
    /// 出生点
    /// </summary>
    private Vector3 birthPos;

    private Vector3 originalPos;

    private Animator npcAnimator;

    /// <summary>
    /// 怪物有限状态机
    /// </summary>
    private IFsm<NPCComponent> fsm;

    private void Start()
    {
        birthPos = this.transform.position;
        npcAnimator = this.GetComponent<Animator>();
        npcCharacterController = this.GetComponent<CharacterController>();

        originalPos = transform.position;

        defendCollider.OnTriggerEnterEvent += (collider) =>
        {
            if (collider.transform.tag == "Player")
            {
                attackHero = collider.transform;
                fsm.ChangeState<NPCAttackState>();
            }
        };

        defendCollider.OnTriggerExitEvent += (collider) =>
        {
            if (collider.transform.tag == "Player")
            {
                attackHero = null;
                fsm.ChangeState<NPCDefendState>();
            }
        };

        hPComponent = NPCManager.Instance.AddNPCHPUI(this);
        hPComponent.SetEnable(true);
        hPComponent.DeadEvent += () =>
        {
            PlayDeath();
        };

        fsm = GameEntry.Fsm.CreateFsm(this.GetHashCode().ToString(), this, new NPCDefendState(), new NPCAttackState());
        fsm.Start<NPCDefendState>();
    }

    void CreateHP()
    {

    }

    private void Update()
    {
    }

    /// <summary>
    /// 播放attack动画
    /// </summary>
    public void PlayAttack()
    {
        if (npcAnimator == null)
        {
            return;
        }
        npcAnimator.SetBool("attack", true);
        npcAnimator.SetBool("run", false);
    }

    /// <summary>
    /// 播放idle动画
    /// </summary>
    public void PlayIdle()
    {
        if (npcAnimator == null)
        {
            return;
        }
        npcAnimator.SetBool("attack", false);
        npcAnimator.SetBool("run", false);
    }

    /// <summary>
    /// 播放run动画
    /// </summary>
    public void PlayMove()
    {
        if (npcAnimator == null)
        {
            return;
        }
        npcAnimator.SetBool("run", true);
        npcAnimator.SetBool("attack", false);
    }

    /// <summary>
    /// 播放death动画
    /// </summary>
    public void PlayDeath()
    {
        if (npcAnimator == null)
        {
            return;
        }
        npcAnimator.SetBool("death", true);
    }

    #region 动画帧动画事件

    private void HurtAniEvent(int value)
    {
        if (attackHero != null)
        {
            //attackHero.GetComponent<PlayerComponent>().hPComponent.AddAttackHp(value);
            BattleManager.Instance.Attack(this, attackHero.GetComponent<PlayerComponent>());
        }
    }

    private void DeadEvent()
    {
        if (npcAnimator == null)
        {
            return;
        }
        npcAnimator.SetBool("death", false);
        NPCManager.Instance.DeadNPC(this);
    }

    #endregion 动画帧动画事件

    public void SetEnable(bool boo)
    {
        hPComponent.SetEnable(boo);
        gameObject.SetActive(boo);

    }
}