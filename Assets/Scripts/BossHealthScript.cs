using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthScript : MonoBehaviour
{
   
    public float health;
    [SerializeField]float startHealth;
   
    EnemyManager enemyManager;
    BossSpawnerScript bossSpawner;

    public GameObject damageEffect;
    public GameObject deatheffect;
    public GameObject explosionEffect, explosionRing;
    public float intensity;
    public float time;

    public GameObject[] coins;
    public GameObject coinContainer;
    buttonSoundHolder soundHolder;

    public SpriteRenderer SR;
    Color currentcolor;

    public Slider healthBar;
 
    public void TakeDamage(int damage)
    {
        health -= damage;
        StartCoroutine(shineWhite());
        Instantiate(damageEffect, transform.position, Quaternion.identity);
    }
    public void slowlyDamage(float damage)
    {
        health -= damage * Time.deltaTime;
        Instantiate(damageEffect, transform.position, Quaternion.identity);
    }
    IEnumerator shineWhite()
    {
        SR.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        SR.color = currentcolor;

    }

    // Start is called before the first frame update
    void Start()
    {
        currentcolor = SR.GetComponent<SpriteRenderer>().color;
       
        bossSpawner = FindObjectOfType<BossSpawnerScript>();
        health = startHealth;
     
        soundHolder = FindObjectOfType<buttonSoundHolder>();
        healthBar.maxValue = health;
    }


    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            // shake camera
            ScreenShake.instance.shakeCamera(intensity, time);
            bossSpawner.ShowEffects();
           
            death();
            bossSpawner.startWaitTime(); // look into this tomorrow


        }
        healthBar.value = health;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BottomDeath")
        {
            death();
        }
    }
    public void death()
    {
        soundHolder.EnemyDeath();
        ScoreManager.instance.AddPoints();
        Instantiate(explosionRing, transform.position, Quaternion.identity);
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Instantiate(coinContainer, transform.position, Quaternion.identity);
      /*  Instantiate(coins[0], transform.position, Quaternion.Euler(0, 0, 90));
        Instantiate(coins[1], transform.position, Quaternion.identity);
        Instantiate(coins[2], transform.position, Quaternion.Euler(0, 0, -90));
        Instantiate(coins[3], transform.position, Quaternion.Euler(0, 0, 90));
        Instantiate(coins[4], transform.position, Quaternion.Euler(0, 0, -90));
        Instantiate(coins[5], transform.position, Quaternion.Euler(0, 0, 90));
        Instantiate(coins[6], transform.position, Quaternion.Euler(0, 0, -90));
        Instantiate(coins[7], transform.position, Quaternion.Euler(0, 0, -90));
        Instantiate(coins[8], transform.position, Quaternion.Euler(0, 0, 90));
        Instantiate(coins[9], transform.position, Quaternion.Euler(0, 0, -90)); */

        Destroy(gameObject);
        Instantiate(deatheffect, transform.position, Quaternion.identity);
    }
}
