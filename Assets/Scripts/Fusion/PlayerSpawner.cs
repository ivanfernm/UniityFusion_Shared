using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, ISpawned  
{
    
    [SerializeField]private NetworkPrefabRef _PlayerPrefab = NetworkPrefabRef.Empty;
    
    private GameStateHandeler _gameStateHandeler;
    
    [SerializeField]Vector3 SpawnPosition;

    [SerializeField] private PlayerRef a;
    
    //List of spawn points in which the player can spawn
    public SpawnPoint[] _spawnPoints;

    public void Spawned()
    {
        _spawnPoints = FindObjectsOfType<SpawnPoint>();
        
        if (FindObjectOfType<GameStateHandeler>().GameIsRunning)
        {
            SpawnPlayer(Runner.LocalPlayer);
        }
    }

    public void startSpawnPlayer(GameStateHandeler gameStateHandeler)
    {
        _gameStateHandeler = gameStateHandeler;
    }
    public void SpawnPlayer(PlayerRef runnerLocalPlayer)
    {
       
        int index = runnerLocalPlayer % _spawnPoints.Length;
        SpawnPosition = _spawnPoints[index].transform.position;
        
        var playerObject = Runner.Spawn(_PlayerPrefab,SpawnPosition, Quaternion.identity, runnerLocalPlayer);
        Runner.SetPlayerObject(runnerLocalPlayer, playerObject);
        
        //if player is spawned, game state controller is ready
        if (!_gameStateHandeler)
            _gameStateHandeler = FindObjectOfType<GameStateHandeler>();
        
        _gameStateHandeler.RPC_TrackNewPlayer(playerObject.GetComponent<PlayerBasics>().Id);
    }
    
    
    
}