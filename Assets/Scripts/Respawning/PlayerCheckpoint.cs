using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    public Vector3 respawnPosition;
    private Checkpoint currentNearbyCheckpoint;

    void Start()
    {
        respawnPosition = transform.position; // Initial spawn
    }

    void Update()
    {
        if (currentNearbyCheckpoint != null && currentNearbyCheckpoint.PlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("i slep");
            respawnPosition = currentNearbyCheckpoint.transform.position;
            // Optionally: play animation, sound, or UI feedback
        }
    }

    public void Respawn()
    {
        transform.position = respawnPosition + new Vector3(0,5,0);
        // Optionally: reset velocity, play animation, etc.
    }

    private void OnCollisionEnter(Collision other)
    {
        var checkpoint = other.gameObject.GetComponent<Checkpoint>();
        if (checkpoint != null)
            currentNearbyCheckpoint = checkpoint;
    }

    private void OnCollisionExit(Collision other)
    {
        var checkpoint = other.gameObject.GetComponent<Checkpoint>();
        if (checkpoint != null && currentNearbyCheckpoint == checkpoint)
            currentNearbyCheckpoint = null;
    }
}