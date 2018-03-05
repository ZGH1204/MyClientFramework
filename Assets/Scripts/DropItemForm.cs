using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZFramework.Event;
using ZFramework.Runtime;

public class DropItemForm : MonoBehaviour
{ 
    public Transform sv;
    public Transform content;
    public GameObject child;

    // Use this for initialization
    private void Start()
    {
        GGame.GameEntry.Event.Subscribe(DropItemEventArgs.EventId, DropItemFun);
        sv.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void Open(object sender, List<int> items)
    {
        ClearChild(); 
        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, items.Count * 110 + 20);

        for (int i = 0; i < items.Count; i++)
        {
            GameObject go = Instantiate(child);
            go.transform.SetParent(content);
            go.SetActive(true);
            go.name = items[i].ToString();

            DropItemCSV info = GGame.GameEntry.DateTable.GetTypeById<DropItemCSV>(DataTableName.DropItem, items[i]);

            go.GetComponentInChildren<Text>().text = info.Name;
            go.GetComponentInChildren<Button>().onClick.AddListener(()=> {
                PickItem(sender, int.Parse(go.name));
            });
        }
    }

    private void DropItemFun(object sender, GameEventArgs e)
    {
        DropItemEventArgs args = (DropItemEventArgs)e;

        bool boo = args.ItemId == null || args.ItemId.Count == 0;
        if (sv != null)
        {
            sv.gameObject.SetActive(!boo);
            if (!boo)
            {
                Open(sender, args.ItemId);
            }
        }
    }

    private void ClearChild()
    {
        for (int i = 0; i < content.childCount; i++)
        {
            if (content.GetChild(i) != null)
            {
                Destroy(content.GetChild(i).gameObject);
            }
        }
    }

    private void PickItem(object sender, int id)
    {
        DropItemCollider info = sender as DropItemCollider;
        info.PickItem(id);

        ItemData.Instance.AddItem(id);

        GGame.GameEntry.Event.Fire(this, new BattleUIEventArgs());

        Open(sender, info.itemId);

    }
}