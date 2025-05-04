using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float heightJump = 300f;
    public ThirdPersonCamera cameraMovement;
    private Rigidbody rb;
    private bool onGround = true;
    private int contacts = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        MovePlayer();
        CheckJump();
        Debug.Log(contacts);
        Debug.Log("E");
    }

    void OnCollisionEnter(Collision other){
        if (other.gameObject.tag == "Ground"){
            contacts += 1;
            onGround = true;
        }
    }

    void OnCollisionExit(Collision other){
        contacts -= 1;
        if (contacts == 0){
            onGround = false;
        }
        
    }
    void CheckJump(){
        if (Input.GetKeyDown(KeyCode.Space) && onGround){
            rb.AddForce(transform.up * heightJump, ForceMode.Impulse);
        }
    }
    void MovePlayer(){
        // Get camera forward and right directions, flattened (no vertical tilt)
        Vector3 camForward = cameraMovement.cameraDirection;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = Quaternion.Euler(0, 90, 0) * camForward;

        // Get input
        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.W)) vertical += 1f;
        if (Input.GetKey(KeyCode.S)) vertical -= 1f;
        if (Input.GetKey(KeyCode.D)) horizontal += 1f;
        if (Input.GetKey(KeyCode.A)) horizontal -= 1f;

        // Combine input direction
        Vector3 moveDirection = camForward * vertical + camRight * horizontal;

        // Apply movement if there is input
        Vector3 newVelocity = rb.linearVelocity;
        if (moveDirection.magnitude > 0.1f)
        {
            moveDirection.Normalize();
            Vector3 targetVelocity = moveDirection * speed;
            newVelocity.x = targetVelocity.x;
            newVelocity.z = targetVelocity.z;
        }
        else
        {
            // No input, no horizontal movement
            newVelocity.x = 0f;
            newVelocity.z = 0f;
        }

        // Keep vertical physics (gravity, jumping, etc.)
        rb.linearVelocity = newVelocity;
        // transform.LookAt(new Vector3(newVelocity.x, 0, newVelocity.z));
    }
}
