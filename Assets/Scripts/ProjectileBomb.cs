using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBomb : MonoBehaviour
{
    public GameObject explosionEffect;
    public GameObject RemnantEffect;
    public int damage;
    public float radius;
    public float force;
    public LayerMask layertoHit;

    public float intensity;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("World"))
        {

            processCollision();
            Instantiate(RemnantEffect, transform.position, Quaternion.identity);
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
        if (collision.gameObject.tag == "Boss")
        {
            processCollision();
            collision.gameObject.GetComponent<BossHealthScript>().TakeDamage(damage);
        }
        if (collision.gameObject.tag == "Enemy5")
        {
            processCollision();
            collision.gameObject.GetComponent<TakeDamageandDisappear>().TakeDamage(damage);
        }
        if (collision.gameObject.tag == "ConsPow")
        {
            processCollision();

        }
        if (collision.gameObject.tag == "PowerUp")
        {
            processCollision();

        }
        // for testing combat game
        if (collision.gameObject.tag == "Ground")
        {
            processCollision();

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BottomDeath"))
        {
            processCollision();
            Instantiate(RemnantEffect, transform.position, Quaternion.identity);
        }
    }
    void processCollision()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        ScreenShake.instance.shakeCamera(intensity, time);
        Explode();
        gameObject.SetActive(false);
    }
    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, layertoHit);

        foreach (Collider2D obj in colliders)
        {
            Vector2 direction = obj.transform.position - transform.position;
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(direction * force);


            }
            EnemyScript enem = obj.GetComponent<EnemyScript>();
            if (enem != null)
            {
                enem.TakeDamage(damage);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    IEnumerator spawnFire()
    {
        
        yield return new WaitForSeconds(2f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
