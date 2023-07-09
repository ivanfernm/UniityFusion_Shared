using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class CanvaManager : NetworkBehaviour
{
    
    public Text StateText;

    public GameObject waitingPannel;
    
    public enum gameState
    {
        none,
        win,
        lose,
    }
    [Networked]public gameState _gameState { get;  set; }
    public GameObject winGanvas ;
    public GameObject loseGanvas;    

    public override void Spawned()
    {
        _gameState = gameState.none;
    }

    
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_ChangeGameState(gameState state)
    {
        _gameState = state;
        
        switch (_gameState)
        {
            case gameState.win:
                Win();
                break;
            
            case gameState.lose:
                Lose();
                break;
            
        }
        
    }
    
    
    public void Win()
    {
        winGanvas.SetActive(true);
    }
    
    public void Lose()
    {
        
        loseGanvas.SetActive(true);

    }
    public void Quit()
    {
        Application.Quit();
    }
}
