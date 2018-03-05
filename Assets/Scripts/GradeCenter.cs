using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradeCenter : MonoBehaviour {

    public GameObject ok;
     
    /// <summary>
    /// 当进入触发器
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter(Collider collider)
    {
        ok.SetActive(true);
    }

    /// <summary>
    /// 当退出触发器
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerExit(Collider collider)
    {
        ok.SetActive(false);
    }

}
