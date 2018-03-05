using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlow : MonoBehaviour {

    public ETCJoystick rotationJoystick;
    public Transform target;
      
    public float yawSensitivity = 30f;
    public float pinchSensitivity = 5.0f; 
    public float distance = 2f;

    float yaw = 0;
    float pitch = 10; 
    Vector3 gyro;
     
    void Start()
    {  
        GyroInit();
        yaw = transform.eulerAngles.y;
        UpdateCamera();

         
    }

    private void LateUpdate()
    {
        UpdateCamera();
    }

    void UpdateCamera()
    {
        //gyro += new Vector3(Input.gyro.rotationRate.x / 2.5f, Input.gyro.rotationRate.y / -2.5f, 0);

        yaw -= Input.gyro.rotationRate.y * yawSensitivity / 60f;
        pitch -= Input.gyro.rotationRate.x * pinchSensitivity / 30f;
        Quaternion rotaetAngle = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPos = target.localPosition - rotaetAngle * new Vector3(0, 0, distance) + target.up * 0.5f;
        float xAngle = rotaetAngle.eulerAngles.x % 360;
        xAngle = xAngle > 180f ? xAngle - 360f : (xAngle < -180f ? xAngle + 360f : xAngle);
        xAngle = xAngle > 45f ? 45f : (xAngle < -60f ? -60f : xAngle);
        
        transform.localPosition = desiredPos + transform.right;
        transform.localEulerAngles = new Vector3(xAngle, rotaetAngle.eulerAngles.y, rotaetAngle.eulerAngles.z); 
    }

    /// <summary>
    /// 设置陀螺仪
    /// </summary>
    void GyroInit()
    {
        gyro = new Vector3(0, 0, 0);
        //设置设备陀螺仪的开启/关闭状态，使用陀螺仪功能必须设置为 true  
        Input.gyro.enabled = true; 
        //设置陀螺仪的更新检索时间，即隔 0.1秒更新一次  
        Input.gyro.updateInterval = 0.1f; 
    }
}
