using GGame;
using GGame.NetWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFramework.Network;

public class PlayerControlComponent : MonoBehaviour {

     
    public PlayerComponent playerComponent;


    Vector3 delatPosition;
    bool isAddSpeed;
    INetworkChannel channel01;
    bool controlMoveByUI;
    float addSpeedRate;
    int curAnimation;

    void Start () {

        addSpeedRate = 1.0f;
        channel01 = GameEntry.Network.GetNetworkChannel("Battle");

    }
	 
	void Update () {

        channel01.Send<CSPosition>(new CSPosition()
        {
            PlayerId = playerComponent.playerUnitData.PlayerId,
            X = delatPosition.x,
            Y = delatPosition.y,
            Z = delatPosition.z,
            Animation = curAnimation
        });

        //攻击响应一次
        if (curAnimation == 3)
        {
            curAnimation = 1;
        }
    }

    /// <summary>
    /// 普攻
    /// </summary>
    public void Attack()
    {
        curAnimation = 3;
    }
    


    /// <summary>
    /// 加速技能
    /// </summary>
    public void AddSpeedSkillEvent()
    {
        if (isAddSpeed)
        {
            return;
        }
         
        isAddSpeed = true;
        playerComponent.SetAnimatorSpeed(2f);
        addSpeedRate = 2.0f;

        GGame.GameEntry.Cooldown.AddOnceTimer("AddMoveSpeed".GetHashCode(), 3f, () => {
            isAddSpeed = false;
            playerComponent.SetAnimatorSpeed(1f);
            addSpeedRate = 1.0f;
            
        }).Start();
    }

    /// <summary>
    /// 移动开始
    /// </summary>
    public void MoveStart()
    {
        delatPosition = Vector3.zero;
        controlMoveByUI = true;
    }

    /// <summary>
    /// 移动...
    /// </summary>
    /// <param name="deltaPosition"></param>
    public void Move(Vector2 deltaPosition)
    {
        curAnimation = 2;
        delatPosition = new Vector3(deltaPosition.x, 0, deltaPosition.y) * 5.5f * Time.deltaTime * addSpeedRate;
    }

    /// <summary>
    /// 移动停止
    /// </summary>
    public void MoveEnd()
    {
        delatPosition = Vector3.zero;
        controlMoveByUI = false;
        curAnimation = 1;
    }
}
