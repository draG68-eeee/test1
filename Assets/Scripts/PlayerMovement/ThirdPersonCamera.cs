using UnityEngine;

/// <summary>
/// ThirdPersonCamera follows a target (usually the player), rotates based on mouse input, and prevents camera clipping through objects.
/// Attach this script to your Camera GameObject.
/// </summary>
public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;       // Usually the player
    public Transform cameraPivot;  // Empty GameObject to rotate the camera around
    public float mouseSensitivity = 3f;
    public float distanceFromTarget = 15f;
    public float verticalRotationLimit = 80f;
    public float cameraCollisionRadius = 0.3f; // Radius for sphere cast
    public LayerMask collisionMask = ~0; // Layers to collide with (default: everything)
    public Vector3 cameraDirection;

    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Hide and lock cursor
    }

    void LateUpdate()
    {
        // Mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -verticalRotationLimit, verticalRotationLimit);

        // Rotate pivot
        cameraPivot.rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Desired camera position behind the pivot
        Vector3 desiredOffset = -cameraPivot.forward * distanceFromTarget;
        Vector3 desiredPosition = cameraPivot.position + desiredOffset;

        // SphereCast from pivot to desired position to prevent clipping
        RaycastHit hit;
        float actualDistance = distanceFromTarget;
        if (Physics.SphereCast(cameraPivot.position, cameraCollisionRadius, desiredOffset.normalized, out hit, distanceFromTarget, collisionMask, QueryTriggerInteraction.Ignore))
        {
            actualDistance = Mathf.Max(hit.distance - cameraCollisionRadius, 0.5f); // Keep at least 0.5 units from pivot
        }
        Vector3 finalPosition = cameraPivot.position + (-cameraPivot.forward * actualDistance);
        transform.position = finalPosition;

        // Look at the player
        transform.LookAt(cameraPivot.position + new Vector3(0,1,0));
        cameraDirection = transform.forward;
    }
}
