using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool PlayerInRange { get; private set; }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            PlayerInRange = true;
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            PlayerInRange = false;
    }
}