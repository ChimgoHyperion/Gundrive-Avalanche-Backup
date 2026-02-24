using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowAndHit : MonoBehaviour
{
    public float speed;
    private Transform target;
    public float minimumDistance;
    Rigidbody2D rb;
    public float force;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Vector2.Distance(transform.position, target.position) > minimumDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            //Vector2 direction = target.position - transform.position;
          //  transform.Translate(direction * speed);
           // rb.AddForce(direction * force * Time.deltaTime, ForceMode2D.Force);


            //Deal Physical damage
        }*/
        
    }
    private void FixedUpdate()
    {
        if(target!=null)
        if (Vector2.Distance(transform.position, target.position) > minimumDistance)
        {
           // transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            Vector2 direction = target.position - transform.position;
            //  transform.Translate(direction * speed);
             rb.AddForce(direction * force * Time.deltaTime, ForceMode2D.Force);
           // rb.MovePosition((Vector2) transform.position+ direction* force * Time.deltaTime);

            //Deal Physical damage
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<MovementandShooting>().TakeDamage(damage);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minimumDistance);
    }
}
