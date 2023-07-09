using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class GameStateHandeler : NetworkBehaviour
{
    public enum GameState
    {
        Starting,
        Running,
        Ending
    }

    public enum GameResult
    {
        Win,
        Lose
    }


    [SerializeField] private float _PlayersToStart = 2;

    [Networked] public GameState _gameState { get; set; }
    [Networked] private TickTimer _timer { get; set; }
    [SerializeField] private float _startDelay = 4.0f;
    [SerializeField] private Text StateText;
    [SerializeField] private CanvaManager CanvasHandeler;
    [SerializeField] private GameObject _WaitingPanel;

    public bool GameIsRunning => _gameState == GameState.Running;

    [Networked] private NetworkBehaviourId _winner { get; set; }
    [Networked, Capacity(2)] private NetworkLinkedList<NetworkBehaviourId> _playerDataNetworkedIds => default;

    public override void Spawned()
    {
        if (Object.HasStateAuthority == false) return;

        if (_gameState != GameState.Starting)
        {
            //buscar a todos los players que estan activos y agregarlos a la lista
            foreach (var VARIABLE in Runner.ActivePlayers)
            {
                if (Runner.TryGetPlayerObject(VARIABLE, out var playerObject) == false) continue;
                RPC_TrackNewPlayer(playerObject.GetComponent<PlayerBasics>().Id);
            }
        }

        _gameState = GameState.Starting;
        _timer = TickTimer.CreateFromSeconds(Runner, _startDelay);
    }


    public override void FixedUpdateNetwork()
    {
        switch (_gameState)
        {
            case GameState.Starting:
                _WaitingPanel.gameObject.SetActive(true);
                StateText.text = "STARTING";
                StateText.color = Color.yellow;
                updateStartig();
                break;
            case GameState.Running:

                updateRunning();
                StateText.text = "RUNNING";
                StateText.color = Color.green;
                break;
            case GameState.Ending:
                _timer = TickTimer.CreateFromSeconds(Runner, _startDelay);
                updateEnding();
                _WaitingPanel.gameObject.SetActive(false);
                StateText.text = "ENDING";
                StateText.color = Color.red;
                break;
        }
    }

    private void updateStartig()
    {
        if (Object.HasStateAuthority == false) return;

        if (_timer.ExpiredOrNotRunning(Runner) == false)
        {
            return;
        }

        if (_timer.Expired(Runner))
        {
            if (Runner.ActivePlayers.Count() >= _PlayersToStart)
            {
                //spawn the player
                FindObjectOfType<PlayerSpawner>().startSpawnPlayer(this);
                RPC_SpawnReadyPlayers();
                _WaitingPanel.gameObject.SetActive(false);
                _gameState = GameState.Running;
            }
            else
            {
                _timer = TickTimer.CreateFromSeconds(Runner, _startDelay);
            }
        }
      
    }

    private void updateRunning()
    {
        _WaitingPanel.gameObject.SetActive(false);
   
    }

    private void updateEnding()
    {
        if (!_timer.Expired(Runner))
        {
            var a = FindObjectOfType<Health>();
            if (a.IsDead != true)
            {
                a.RPC_win();
            }
        }
        else
        {
            Runner.Shutdown();
        }
        
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_CheckIfGameHasEnded()
    {
        if (Object.HasStateAuthority == false) return;

        int playersAlive = 0;

        for (int i = 0; i < _playerDataNetworkedIds.Count; i++)
        {
            if (Runner.TryFindBehaviour(_playerDataNetworkedIds[i],
                    out Health playerHealth) == false)
            {
                _playerDataNetworkedIds.Remove(_playerDataNetworkedIds[i]);
                i--;
                continue;
            }

            if (playerHealth.IsDead != true) playersAlive++;
        }


        if (playersAlive > 1) return;

        foreach (var playerDataNetworkedId in _playerDataNetworkedIds)
        {
            if (Runner.TryFindBehaviour(playerDataNetworkedId,
                    out Health playerhealth) ==
                false) continue;

            if (playerhealth.IsDead == true) continue;

                playerhealth.RPC_win(); 
                _winner = playerDataNetworkedId;
           
           
        }

        GameHasEnded();
    }

    private void GameHasEnded()
    {
        _gameState = GameState.Ending;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Ending(GameResult result)
    {
        StartCoroutine(GameEnding(result));
    }

    public IEnumerator GameEnding(GameResult result)
    {
        _gameState = GameState.Ending;
        if (result == GameResult.Win)
        {
            CanvasHandeler.Win();
        }
        else
        {
            CanvasHandeler.Lose();
        }

        yield return new WaitForSeconds(5f);
    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_TrackNewPlayer(NetworkBehaviourId playerDataNetworkedId)
    {
        _playerDataNetworkedIds.Add(playerDataNetworkedId);
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_SpawnReadyPlayers()
    {
        FindObjectOfType<PlayerSpawner>().SpawnPlayer(Runner.LocalPlayer);
    }
}