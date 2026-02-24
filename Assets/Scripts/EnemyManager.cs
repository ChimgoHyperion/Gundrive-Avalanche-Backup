using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
   public float health;
    public float enemy5health;
    public float enemy5starthealth = 7f;
   public  float startHealth = 4f;
    public float healthIncrement;
    public float timebtwshots;
    public float shotIncrement;

    public GameObject[] TextEffects;
    public int rand;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
   
    public void ReturnHealth()
    {
        health = startHealth;
       
    }
    public void IncreaseHealth()
    {
        health +=  Mathf.Sqrt(Time.deltaTime / healthIncrement);

    }
    public void IncreaseShootFreq()
    {
      //  timebtwshots -= Mathf.Sqrt(Time.deltaTime / shotIncrement);
    }
    public void Return5Heealth()
    {
        enemy5health = enemy5starthealth;
    }
    public void Increase5Health()
    {
        enemy5health += Mathf.Sqrt(Time.deltaTime / healthIncrement);
    }
    public void ShowEffects()
    {
        rand= Random.Range(0, 4);
        switch (rand)
        {
            case 0:
                TextEffects[0].SetActive(true);
                TextEffects[0].GetComponent<DisableText>().SelectColor();

                /*   StartCoroutine(ActivateandDeactivate());
                   TextEffects[0].SetActive(false); */
                break;
            case 1:
                TextEffects[1].SetActive(true);
                TextEffects[1].GetComponent<DisableText>().SelectColor();
                break;
            case 2:
                Debug.Log("Nothing");
                break;
            case 3:
                TextEffects[3].SetActive(true);
                TextEffects[3].GetComponent<DisableText>().SelectColor();
                break;
            case 4:
              
                TextEffects[2].SetActive(true);
                TextEffects[2].GetComponent<DisableText>().SelectColor();
                break;


        }
            
    }
   
}
