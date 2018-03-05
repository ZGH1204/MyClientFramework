using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillComponent : MonoBehaviour {

    public float CD;
    public Image level;
   

    public Action clickEvent;

    float time1 = 0;
     

    public bool enableClick
    {
        get;
        set;
    }
 
    public void StartCD()
    {
        enableClick = false;
        time1 = 0;

        GGame.GameEntry.Cooldown.AddOnceTimer(this.GetHashCode(), CD, () => {
            enableClick = true; 
        }).Start();
    }

    public void Click()
    {
        if (enableClick)
        {
            if (clickEvent != null)
            {
                clickEvent.Invoke();
            }
            
            StartCD();
        }
    }
     
	void Update () {

        if (!enableClick)
        {
           
            time1 += Time.deltaTime;

            time1 = time1 > CD ? CD : time1;
            level.fillAmount = time1 / CD;

        }
	}
}
