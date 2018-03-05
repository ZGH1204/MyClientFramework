using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFramework;

public class EffectManager : MonoBehaviour {

    public static EffectManager Instance;

    /// <summary>
    /// 所有的特效
    /// </summary>
    public List<Transform> effectList;


    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 播放特效
    /// </summary>
    /// <param name="effectId"></param>
    /// <param name="position"></param>
    /// <param name="eulerAngles"></param>
    public void PlayEffect(PlayerComponent attacker, int effectId, Vector3 position, Vector3 eulerAngles)
    {
        if (effectList.Count <= effectId)
        {
            return;
        }

        var effect = Instantiate(effectList[effectId]);
         
        effect.transform.position = position + Vector3.Normalize(attacker.transform.forward) * 2;
        effect.transform.eulerAngles = attacker.transform.eulerAngles;

        Log.Info(effect.transform.eulerAngles.ToString());
    }
 
}
