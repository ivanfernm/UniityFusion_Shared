using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class LifeBar : NetworkBehaviour
{
    public PlayerBasics _myPlayer;
    
    [SerializeField] MeshRenderer _meshRenderer;
    [SerializeField] private Material _lifeBarMat;

    private void Awake()
    {
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        _lifeBarMat = _meshRenderer.material;
    }

    public override void Spawned()
    {
        //_myPlayer.UpdateLifeBar += UpdateLife;
    }

    public void RestLifeBar(){}

    public void UpdateLife(float CurrentLife)
    {
        _lifeBarMat.SetFloat("_Amount", CurrentLife);
    }
}
