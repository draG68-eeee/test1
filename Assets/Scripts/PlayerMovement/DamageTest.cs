using UnityEngine;

public class DamageTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject player;
    private Health health;
    void Start()
    {
        player = GameObject.Find("Player");
        health = player.GetComponent<Health>();
        health.ChangeHp(50);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
