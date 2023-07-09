using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class RaycastAttack : NetworkBehaviour
{
    public float Damage;
    public KeyCode AttackKey;
    public PlayerMovement _PlayerMovement;
    public GameObject _spawnPoint;
    
    public LineRenderer lineRenderer;
    private Ray lastcast;

    public override void Spawned()
    {
        _PlayerMovement = GetComponent<PlayerMovement>();
        lineRenderer = _spawnPoint.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        if (HasStateAuthority == false)
        {
            return;
        }

    

        if (Input.GetKeyDown(AttackKey))
        {
            Ray ray = _PlayerMovement.camera.ScreenPointToRay(Input.mousePosition);
            ray.origin = _spawnPoint.transform.position;
            ray.origin += _PlayerMovement.gameObject.transform.forward;
            lastcast = ray;
           
            StartCoroutine(DrawRaycast(4));
            lineRenderer.SetPosition(0,ray.origin);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2);
        }

        if (Runner.GetPhysicsScene().Raycast(lastcast.origin,lastcast.direction,out var hit))
        {
            if (hit.transform.TryGetComponent<Health>(out var health))
            {
                if (health.Object.HasStateAuthority == false)
                {
                     health.RPC_getHit(Damage);
                }
            }
            lineRenderer.SetPosition(1,hit.point);
        }
        
    }

    public IEnumerator DrawRaycast(float duration)
    {
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(duration);
        lineRenderer.enabled = false;
    }
}
