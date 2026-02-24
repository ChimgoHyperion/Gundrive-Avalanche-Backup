using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public static EnemyScript instance;
    public float health;
    public float startHealth;
    private EnemySpawning enemySpawning;
    EnemyManager enemyManager;

    public GameObject damageEffect;
    public GameObject deatheffect,explosionEffect,explosionRing;
    public Color currentColor;

    public GameObject[] coins;
    buttonSoundHolder soundHolder;
    public float intensity;
    public float time;
    public void TakeDamage(int damage)
    {
        health -= damage ;
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
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.05f);
        gameObject.GetComponent<SpriteRenderer>().color = currentColor;

    }

    // Start is called before the first frame update
    void Start()
    {
        currentColor = gameObject.GetComponent<SpriteRenderer>().color;
        enemyManager = FindObjectOfType<EnemyManager>();
        health = enemyManager.health;
        startHealth = enemyManager.startHealth;
        soundHolder = FindObjectOfType<buttonSoundHolder>();
       
    }


    // Update is called once per frame
    void Update()
    {
       // health = enemyManager.health;
        if (health <= 0)
        {
            // shake camera
            ScreenShake.instance.shakeCamera(intensity, time);
           
            death();
           
        }
        Destroy(gameObject, 60f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BottomDeath")
        {
           
            death();
        }

        if (collision.gameObject.tag == "EnemyDeath")
        {

            death();
        }


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "BottomDeath")
        {
           
            death();
        }
        if (collision.gameObject.tag == "BlackHole")
        {

            death();
        }
        if (collision.gameObject.tag == "EnemyDeath")
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
        enemyManager.ShowEffects();
        Instantiate(coins[0], transform.position, Quaternion.Euler(0, 0, 90));
        Instantiate(coins[1], transform.position, Quaternion.identity);
        Instantiate(coins[2], transform.position, Quaternion.Euler(0, 0, -90));

        Destroy(gameObject);
        Instantiate(deatheffect, transform.position, Quaternion.identity);
    }
}

