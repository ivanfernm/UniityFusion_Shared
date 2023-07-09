using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
   [Header("Runner")]
   [SerializeField] private NetworkRunner _networkRunnerPrefab = null;
   [SerializeField] private PlayerInfo _playerDataPrefab = null;

   [Header("Inputfields")]
   [SerializeField] private InputField _nickName;
   [SerializeField] private InputField _roomName;
   [SerializeField] private string _gameSceneName;

   private NetworkRunner _runnerInstance;
   
   
   public void StartSharedSession()
   {
      SetPlayerInfo();
      StartGame(GameMode.Shared, _roomName.text, _gameSceneName);
   }

   private void SetPlayerInfo()
   {
         var playerInfo = FindObjectOfType<PlayerInfo>();
      if (playerInfo == null)
      {
         playerInfo = Instantiate(_playerDataPrefab);
      }
      
      if (string.IsNullOrWhiteSpace(_nickName.text))
      {
         playerInfo.setNickName(_nickName.placeholder.ToString());
      }
      else
      {
         playerInfo.setNickName(_nickName.text);
      }
   }
   
   private async void StartGame(GameMode mode, string roomName, string sceneName)
   {
      _runnerInstance = FindObjectOfType<NetworkRunner>();
      if (_runnerInstance == null)
      {
         _runnerInstance = Instantiate(_networkRunnerPrefab);
      }

      // Let the Fusion Runner know that we will be providing user input
      // Keep in mind that inputs are not send in shared mode
      // But you can still get them locally, or use local unity inputs if you want to.
      _runnerInstance.ProvideInput = true;

      var startGameArgs = new StartGameArgs()
      {
         GameMode = mode,
         SessionName = roomName,
      };

      await _runnerInstance.StartGame(startGameArgs);

      _runnerInstance.SetActiveScene(sceneName);
   }
}

