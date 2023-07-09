using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    static public GameManager Instance { get; private set; }
    
    public override void Spawned()
    {
        if (Instance) Destroy(gameObject);
        else Instance = this;

    }
    
}
