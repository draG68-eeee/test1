using UnityEngine;
using System.Collections;
public class Health : MonoBehaviour
{
    public RectTransform healthBar;
    public float hp = 100;
    public bool isAlive = true;
    private PlayerCheckpoint playerCheckpoint;



    void Start()
    {
        playerCheckpoint = GetComponent<PlayerCheckpoint>();

        // Optional: initialize scale
        UpdateHealthBar();
        StartCoroutine(Regenerate());

    }


    private IEnumerator Regenerate(){
        while (isAlive){
            yield return new WaitForSeconds(1);
            if (hp < 100){
                
                ChangeHp(-1);
            }
        }
    }
    public void Respawn(){

    }
   
    public void ChangeHp(float input)
    {
        hp -= input;
        hp = Mathf.Clamp(hp, 0, 100); // Keeps hp between 0 and 100
        if (hp == 0){
            isAlive = false;
            hp = 0;
            StartCoroutine(WaitUntilRespawn());
        }
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float newHealthBarValue = hp / 100f;
        healthBar.localScale = new Vector3(newHealthBarValue, 1, 1);
    }

    private IEnumerator WaitUntilRespawn(){
        yield return new WaitForSeconds(5);
        playerCheckpoint.Respawn();
        hp = 100;
        isAlive = true;
    }
}
