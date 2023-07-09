using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    PlayerMovement _myModel;

    NetworkInputData _inputData;
    
    [SerializeField] KeyCode shootKey;
    bool _isFirePressed;

    void Start()
    {
        _myModel = GetComponent<PlayerMovement>();
        _inputData = new NetworkInputData();
    }

    void Update()
    {
        if (!_myModel.HasInputAuthority) return;

        _isFirePressed = Input.GetKeyDown(shootKey);
    }

    public NetworkInputData GetNetworkInput()
    {
        
        _inputData.isFirePressed = _isFirePressed;

        return _inputData;
    }
}
