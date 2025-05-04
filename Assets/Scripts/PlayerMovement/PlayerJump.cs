using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody rb;
    private float heightJump = 300f;

    public bool onGround = true;
    public GameObject feet;
    public LayerMask groundLayer; // Only include GROUND in this layer!
    public float groundCheckDistance = 0.2f;
    private float jumpCooldown = 0.2f; // Small time to "ignore" raycast after jump
    private float timeSinceJump = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        timeSinceJump += Time.deltaTime;
        CheckIfGrounded();
        CheckJump();
    }

    void CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            rb.AddForce(Vector3.up * heightJump, ForceMode.Impulse);
            onGround = false;         // Force off-ground status
            timeSinceJump = 0f;       // Reset timer
        }
    }

    void CheckIfGrounded()
    {
        if (timeSinceJump < jumpCooldown)
        {
            // Skip checking for a short time after jumping
            return;
        }

        Ray ray = new Ray(feet.transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, groundCheckDistance, groundLayer, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.gameObject != this.gameObject)
            {
                onGround = true;
                Debug.DrawRay(feet.transform.position, Vector3.down * groundCheckDistance, Color.green);
                return;
            }
        }

        onGround = false;
        Debug.DrawRay(feet.transform.position, Vector3.down * groundCheckDistance, Color.red);
    }

}
