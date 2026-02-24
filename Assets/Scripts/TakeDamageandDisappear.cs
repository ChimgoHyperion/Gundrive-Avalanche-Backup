using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageandDisappear : MonoBehaviour
{
    public float health;
    public float startHealth;
    public float waittime;
    public  Transform newPosition;
     EnemyManager enemyManager;

    public GameObject deatheffect,explosionEffect, explosionRing;
    public GameObject damageEffect;
    public Color currentColor;
    public SpriteRenderer mainBody;

    public GameObject[] coins;
    buttonSoundHolder soundHolder;

    public float intensity;
    public float time;


    // Start is called before the first frame update
    void Start()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
        newPosition  = GameObject.FindGameObjectWithTag("NewPosition").transform;
        currentColor = mainBody.color;
        health = enemyManager.enemy5health;
        startHealth = enemyManager.enemy5starthealth;
        soundHolder = FindObjectOfType<buttonSoundHolder>();

    }
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
        mainBody.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        mainBody.color = currentColor;

    }
    // Update is called once per frame
    void Update()
    {
      //  health = enemyManager.enemy5health;
        if (health == 2)
        {
            StartCoroutine(Teleport());

        }
        if (health <= 0)
        {
            // shake camera
            ScreenShake.instance.shakeCamera(intensity, time);
            death();
           
        }
        Destroy(gameObject, 30f);
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
    IEnumerator Teleport()
    {
        mainBody.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(waittime);
        transform.position = newPosition.position;
        yield return new WaitForSeconds(0.5f);
        mainBody.GetComponent<SpriteRenderer>().enabled = true;

    }
   
}
