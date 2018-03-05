using UnityEngine;

public class ShootableBox : MonoBehaviour
{
    // 总血量
    public int currentHealth = 3;

    public void Damage(int damageAmount)
    {
        // 调用Damage时总血量要减去伤害值
        currentHealth -= damageAmount;

        // 检测总血量是否小于等于0
        if (currentHealth <= 0)
        {
            // 隐藏对象
            gameObject.SetActive(false);
        }
    }
}