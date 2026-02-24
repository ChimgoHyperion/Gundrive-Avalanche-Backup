using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2BombScript : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    public GameObject impactEfect;
    public float destroyTime;
    public int damage;
    public float radius;
    public float force;
    public LayerMask layertoHit;

    public float intesity, time;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void destroyProjectile()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "World")
        {
            StartCoroutine(wait());
        }

      /*  if (collision.gameObject.tag == "Enemy")
        {
            StartCoroutine(wait());


        }*/
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(wait());

        }
        if (collision.gameObject.tag == "Boss")
        {
            StartCoroutine(wait());

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BottomDeath")
        {
            StartCoroutine(wait());


        }
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
          //  EnemyScript enem = obj.GetComponent<EnemyScript>();

         //   TakeDamageandDisappear enem5 = obj.GetComponent<TakeDamageandDisappear>();
            MovementandShooting player = obj.GetComponent<MovementandShooting>();
            BossHealthScript boss = obj.GetComponent<BossHealthScript>();

          /*  if (enem != null)
            {
                enem.TakeDamage(damage);
            }
            if (enem5 != null)
            {
                enem5.TakeDamage(damage);
            }*/
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }
            if (player != null)
            {
                player.TakeDamage(damage);
            }

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(destroyTime);
        Instantiate(impactEfect, transform.position, Quaternion.identity);
        ScreenShake.instance.shakeCamera(intesity, time);
        Explode();
        //  gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
