using ZFramework.Network;
using GGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGame.NetWork;

public class NetWorkTest : MonoBehaviour {

	// Use this for initialization
	void Start () {

        

        INetworkChannelHelper channelHelper01 = new NetworkChannelHelper();
        INetworkChannel channel01 = GameEntry.Network.CreateNetworkChannel("", channelHelper01);
        channel01.Connect(System.Net.IPAddress.Parse("137.80.80.1"), 8888); 

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
