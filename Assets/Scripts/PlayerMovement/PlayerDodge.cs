using UnityEngine;

/// <summary>
/// PlayerDodge allows the player to dodge in 8 directions using WASD + diagonals when the right mouse button is pressed.
/// Inspired by dodge mechanics in Bloodborne and Lies of P.
/// Attach this script to your player GameObject. Requires a Rigidbody.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerDodge : MonoBehaviour
{
    [Header("Dodge Settings")]
    [Tooltip("Distance the player dodges per dodge action.")]
    public float dodgeDistance = 5f;
    [Tooltip("Time in seconds to complete the dodge.")]
    public float dodgeDuration = 0.2f;
    [Tooltip("Cooldown time between dodges (seconds)")]
    public float dodgeCooldown = 0.5f;

    private Rigidbody rb;
    [Tooltip("Camera used to determine dodge direction. If null, will use Camera.main.")]
    public Camera playerCamera;
    private bool isDodging = false;
    private float lastDodgeTime = -Mathf.Infinity;
    private Vector3 dodgeDirection;
    private float dodgeStartTime;
    private Vector3 dodgeStartPosition;
    private Vector3 dodgeTargetPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    void Update()
    {
        if (!isDodging && Time.time >= lastDodgeTime + dodgeCooldown)
        {
            Vector3 inputDir = GetInputDirection();
            if (inputDir != Vector3.zero && Input.GetMouseButtonDown(1)) // Right mouse button
            {
                StartDodge(inputDir);
            }
        }
    }

    void FixedUpdate()
    {
        if (isDodging)
        {
            float t = (Time.time - dodgeStartTime) / dodgeDuration;
            Vector3 nextTarget;
            if (t < 1f)
            {
                nextTarget = Vector3.Lerp(dodgeStartPosition, dodgeTargetPosition, t);
            }
            else
            {
                nextTarget = dodgeTargetPosition;
            }

            // Compute movement delta for this frame
            Vector3 moveDelta = nextTarget - rb.position;
            float moveDist = moveDelta.magnitude;
            Vector3 moveDir = moveDelta.normalized;

            // SweepTest to check for obstacles along the move direction
            RaycastHit hit;
            bool hitObstacle = false;
            float allowedDist = moveDist;
            if (moveDist > 0.001f)
            {
                // Use the Rigidbody's collider for sweep test
                if (rb.SweepTest(moveDir, out hit, moveDist))
                {
                    // Stop at the hit point, minus a small offset to avoid overlap
                    allowedDist = hit.distance - 0.01f;
                    if (allowedDist < 0f) allowedDist = 0f;
                    hitObstacle = true;
                }
            }
            Vector3 newPosition = rb.position + moveDir * allowedDist;
            rb.MovePosition(newPosition);

            // End dodge if reached target or hit obstacle
            if (t >= 1f || hitObstacle)
            {
                isDodging = false;
                lastDodgeTime = Time.time;
            }
        }
    }

    /// <summary>
    /// Gets the normalized direction vector based on WASD input, relative to the camera's facing (XZ only).
    /// </summary>
    private Vector3 GetInputDirection()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        float v = Input.GetAxisRaw("Vertical");   // W/S or Up/Down
        Vector3 input = new Vector3(h, 0, v);
        if (input.sqrMagnitude < 0.01f) return Vector3.zero;
        if (input.sqrMagnitude > 1f) input.Normalize(); // Allow for diagonal movement

        // Use camera's orientation for movement direction (XZ only)
        if (playerCamera == null)
            playerCamera = Camera.main;
        Vector3 camForward = playerCamera.transform.forward;
        Vector3 camRight = playerCamera.transform.right;
        // Flatten vectors to XZ plane
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();
        Vector3 moveDir = camForward * input.z + camRight * input.x;
        if (moveDir.sqrMagnitude > 1f) moveDir.Normalize();
        return moveDir;
    }

    /// <summary>
    /// Initiates the dodge in the given direction.
    /// </summary>
    private void StartDodge(Vector3 direction)
    {
        isDodging = true;
        dodgeStartTime = Time.time;
        dodgeStartPosition = rb.position;
        dodgeDirection = direction;
        dodgeTargetPosition = dodgeStartPosition + dodgeDirection * dodgeDistance;
    }
}
