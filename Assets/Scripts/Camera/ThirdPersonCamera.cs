using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
   public Transform _target;
    [SerializeField] private float distance = 5f;
    [SerializeField] private float height = 1f;
    [SerializeField] private float smoothSpeed = 1f;

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (_target == null)
        {
            return;
        }

        // Calculate the desired position based on the target's position and camera offset
        Vector3 targetPosition = _target.position - _target.forward * distance;
        targetPosition.y = _target.position.y + height;

        // Smoothly move the camera towards the desired position using SmoothDamp
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed);

        // Look at the target
        transform.LookAt(_target.position + _target.forward);
    }
}
