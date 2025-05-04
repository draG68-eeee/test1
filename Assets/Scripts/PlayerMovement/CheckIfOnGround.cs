using UnityEngine;

public class CheckIfOnGround : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public PlayerJump playerJump;
    private int groundContacts = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Ground")){
            groundContacts++;
            playerJump.onGround = true;
        }
    }

    void OnCollisionExit(Collision collision){
        if (collision.gameObject.CompareTag("Ground")){
            groundContacts--;
            if (groundContacts <= 0){
                playerJump.onGround = false;
            }
        }
    }

}
