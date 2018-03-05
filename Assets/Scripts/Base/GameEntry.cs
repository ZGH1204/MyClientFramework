using GGame.NetWork;
using GGame.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ZFramework;
using ZFramework.Event;
using ZFramework.Fsm;
using ZFramework.Network;
using ZFramework.Runtime;

namespace GGame
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        public Text downloadPro;

        private void Start()
        {
            DontDestroyOnLoad(this);

            InitBuiltinComponents();
            InitCustomComponents();

            Test();

             
        }
 
        private void Test()
        { 
            if (false)
            {
                GameEntry.Network.EventComponent = GameEntry.Event;

                INetworkChannelHelper channelHelper01 = new NetworkChannelHelper();
                INetworkChannel channel01 = GameEntry.Network.CreateNetworkChannel("Battle", channelHelper01);
                channel01.HeartBeatInterval = 100f;
                channel01.Connect(System.Net.IPAddress.Parse("10.246.52.157"), 9099);
                //channel01.Connect(System.Net.IPAddress.Parse("115.159.68.24"), 9099);

                //Debug.Log("*****************************************");
                //CSLogin msg = new CSLogin();

                //for (int i = 0; i < 0; i++)
                //{
                //    msg.PlayerId = "2";
                //    msg.HPValue = "2";
                //    msg.PacketType = 7;
                //    channel01.Send(msg, i.ToString());
                //}
            }

            if (false)
            {
                GameEntry.UI.OpenUIForm(UIFormId.LoginForm, null); 
            }

            if (true)
            { 
                GameEntry.Resource.SetBundleBaseURL("http://123.207.181.232/dev/");
                CoroutineHelper.Instance.StartCoroutine(PreDownloadInitBundle());
            }
        }

        IEnumerator PreDownloadInitBundle()
        {
            IEnumerator e = GameEntry.Resource.PreDownloadInitBundle();
            while (e.MoveNext()) { yield return null; }

            GameEntry.Resource.LoadAsset<Sprite>("Unitychan_Run_11", (sp)=> {
                GameObject.Find("Unitychan_Run_11").GetComponent<Image>().sprite = sp;
                GameObject.Find("Unitychan_Run_11").GetComponent<Image>().SetNativeSize();
            });
            
        }

        private void Update()
        {
            downloadPro.text = GameEntry.Resource.GetDownloadProgress().ToString();
        }



    }
     
}