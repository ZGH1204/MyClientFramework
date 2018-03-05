using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGame.NetWork
{ 
    public enum PacketId : int
    {
        HeartBeat,
        Login,
        Position,
        CreatePlayer,
        HurtInfo,
        PlayerInfo
    }
     
}
