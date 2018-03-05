using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimator : MonoBehaviour {

    public HeroControl heroControl;

    Animator animator;
    bool startAttack = false;
    // Use this for initialization
    void Start () {

        animator = this.GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 当进入触发器
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter(Collider collider)
    {
        animator.SetBool("Attack", true);
        startAttack = true;
        StartCoroutine(StartAttack());
    }

    /// <summary>
    /// 当退出触发器
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerExit(Collider collider)
    {
        startAttack = false;
        StopCoroutine(StartAttack());
        animator.SetBool("Attack", false);
    }
  
    IEnumerator StartAttack()
    {
        while (startAttack)
        {
            animator.SetBool("Attack", true);
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("Attack", false);
            heroControl.AttackToHp();
            yield return new WaitForSeconds(2f);
        }
    }



}
