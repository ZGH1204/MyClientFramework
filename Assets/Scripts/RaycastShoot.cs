using UnityEngine;

public class RaycastShoot : MonoBehaviour
{
    // 设置枪击带来的伤害值
    public int gunDamage = 1;

    // 设置玩家可以射击的Unity单位
    public float weaponRange = 50f;

    // 设置枪击为物体带来的冲击力
    public float hitForce = 100f;

    // GunEnd游戏对象
    public Transform gunEnd;

    // FPS相机
    public Camera fpsCam;

    // 设置射击轨迹显示的时间
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);

    // 枪击音效
    private AudioSource gunAudio;

    // 玩家上次射击后的间隔时间
    private float nextFire;

    private void Start()
    {
        // 获取AudioSource组件
        gunAudio = GetComponent<AudioSource>();
    }

    public void Shoot()
    {
        // 在相机视口中心创建向量
        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        // 声明RaycastHit存储射线射中的对象信息
        RaycastHit hit;

        // 检测射线是否碰撞到对象
        if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
        {
            // 获取被射中对象上的ShootableBox组件
            ShootableBox health = hit.collider.GetComponent<ShootableBox>();

            // 如果组件存在
            if (health != null)
            {
                // 调用组件的Damage函数计算伤害
                health.Damage(gunDamage);
            }

            // 检测被射中的对象是否存在rigidbody组件
            if (hit.rigidbody != null)
            {
                // 为被射中的对象添加作用力
                hit.rigidbody.AddForce(-hit.normal * hitForce);
            }
        }
        else
        {
            // 如果未射中任何对象，则将射击轨迹终点设为相机前方的武器射程最大距离处
            //rayOrigin + (fpsCam.transform.forward * weaponRange)
        }
    }
}