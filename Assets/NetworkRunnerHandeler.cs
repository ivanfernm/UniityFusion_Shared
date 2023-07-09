using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkRunnerHandeler : MonoBehaviour,IPlayerJoined
{
    [SerializeField]NetworkRunner _runner;

    void Start()
    {
        _runner = GetComponent<NetworkRunner>();
        var clientTask = IntializeNetworkRunner(_runner, GameMode.Shared, SceneManager.GetActiveScene().buildIndex);
    }
    

    Task IntializeNetworkRunner(NetworkRunner runner, GameMode gameMode, SceneRef scene)
    {
        var sceneObject = runner.GetComponent<NetworkSceneManagerDefault>();

        runner.ProvideInput = true;

     
            return runner.StartGame(new StartGameArgs
            {
                PlayerCount = 2,
                GameMode = gameMode,
                Scene = scene,
                SessionName = "GameSession",
                SceneManager = sceneObject
            });
            
        
    }

    public void PlayerJoined(PlayerRef player)
    {
        if (_runner.ActivePlayers.Count() >= 1)
        {
        }
        else
        {
            Debug.Log("Esperando a que se conecte otro jugador");
        }
    }
}
