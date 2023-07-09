using UnityEngine;

namespace Entities.Camera
{
    public class CameraOrbit : MonoBehaviour
    {
        private Vector2 angle = new Vector2(90 * Mathf.Deg2Rad, 0);
        private new UnityEngine.Camera camera;
        private Vector2 nearPlaneSize;

        public Transform follow;
        public float maxDistance = 3;
        [SerializeField] private float minDistance = 0.2f;
        public Vector2 sensitivity = new Vector2(3, 3);

        public bool mouseOrbitEnabled = true;
        public float orbitSpeedX = 1f;
        public float orbitSpeedY = 1f;
        public LayerMask ignoreLayer;

        public bool showGizmos = true;

        // Damping variables
        public float positionDamping;
        public float rotationDamping;

        public float xDistanceThreshold = 5f;

        private Vector3 targetPosition;
        private Quaternion targetRotation;

        void Start()
        {
            if (follow == null)
            {
                return;
            }
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            camera = GetComponent<UnityEngine.Camera>();

            CalculateNearPlaneSize();//
        }

        private void CalculateNearPlaneSize()
        {
            float height = Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad / 2) * camera.nearClipPlane;
            float width = height * camera.aspect;

            nearPlaneSize = new Vector2(width, height);
        }

        private Vector3[] GetCameraCollisionPoints(Vector3 direction)
        {
            Vector3 position = follow.position;
            Vector3 center = position + direction * (camera.nearClipPlane + minDistance);

            Vector3 right = transform.right * nearPlaneSize.x;
            Vector3 up = transform.up * nearPlaneSize.y;

            return new Vector3[]
            {
                center - right + up,
                center + right + up,
                center - right - up,
                center + right - up
            };
        }

        void Update()
        {
            if (mouseOrbitEnabled)
            {
                float hor = Input.GetAxis("Mouse X");

                if (hor != 0)
                {
                    angle.x += hor * Mathf.Deg2Rad * sensitivity.x;
                }

                float ver = Input.GetAxis("Mouse Y");

                if (ver != 0)
                {
                    angle.y += ver * Mathf.Deg2Rad * sensitivity.y;
                    angle.y = Mathf.Clamp(angle.y, -80 * Mathf.Deg2Rad, 80 * Mathf.Deg2Rad);
                }
            }
            else
            {
                angle.x = Mathf.Deg2Rad * orbitSpeedX;
                angle.y = Mathf.Deg2Rad * orbitSpeedY;
                angle.y = Mathf.Clamp(angle.y, -80 * Mathf.Deg2Rad, 80 * Mathf.Deg2Rad);
            }
        }

        void LateUpdate()
        {
            Vector3 direction = new Vector3(
                Mathf.Cos(angle.x) * Mathf.Cos(angle.y),
                -Mathf.Sin(angle.y),
                -Mathf.Sin(angle.x) * Mathf.Cos(angle.y)
            );

            float distanceX = Mathf.Abs(transform.position.x - follow.position.x);
            bool lerpX = distanceX > xDistanceThreshold;

            RaycastHit hit;
            float distance = maxDistance;
            Vector3[] points = GetCameraCollisionPoints(direction);

            foreach (Vector3 point in points)
            {
                if (Physics.Raycast(point, direction, out hit, maxDistance, ~ignoreLayer))
                {
                    distance = Mathf.Min((hit.point - follow.position).magnitude, distance);
                }
            }

            Vector3 targetPosition = follow.position + direction * distance;
            Quaternion targetRotation = Quaternion.LookRotation(follow.position - targetPosition);

            // Apply damping
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * positionDamping);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationDamping);

            // Lerp towards the target in X axis if necessary
            if (lerpX)
            {
                float lerpFactorX = distanceX / xDistanceThreshold;
                float lerpedX = Mathf.Lerp(transform.position.x, follow.position.x, lerpFactorX);
                transform.position = new Vector3(lerpedX, transform.position.y, transform.position.z);
            }
        }

        private void OnDrawGizmos()
        {
            if (showGizmos)
            {
                // Visualizar órbita de la cámara
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(follow.position, maxDistance);

                // Visualizar límites de movimiento en grados
                Gizmos.color = Color.red;
                float angleRange = 80f;
                float halfAngleRange = angleRange / 2f;
                Quaternion leftLimit = Quaternion.Euler(0f, -halfAngleRange, 0f);
                Quaternion rightLimit = Quaternion.Euler(0f, halfAngleRange, 0f);
                Vector3 leftLimitDirection = leftLimit * follow.forward;
                Vector3 rightLimitDirection = rightLimit * follow.forward;
                Gizmos.DrawRay(follow.position, leftLimitDirection * maxDistance);
                Gizmos.DrawRay(follow.position, rightLimitDirection * maxDistance);

                // Visualizar línea desde la cámara hacia el objetivo
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, follow.position);

                // Visualizar minDistance
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(follow.position, minDistance);

                // Visualizar distancia límite en el eje X
                Gizmos.color = Color.cyan;
                float distanceX = Mathf.Abs(transform.position.x - follow.position.x);
                if (distanceX > xDistanceThreshold)
                {
                    Vector3 targetPositionX = new Vector3(follow.position.x, transform.position.y, transform.position.z);
                    Gizmos.DrawLine(transform.position, targetPositionX);
                }
            }
        }
    }
}

