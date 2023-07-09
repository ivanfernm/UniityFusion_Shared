using Fusion;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [Networked(OnChanged = nameof(NetworkedLifeChanged))]
    public float NetworkedLife { get; set; } = 100;

    [Networked(OnChanged = nameof(NetworkedWinChanged))]
    public NetworkBool isWinner { get; set; } = false;

    public bool IsDead => NetworkedLife <= 0;

    public override void Spawned()
    {
     
    }

    public override void FixedUpdateNetwork()
    {
    }
    

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)] // All, Server, Others, Owner,
    public void RPC_getHit(float dmg)
    {
        //Debug.Log("Recived damage");
        if (NetworkedLife >= 0)
        {
            NetworkedLife -= dmg;
            FindObjectOfType<GameStateHandeler>().RPC_CheckIfGameHasEnded();
          
        }
        else if(NetworkedLife <= 0)
        {
            FindObjectOfType<CanvaManager>().loseGanvas.SetActive(true);
        }
    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_win()
    {
        isWinner = true;
        FindObjectOfType<CanvaManager>().winGanvas.SetActive(true);
    }
    private void Update()
    {
       
        if (NetworkedLife <= 0)
        {
            Runner.Despawn(Object);
        }
    }
    
    private static void NetworkedLifeChanged(Changed<Health> changed)
    {
       
    }
    private static void NetworkedWinChanged(Changed<Health> changed)
    {
       
    }  
    
}