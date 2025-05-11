using UnityEngine;
using System.Collections;
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
        StartCoroutine(Kill());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator Kill(){
        yield return new WaitForSeconds(5);
        health.ChangeHp(100);
    }
}
