using UnityEngine;

/// <summary>
/// Rotates the player to face the direction of movement (velocity or input direction).
/// Attach this script to the same GameObject as PlayerDodge.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerOrienter : MonoBehaviour
{
    [Tooltip("Speed at which the player rotates to face movement direction.")]
    public float rotationSpeed = 720f; // Degrees per second

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Use only horizontal (XZ) movement for orientation; ignore vertical (Y) movement (e.g., jumping/falling)
        Vector3 moveDir = rb.linearVelocity;
        moveDir.y = 0f;
        // If horizontal velocity is very small, try to get input direction (XZ only)
        if (moveDir.sqrMagnitude < 0.01f)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            moveDir = new Vector3(h, 0, v);
        }
        if (moveDir.sqrMagnitude > 0.01f)
        {
            // Get the desired rotation to face horizontal movement direction
            Quaternion targetRot = Quaternion.LookRotation(moveDir.normalized, Vector3.up);
            // Smoothly interpolate rotation
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));
        }
    }
}
