using System;
using Fusion;
using UnityEngine;

public class PlayerBasics : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnNickNameChanged))]
    public NetworkString<_16> nickName {get;set;}

    private CanvaManager _overviewPanel;

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
           var name  = FindObjectOfType<PlayerInfo>().getNickName();
           nickName = name;
        }
    }

    public static void OnNickNameChanged(Changed<PlayerBasics> playerInfo)
    {
        //modify canvas
        //playerInfo.Behaviour._overviewPanel.UpdateNickName(playerInfo.Behaviour.Object.InputAuthority,
            //playerInfo.Behaviour.NickName.ToString());
    }
}