using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZFramework;
using ZFramework.Runtime;

public class ShopEquipForm : MonoBehaviour {

    public GameObject shopUI;
    public Transform content;
    public Transform child;

    
    
	void Start () {

        var dic = GGame.GameEntry.DateTable.GetCSVByName<EquipCSV>(DataTableName.Equip);
        foreach (var item in dic.Keys)
        {
            var go = Instantiate(child);
            go.SetParent(content);
            go.gameObject.SetActive(true);
            go.name = item.ToString();


            go.Find("Icon/name").GetComponent<Text>().text = dic[item].Name;
            go.Find("price").GetComponent<Text>().text = "积分:" + dic[item].NeedGrade.ToString();
            go.GetComponentInChildren<Button>().onClick.AddListener(() => {

                int needGrade = dic[int.Parse(go.name)].NeedGrade;
                if (ItemData.Instance.grade > needGrade)
                {
                    ItemData.Instance.AddEquip(int.Parse(go.name), 1);
                    ItemData.Instance.grade -= dic[int.Parse(go.name)].NeedGrade;
                    GGame.GameEntry.Event.Fire(this, new BattleUIEventArgs());
                }
                else
                {
                    Log.Info("积分不足");
                }
               
            });
        }
         
        shopUI.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void Open( )
    {
        shopUI.SetActive(!shopUI.activeSelf);
    }


    public void ExchangeGrade()
    {
        ItemData.Instance.ExchangeGrade();
        GGame.GameEntry.Event.Fire(this, new BattleUIEventArgs());
    }

}
