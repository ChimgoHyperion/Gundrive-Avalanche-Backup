using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleDamageSingle : MonoBehaviour
{
    public GameObject explosionEffect;
    public GameObject finishEffect;
    public int damage;
    public float radius;
    public float force;
    public float waitTime;

    public float intensity;
    public float time;
     buttonSoundHolder soundHolder;

    // Start is called before the first frame update
    void Start()
    {
         soundHolder = FindObjectOfType<buttonSoundHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(MakeKinematic());
        StartCoroutine(destroy());

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        /* if (collision.gameObject.CompareTag("World"))
         {

             processCollision();
             Instantiate(RemnantEffect, transform.position, Quaternion.identity);
         }*/
        if (collision.gameObject.tag == "Boss")
        {
            processCollision();
            collision.gameObject.GetComponent<BossHealthScript>().TakeDamage(damage);
        }
        if (collision.gameObject.tag == "Player")
        {
            processCollision();
            collision.gameObject.GetComponent<MovementandShooting>().TakeDamage(damage);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            processCollision();
            collision.gameObject.GetComponent<EnemyScript>().TakeDamage(damage);
        }

        if (collision.gameObject.tag == "Enemy5")
        {
            processCollision();
            collision.gameObject.GetComponent<TakeDamageandDisappear>().TakeDamage(damage);
        }
    }
    void processCollision()
    {

        ScreenShake.instance.shakeCamera(intensity, time);

        //  gameObject.SetActive(false);
    }
    IEnumerator destroy()
    {
        yield return new WaitForSeconds(waitTime);
        Instantiate(finishEffect, transform.position, Quaternion.identity);
        soundHolder.EnemyDeath();
        Destroy(gameObject);
    }
    IEnumerator MakeKinematic()
    {
        yield return new WaitForSeconds(2f);
        GetComponent<Rigidbody2D>().isKinematic = true; // for the blackhole gun
    }
}
