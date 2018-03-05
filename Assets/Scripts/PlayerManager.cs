using GGame;
using GGame.NetWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZFramework.Network;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager Instance;

    public Transform unitPrefab;
    public BattleUIForm battleUIForm;

    public List<Transform> randomTansformList;

    public Dictionary<int, PlayerComponent> playerDic;



    INetworkChannel channel01;

    private void Awake()
    {
        Instance = this;
        NPCManager.Instance.battleUIForm = battleUIForm;
    }
     
    // Use this for initialization
    void Start () {
         
        SyncPlayerData.Instance.CreatePlayerBack += CreatePlayerEvent;

        playerDic = new Dictionary<int, PlayerComponent>();

        channel01 = GameEntry.Network.GetNetworkChannel("Battle");
        channel01.Send<SCCreatePlayer>(new SCCreatePlayer() {
            PlayerId = channel01.LocalPort,
            x = randomTansformList[0].position.x,
            y = randomTansformList[0].position.y,
            z = randomTansformList[0].position.z,
        });
        //channel01.Send<SCCreatePlayer>(new SCCreatePlayer() { PlayerId = channel01.LocalPort - 1, PositionId = 2 });
        //channel01.Send<SCCreatePlayer>(new SCCreatePlayer() { PlayerId = channel01.LocalPort - 2, PositionId = 3 });
    }

    /// <summary>
    /// 创建角色事件
    /// </summary>
    void CreatePlayerEvent(SCCreatePlayer info)
    { 
        UnitData heroData = new UnitData();
        heroData.HPValue = 100;
        heroData.IsAddSpeed = false;
        heroData.IsAttack = false;
        heroData.JiFen = 0;
        heroData.PlayerId = info.PlayerId;
        heroData.Postion = new Vector3(info.x, info.y, info.z);

        if (!playerDic.ContainsKey(info.PlayerId))
        {
            var player = CreatePlayer(heroData);
            if (info.PlayerId == channel01.LocalPort)
            {
                CreateHero(player);
            }
            else
            {
                PlayerComponent hero = GetHeroPlayer();
                channel01.Send<SCCreatePlayer>(new SCCreatePlayer() {
                    PlayerId = hero.playerUnitData.PlayerId,
                    x = hero.transform.position.x,
                    y = hero.transform.position.y,
                    z = hero.transform.position.z,
                });
            }
        }
        
    }

    // Update is called once per frame
    void Update () {
		
	}


    public void CreateHero(PlayerComponent player)
    {
        var playerControlComponent = player.gameObject.AddComponent<PlayerControlComponent>();
        playerControlComponent.playerComponent = player;

        player.hPComponent.SetHeroHPUIColor();
        //增加点击加速技能
        battleUIForm.clickAddSpeedSkillEvent += playerControlComponent.AddSpeedSkillEvent;
        battleUIForm.addSpeedSkill1.StartCD();

        battleUIForm.SetMoveJoystick(playerControlComponent);
        battleUIForm.SetFollowCamera(playerControlComponent);
        battleUIForm.SetAttackBtn(playerControlComponent);

    }

    PlayerComponent CreatePlayer(UnitData unitData)
    { 
        //实例化
        Transform player = Instantiate(unitPrefab);
        player.SetParent(this.transform);
        player.name = unitData.PlayerId.ToString();
        //增加血条
        var playerComponent = player.gameObject.AddComponent<PlayerComponent>();
        var hp = battleUIForm.AddPlayerHPUI(playerComponent);
    
        playerComponent.SetHPComponent(hp);
        playerComponent.playerUnitData = unitData;

        //设置随机位置
        player.position = unitData.Postion;
        player.eulerAngles = unitData.eulerAngles;

        playerDic.Add(unitData.PlayerId, playerComponent);
        UnitDatas.Instance.AddPlayerUnit(unitData);

        return playerComponent;
    }


    public PlayerComponent GetHeroPlayer()
    {
        PlayerComponent hero;
        if (playerDic.TryGetValue(channel01.LocalPort, out hero))
        {
            
        }

        return hero;
    }

     
}
