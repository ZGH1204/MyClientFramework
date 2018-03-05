using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HeroControl : MonoBehaviour
{
    public NPCControl npcControl;
    public CharacterController characterController;
    public Animator animator;
    public CameraSlideComponent cameraSlideComponent;
    public CameraFollowComponent cameraFollowComponent;

    public Transform hp;
    public Slider slider;
    public BloodImage bloodImage;


    private Vector3 onMoveMotion;

    Transform npcTrans;

    public float maxHp = 1000;
    float attackedValue = 0;


    private void Start()
    {
        this.Move(new Vector2(1, 0));
        this.MoveEnd();


    }

    private void Update()
    {
        characterController.Move(onMoveMotion);

        hp.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0, 130f, 0);

        float gezi = int.Parse((maxHp / 200).ToString());
        //bloodImage.uvRect = new Rect(new Vector2(0, 0), new Vector2(gezi, 1));

        slider.maxValue = gezi;
        slider.value = gezi - attackedValue;

    }

    /// <summary>
    /// 移动控制
    /// </summary>
    /// <param name="deltaPosition"></param>
    public void Move(Vector2 deltaPosition)
    { 
        //旋转
        characterController.transform.eulerAngles = new Vector3(0, 90f - Mathf.Atan2(deltaPosition.y, deltaPosition.x) * 180f / Mathf.PI, 0);
        //移动
        onMoveMotion = new Vector3(deltaPosition.x, -10f, deltaPosition.y) * 3f * Time.deltaTime;
        //动画
        animator.SetFloat("Speed", 0.2f);
    }

    /// <summary>
    /// 移动停止
    /// </summary>
    public void MoveEnd()
    {
        onMoveMotion = Vector3.zero;
        animator.SetFloat("Speed", 0f);  
    }

    /// <summary>
    /// 释放技能
    /// </summary>
    public void FireSkill(int skillId)
    {
    }

    /// <summary>
    /// 普通攻击
    /// </summary>
    public void Attack()
    {
        if (animator.GetBool("Attack"))
        {
            return;
        }

        if (npcTrans != null)
        {
            Vector3 forwardDir = npcTrans.position - transform.position;
            Quaternion lookAtRot = Quaternion.LookRotation(forwardDir);
            transform.localEulerAngles = lookAtRot.eulerAngles;
        }

        animator.SetBool("Attack", true);
        StartCoroutine(StopAttack(1f));
    }

    private IEnumerator StopAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        npcControl.AttackToHp();
        animator.SetBool("Attack", false);
    }

    /// <summary>
    /// 当进入触发器
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter(Collider collider)
    {
        npcTrans = collider.transform;

    }

    /// <summary>
    /// 当退出触发器
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerExit(Collider collider)
    {
        npcTrans = null;

    }


    public void AttackToHp()
    {
        attackedValue += 0.9f;
    }
}