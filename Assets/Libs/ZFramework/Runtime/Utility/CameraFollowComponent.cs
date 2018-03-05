using UnityEngine;

public class CameraFollowComponent : MonoBehaviour
{ 
    /// <summary>
    /// 跟随目标
    /// </summary> 
    public Transform target;

    Vector3 m_dir = new Vector3(0, -10f, 7f);
     
    private void Start()
    { 
    }
     
    private void Update()
    {
        if (target != null)
        {
            transform.position = target.position - m_dir;
        }
    }
}