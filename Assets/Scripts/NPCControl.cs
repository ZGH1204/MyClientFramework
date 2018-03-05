using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCControl : MonoBehaviour {


    Transform heroTrans;
     
    public Transform hp;
    public Slider slider;
    public BloodImage bloodImage;

    float maxHp = 1000;
    float curHp = 5;


    /// <summary>
    /// 当进入触发器
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter(Collider collider)
    {
        heroTrans = collider.transform; 
    }

    /// <summary>
    /// 当退出触发器
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerExit(Collider collider)
    {
        heroTrans = null;
    }

    private void Update()
    {
        if (heroTrans != null)
        {
            FaceToHero(heroTrans);
        }

        hp.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0, 130f, 0);

        float gezi = maxHp / 200f;
        //bloodImage.uvRect = new Rect(new Vector2(0, 0), new Vector2(gezi, 1));

        slider.maxValue = gezi;
        
        slider.value = curHp;
    }

    void FaceToHero(Transform hero)
    {
        Vector3 forwardDir = hero.position - transform.position;
        Quaternion lookAtRot = Quaternion.LookRotation(forwardDir);
        transform.localEulerAngles = lookAtRot.eulerAngles;


    }

    public void AttackToHp()
    {
        curHp -= 0.8f;
        if (curHp <= 0)
        {
            transform.gameObject.SetActive(false);
            hp.gameObject.SetActive(false);
        }
       
    }


}
