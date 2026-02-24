using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public float speed ;
    Transform player;
    public int damage;// should be derived from a game manager script along with other projectiles for scalabillity(increase in difficulty)
    public float lifeTime;
    public GameObject destroyEffect;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            if (transform.position == player.position)
            {

                Destroy(gameObject);
            }
        }
       
       
       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<MovementandShooting>().TakeDamage(damage);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "World")
        {
           // Instantiate(destroyEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
