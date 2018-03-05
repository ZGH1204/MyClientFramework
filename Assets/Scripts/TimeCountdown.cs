using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCountdown : MonoBehaviour {
     
    int m_Cooldown;
    Text txt;

    void Start () {

        m_Cooldown = 600;
        txt = this.GetComponent<Text>();

        GGame.GameEntry.Cooldown.AddTimer("战斗倒计时".GetHashCode(), 1, () => { 
            txt.text = string.Format("剩余时间    {0:D2}:{1:D2}", m_Cooldown / 60, m_Cooldown % 60);
            m_Cooldown--;
            if (m_Cooldown < 0)
            {
                GGame.GameEntry.Cooldown.RemoveTimer("战斗倒计时".GetHashCode());
            }
        }).Start();

       
    }
	 
}
