using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SphereColliderComponent : MonoBehaviour
{
    /// <summary>
    /// Collider 半径
    /// </summary>
    public float colliderRadius
    {
        get;
        set;
    }

    /// <summary>
    /// 当进入触发器
    /// </summary>
    public Action<Collider> OnTriggerEnterEvent;

    /// <summary>
    /// 当退出触发器
    /// </summary>
    public Action<Collider> OnTriggerExitEvent;

    private void Awake()
    {
        colliderRadius = this.GetComponent<SphereCollider>().radius;
    }

    /// <summary>
    /// 当进入触发器
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter(Collider collider)
    {
        if (OnTriggerEnterEvent != null)
        {
            OnTriggerEnterEvent.Invoke(collider);
        }
    }

    /// <summary>
    /// 当退出触发器
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerExit(Collider collider)
    {
        if (OnTriggerExitEvent != null)
        {
            OnTriggerExitEvent.Invoke(collider);
        }
    }
}