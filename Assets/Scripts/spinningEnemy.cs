using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinningEnemy : MonoBehaviour
{
    public float force;
    Transform target;
    public float minimumDistance;
   // public GameObject projectile;
   // public float timebtwShots;
   // private float nextShotTime;
   // public float force;

    public Transform shootingTip;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {

        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Vector2.Distance(transform.position, target.position) > minimumDistance)
        {
            transform.position = Vector3.Lerp(transform.position, target.position,  Time.deltaTime);
            //Vector2 direction = target.position - transform.position;
            // transform.Translate(direction * speed);
           // rb.AddForce(direction * force * Time.fixedDeltaTime, ForceMode2D.Force);
        }*/
    }
    private void FixedUpdate()
    {
        if (target != null)
            if (Vector2.Distance(transform.position, target.position) > minimumDistance)
            {
                // transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                Vector2 direction = target.position - transform.position;
                //  transform.Translate(direction * speed);
                rb.AddForce(direction * force * Time.deltaTime, ForceMode2D.Force);
                //   rb.MovePosition((Vector2)transform.position + direction * force * Time.deltaTime);
                //  rb.velocity = direction * force;
                //Deal Physical damage
            }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minimumDistance);
    }
}
