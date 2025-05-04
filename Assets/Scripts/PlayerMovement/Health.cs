using UnityEngine;
using System.Collections;
public class Health : MonoBehaviour
{
    public RectTransform healthBar;
    public float hp = 100;
    public bool isAlive = true;

    void Start()
    {
        // Optional: initialize scale
        UpdateHealthBar();
        StartCoroutine(Regenerate());

    }


    private IEnumerator Regenerate(){
        while (isAlive){
            yield return new WaitForSeconds(1);s
            if (hp < 100){
                
                ChangeHp(-1);
            }
        }
    }
s
   
    public void ChangeHp(float input)
    {
        hp -= input;
        hp = Mathf.Clamp(hp, 0, 100); // Keeps hp between 0 and 100
        if (hp == 0){
            isAlive = false;
        }
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float newHealthBarValue = hp / 100f;
        healthBar.localScale = new Vector3(newHealthBarValue, 1, 1);
    }
}
