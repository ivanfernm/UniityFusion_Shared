using System;
using UnityEngine;


public class PlayerInfo : MonoBehaviour
{
    private string _nickName;

    private void Start()
    {
        var count = FindObjectsOfType<PlayerInfo>().Length;
        if (count > 1)
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
    }
    
    public void setNickName(string nickName)
    {
        _nickName = nickName;
    }
    
    public string getNickName()
    {
        if (string.IsNullOrWhiteSpace(_nickName))
        {
            _nickName = GetRandomNickName();
        }

        return _nickName;
    }
    
    public static string GetRandomNickName()
    {
        var rngPlayerNumber = UnityEngine.Random.Range(0, 9999);
        return $"Player {rngPlayerNumber.ToString("0000")}";
    }
}