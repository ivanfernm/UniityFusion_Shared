using Fusion;
using UnityEngine;

public class PlayerColor : NetworkBehaviour
{
    public SkinnedMeshRenderer _MeshRenderer;
    public PlayerBasics _PlayerBasics;

    //[Networked(OnChanged = nameof(NetworkColorChanger))]
    [Networked] Color NetworkedColor { get; set; }

    public void SetColor(Color color)
    {
        NetworkedColor = color;
    }
    public override void Spawned()
    {
        _PlayerBasics = GetComponent<PlayerBasics>();
    }

    public static void NetworkColorChanger(Changed<PlayerColor> changed)
    {
        changed.Behaviour._MeshRenderer.material.color = changed.Behaviour.NetworkedColor;
    }
}