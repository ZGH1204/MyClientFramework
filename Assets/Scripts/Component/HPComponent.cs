using GGame.NetWork;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HPComponent : MonoBehaviour
{
    public Action DeadEvent;

    public Transform followObj;
    private Slider slider;

    public float maxHp = 0;
    private float attackedValue = 0;
     
    PlayerComponent playerComponent;
    NPCComponent npcComponent;

    bool isHero;

    public void Init()
    {
        playerComponent = followObj.GetComponent<PlayerComponent>();
        npcComponent = followObj.GetComponent<NPCComponent>();

        isHero = playerComponent != null ? true : false;
        maxHp = isHero ? 600f : 200f;

        SyncPlayerData.Instance.HurtInfoBack += HurtInfoEvent;
        SyncPlayerData.Instance.PlayerInfoBack += PlayerInfo;

        slider = this.GetComponent<Slider>();
        slider.maxValue = maxHp / 200f;
        slider.value = maxHp / 200f;
        transform.position = Camera.main.WorldToScreenPoint(this.followObj.position) + new Vector3(0, 190f, 0);
           
    }

    public void SetHeroHPUIColor()
    {
        BloodImage img = this.transform.Find("Fill Area/Fill").GetComponent<BloodImage>();
        img.texture = Resources.Load<Texture>("BloodImage");
    }

    void HurtInfoEvent(SCHurtInfo info)
    {
        if (playerComponent != null)
        {
            if (playerComponent.playerUnitData.PlayerId == info.HurterId)
            { 
                float gezi = maxHp / 200f; 
                slider.value = gezi - attackedValue / 200f;
            }

            //攻击者积分加100
            if (PlayerManager.Instance.playerDic.ContainsKey(info.AttackerId) && info.AttackerId == PlayerManager.Instance.GetHeroPlayer().playerUnitData.PlayerId)
            {
                if (slider.value <= 0)
                {
                    ItemData.Instance.killNum++;
                    ItemData.Instance.grade += 100;

                    GGame.GameEntry.Event.Fire(this, new BattleUIEventArgs());
                }
                //PlayerManager.Instance.playerDic[info.AttackerId].playerUnitData.JiFen += 100;
                //ItemData.Instance.grade += 100;
            }
        }
        else if (npcComponent != null)
        {
            if (npcComponent.nPCId == info.HurterId)
            { 
                float gezi = maxHp / 200f; 
                slider.value = gezi - attackedValue / 200f;
            }

            //攻击者积分加50
            if (PlayerManager.Instance.playerDic.ContainsKey(info.AttackerId))
            {
                //PlayerManager.Instance.playerDic[info.AttackerId].playerUnitData.JiFen += 50;
                //ItemData.Instance.grade += 50;
            }
        }

        if (slider.value <= 0)
        {
            if (DeadEvent != null)
            {
                DeadEvent.Invoke();
            }
        }
    }

    void PlayerInfo(SCPlayerInfo info)
    {
        if (playerComponent != null)
        {
            if (playerComponent.playerUnitData.PlayerId == info.PlayerId)
            {
                maxHp = info.MaxHP;

                float gezi = maxHp / 200f; 
                slider.value = info.CurHP / 200f;
            }
        }
        else if (npcComponent != null)
        {
            if (npcComponent.nPCId == info.PlayerId)
            { 
                float gezi = maxHp / 200f; 
                slider.value = gezi - attackedValue / 200f;
            }
        } 
    }

    private void Update()
    {  
        transform.position = Camera.main.WorldToScreenPoint(followObj.position) + new Vector3(0, 190f, 0);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    public float GetCurHp()
    {
        return maxHp - attackedValue;
    }
     
    public void AddAttackHp(int value)
    {
        attackedValue += value; 
    }

    public void SetEnable(bool boo)
    {
        gameObject.SetActive(boo);
        if (boo)
        {
            attackedValue = 0;
            slider.maxValue = maxHp / 200f;
            slider.value = maxHp / 200f;
        }
    }
     
}