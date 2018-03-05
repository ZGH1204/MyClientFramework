using GGame.NetWork;
using GGame.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFramework.Network;

public class PlayerComponent : MonoBehaviour {
       
    public HPComponent hPComponent;

    public UnitData playerUnitData;
    
    Animator heroAnimator;
    CharacterController heroCharacterController;
    SphereColliderComponent sphereColliderComponent;
    
    NPCComponent attackNPC;
    PlayerComponent attackPlayer;
    bool m_Attacking;
    
    void Start()
    {
        heroAnimator = this.GetComponent<Animator>();
        heroCharacterController = this.GetComponent<CharacterController>(); 
        sphereColliderComponent = transform.Find("SphereCollider").GetComponent<SphereColliderComponent>();

        sphereColliderComponent.OnTriggerEnterEvent += (collider) => {
            if (collider.transform.tag == "NPC")
            {
                attackNPC = collider.transform.GetComponent<NPCComponent>();
            }
            if (collider.transform.tag == "Player" && collider.gameObject.name != playerUnitData.PlayerId.ToString())
            {
                attackPlayer = collider.transform.GetComponent<PlayerComponent>();
            }
        };
        sphereColliderComponent.OnTriggerExitEvent += (collider) => {
            if (collider.transform.tag == "NPC")
            {
                attackNPC = null;
            }
            if (collider.transform.tag == "Player" && collider.gameObject.name != playerUnitData.PlayerId.ToString())
            {
                attackPlayer = null;
            }
        };
         

        SyncPlayerData.Instance.syncPositionBack += SyncPositionBack;
    }

    /// <summary>
    /// 绑定血条
    /// </summary>
    /// <param name="hp"></param>
    public void SetHPComponent(HPComponent hp)
    {
        hPComponent = hp;
        hPComponent.SetEnable(true);
        hPComponent.DeadEvent += () => {

            if (playerUnitData.PlayerId == PlayerManager.Instance.GetHeroPlayer().playerUnitData.PlayerId)
            {
                Destroy();
          
            }

            this.gameObject.SetActive(false);
            hPComponent.SetEnable(false);
        };
    }

    #region 控制行为
     
    /// <summary>
    /// 同步位置
    /// </summary>
    /// <param name="pos"></param>
    void SyncPositionBack(SCPositon pos)
    {
        if (playerUnitData.PlayerId == pos.PlayerId)
        {
            if (pos.Animation == 2 && !m_Attacking)
            {
                //旋转
                heroCharacterController.transform.eulerAngles = new Vector3(0, 90f - Mathf.Atan2(pos.Z, pos.X) * 180f / Mathf.PI, 0);
                //移动
                heroCharacterController.Move(new Vector3(pos.X, pos.Y, pos.Z) + new Vector3(0, -100f, 0));
                //动画
                PlayMove();
            }
            else if (pos.Animation == 1 && !m_Attacking)
            {
                //动画
                PlayIdle();
            }
            else if (pos.Animation == 3)
            {
                //动画
                PlayAttack(); 
                if (attackNPC != null)
                {
                    if (attackNPC != null)
                    {
                        Quaternion lookAtRot = Quaternion.LookRotation(attackNPC.transform.position - transform.position);
                        transform.localEulerAngles = lookAtRot.eulerAngles;
                    }
                    else if (attackPlayer != null)
                    {
                        Quaternion lookAtRot = Quaternion.LookRotation(attackPlayer.transform.position - transform.position);
                        transform.localEulerAngles = lookAtRot.eulerAngles;
                    }
                     
                }
            }
        }
    }


    #endregion
     
    private void Update()
    {
       
    }
 
    void Destroy()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, ss) => {

            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Battle");
        };

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Battle");
         
    }

    #region 控制动画

    /// <summary>
    /// 设置动画速度
    /// </summary>
    /// <param name="speed"></param>
    public void SetAnimatorSpeed(float speed)
    {
        heroAnimator.speed = speed;
    }

    /// <summary>
    /// 播放attack动画
    /// </summary>
    public void PlayAttack()
    {
        if (m_Attacking)
        {
            return;
        }

        m_Attacking = true;
        heroAnimator.SetBool("attack", true); 
        GGame.GameEntry.Cooldown.AddOnceTimer("PlayAttack".GetHashCode(), 0.1f, () =>{
            heroAnimator.SetBool("attack", false);
        }).Start();
    }

    /// <summary>
    /// 播放idle动画
    /// </summary>
    public void PlayIdle()
    {
        heroAnimator.SetBool("attack", false);
        heroAnimator.SetFloat("speed", 0f); 
    }

    /// <summary>
    /// 播放run动画
    /// </summary>
    public void PlayMove()
    { 
        heroAnimator.SetFloat("speed", 10f);
    }

    /// <summary>
    /// 播放death动画
    /// </summary>
    public void PlayDeath()
    {
        heroAnimator.SetBool("death", true); 
    }

    #endregion

    #region 动画帧动画事件

    void PlayEffect(int id)
    { 
        var trans = this.transform.Find("Root/Spine1/Spine2/Spine3/L_Clavicle/L_Shoulder/L_Elbow/L_Hand/Sword");
        EffectManager.Instance.PlayEffect(this, id, trans.position, trans.eulerAngles);
    }


    void AttackAniEnd()
    {
        m_Attacking = false; 
    }

    void HurtAniEvent()
    {
        if (attackNPC != null)
        {
            //npc.hPComponent.AddAttackHp(value);
            BattleManager.Instance.Attack(this, attackNPC);

        }
        if (attackPlayer != null)
        {
            BattleManager.Instance.Attack(this, attackPlayer);
        }
    }
     
    void DeathAniEnd()
    {
        
    }

    #endregion 动画帧动画事件

}
