using UnityEngine;

public class GyroManager : MonoBehaviour
{
    // Use this for initialization

    private void Start()
    {
        //设置设备陀螺仪的开启/关闭状态，使用陀螺仪功能必须设置为 true
        Input.gyro.enabled = true;
        //获取设备重力加速度向量
        Vector3 deviceGravity = Input.gyro.gravity;
        //设备的旋转速度，返回结果为x，y，z轴的旋转速度，单位为（弧度/秒）
        Vector3 rotationVelocity = Input.gyro.rotationRate;
        //获取更加精确的旋转
        Vector3 rotationVelocity2 = Input.gyro.rotationRateUnbiased;
        //设置陀螺仪的更新检索时间，即隔 0.1秒更新一次
        Input.gyro.updateInterval = 0.1f;
        //获取移除重力加速度后设备的加速度
        Vector3 acceleration = Input.gyro.userAcceleration;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}