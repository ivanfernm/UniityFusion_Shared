using System;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] public Transform _targer;
    [SerializeField] private float MouseSensitivity = 10f;
    [SerializeField] private float OffsetX = 2f;
    [SerializeField] private float OffsetY = 2f;
    [SerializeField] private float OffsetZ = 2f;
    private float VerticalRotation;
    private float HorizontalRotation;

    private void LateUpdate()
    {
        if (_targer == null)
        {
            return;
        }

        var offsets = new Vector3(OffsetX, OffsetY, OffsetZ);
        transform.position = _targer.position + offsets;

        float mouseX = Input.GetAxis("Mouse X");
        float mousey = Input.GetAxis("Mouse Y");

        VerticalRotation -= mousey * MouseSensitivity;
        VerticalRotation = Mathf.Clamp(VerticalRotation, -70f, 70f);


        HorizontalRotation += mouseX * MouseSensitivity;
        //transform.rotation = _targer.rotation * Quaternion.Euler(MouseSensitivity, MouseSensitivity, MouseSensitivity);
        
        transform.LookAt(_targer.transform.position);
        transform.rotation = _targer.rotation;
        //transform.rotation = Quaternion.Euler(0,HorizontalRotation,0);
        //transform.rotation = Quaternion.Euler(0, _targer.rotation.y, 0);

    }
    
}
