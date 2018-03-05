using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZFramework.Event;

public class BattleUIForm : MonoBehaviour {

    public Action clickAddSpeedSkillEvent;

    /// <summary>
    /// 加速技能
    /// </summary>
    public UISkillComponent addSpeedSkill1;

    public ETCJoystick move_Joystick;
    public Button attackBtn;
    public CameraFollowComponent followCamera;

    public Transform HPUIPrefab;
    public Text itemTxt;
    public Text cooldownTxt;
    public Text gradeTxt;

    int m_Cooldown;

    // Use this for initialization
    void Start () {

        GGame.GameEntry.Event.Subscribe(BattleUIEventArgs.EventId, (sender, e) =>
        {
            BattleUIEventArgs arg = e as BattleUIEventArgs;

            ShowItem();
            ShowGrade();
        });

        addSpeedSkill1.clickEvent += ClickAddSpeedSkill;

        ShowCoolDown(); 
    }
     

    void ShowGrade()
    {
        gradeTxt.text = string.Format("当前积分：{0}", ItemData.Instance.grade);
    }
   
    void ShowItem()
    {
        itemTxt.text = string.Format("击杀：{0}   物资：{1}   稀有物资：{2}", ItemData.Instance.killNum, ItemData.Instance.GetItemNum(), ItemData.Instance.GetRareItemNum());
    }

    void ShowCoolDown()
    {
        m_Cooldown = 600; 

        GGame.GameEntry.Cooldown.AddTimer(this.GetHashCode(), 1, () =>
        {
            if (cooldownTxt == null)
            {
                GGame.GameEntry.Cooldown.RemoveTimer(this.GetHashCode());
                return;
            }
            cooldownTxt.text = string.Format("剩余时间    {0:D2}:{1:D2}", m_Cooldown / 60, m_Cooldown % 60);
            m_Cooldown--;
            if (m_Cooldown < 0)
            {
                GGame.GameEntry.Cooldown.RemoveTimer("战斗倒计时".GetHashCode());
            }
        }).Start();
    }


    public void OnDestroy()
    {
        GGame.GameEntry.Event.UnsubscribeAll();
    }


    /// <summary>
    /// 增加血条
    /// </summary>
    /// <returns></returns>
    public HPComponent AddPlayerHPUI(PlayerComponent player)
    {
        Transform obj = Instantiate(HPUIPrefab);
        obj.gameObject.transform.SetParent(transform.Find("HP"));
        HPComponent hp = obj.GetComponent<HPComponent>();
        hp.followObj = player.transform;
        hp.Init();

        return hp;
    }

    /// <summary>
    /// 增加血条
    /// </summary>
    /// <returns></returns>
    public HPComponent AddNPCHPUI(NPCComponent npc)
    {
        Transform obj = Instantiate(HPUIPrefab);
        obj.localScale = Vector3.one;
        obj.gameObject.transform.SetParent(transform.Find("HP"));
        HPComponent hp = obj.GetComponent<HPComponent>();
        hp.followObj = npc.transform;
        hp.Init();

        return hp;
    }

    /// <summary>
    /// 加速技能按钮点击事件
    /// </summary>
    void ClickAddSpeedSkill()
    {
        if (clickAddSpeedSkillEvent != null)
        {
            clickAddSpeedSkillEvent.Invoke();
        }
    }

    public void SetMoveJoystick(PlayerControlComponent player)
    { 
        move_Joystick.onMoveStart.AddListener(player.MoveStart);
        move_Joystick.onMove.AddListener(player.Move);
        move_Joystick.onMoveEnd.AddListener(player.MoveEnd);
    }


    public void SetAttackBtn(PlayerControlComponent player)
    {
        attackBtn.onClick.AddListener(player.Attack); 
    }

    public void SetFollowCamera(PlayerControlComponent player)
    {
        followCamera.target = player.transform;
    }
}

public class BattleUIEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(BattleUIEventArgs).GetHashCode();

    public BattleUIEventArgs()
    {
    }

    public override int Id
    {
        get
        {
            return EventId;
        }
    }
}
