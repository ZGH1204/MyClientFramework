using UnityEngine;

public class CameraSlideComponent : MonoBehaviour
{
    public bool enable = false;

    // Use this for initialization
    private void Start()
    {
         
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    /// <summary>
    /// 滑动屏幕
    /// </summary>
    public void Slide(Vector2 deltaPosition)
    {
        if (enable)
        {
            transform.position += new Vector3(deltaPosition.x / 10f, 0, deltaPosition.y / 10f);
        }
    }


}