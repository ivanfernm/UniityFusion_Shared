using System;
using UnityEngine;

public class CameraClickManager : MonoBehaviour
{
    public Vector3 worldPos;
    private Plane _plane = new Plane(Vector3.up, 0);
    
    private void Update()
    {
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (_plane.Raycast(ray, out distance))
        {
            worldPos = ray.GetPoint(distance);
        }
    }
}