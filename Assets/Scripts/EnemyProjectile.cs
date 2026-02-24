using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    Vector3 targetPosition;
    public float speed;
    public int damage;
    public float lifeTime;
    Rigidbody2D rb;
    Vector2 moveDirection;
    public GameObject destroyEffect;
    public AudioClip impactSound;
    buttonSoundHolder soundHolder;
    // Start is called before the first frame update
    void Start()
    {
       /* targetPosition = FindObjectOfType<MovementandShooting>().transform.position;
        rb = GetComponent<Rigidbody2D>();
        moveDirection = (targetPosition - transform.position).normalized * speed;
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
        Destroy(gameObject, 3f);*/

    }

    // Update is called once per frame
    void Update()
    {

      
        /* lifeTime -= Time.deltaTime;
          if (lifeTime <= 0)
          {
              Destroy(gameObject);
          }*/

    }
    private void FixedUpdate()
    {
       // transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        //Vector2 direction = targetPosition - transform.position;
       // rb.velocity = direction * speed;
       // if (transform.position == targetPosition)
      //  {
       //     Destroy(gameObject);
       // }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "World")
        {
          // Instantiate(destroyEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Enemy5")
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Player")
        {
           Instantiate(destroyEffect, transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<MovementandShooting>().TakeDamage(damage);
         //   soundHolder.
            Destroy(gameObject);
        }
    }
  
}
