using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverEnemyScript : MonoBehaviour
{
    public float speed;
     GameObject topOfPlayer;
    public float minimumDistance;

    public GameObject projectile;
    public float timebtwShots;
    private float nextShotTime;

    public Transform shootingTip;
    Rigidbody2D rb;
    public float force;
    EnemyManager enemyManager;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        topOfPlayer = GameObject.FindGameObjectWithTag("PlayerTop");
        speed = Random.Range(10, 20);
        rb = GetComponent<Rigidbody2D>();
        enemyManager = FindObjectOfType<EnemyManager>();
        timebtwShots = enemyManager.timebtwshots;
    }

    // Update is called once per frame
    void Update()
    {

        timebtwShots = enemyManager.timebtwshots;
        /* if (Vector2.Distance(transform.position, topOfPlayer.transform.position) < minimumDistance)
         {
             transform.position = Vector2.MoveTowards(transform.position, topOfPlayer.transform.position, speed * Time.deltaTime);

         }*/
        fireAtPlayer();

    }
    private void FixedUpdate()
    {
        if (topOfPlayer != null)
            if (Vector2.Distance(transform.position, topOfPlayer.transform.position) < minimumDistance)
            {
                // transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                Vector2 direction = topOfPlayer.transform.position - transform.position;
                //  transform.Translate(direction * speed);
                rb.AddForce(direction * force * Time.deltaTime, ForceMode2D.Force);
                //   rb.MovePosition((Vector2)transform.position + direction * force * Time.deltaTime);
                //  rb.velocity = direction * force;
                //Deal Physical damage
            }
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(4);
       
        transform.position = Vector2.Lerp(this.transform.position, topOfPlayer.transform.position, speed * Time.deltaTime);
    }
    public void fireAtPlayer()
    {
        if (Time.time > nextShotTime)
        {
            anim.SetTrigger("Squish");
            GameObject bulletInstance = Instantiate(projectile, shootingTip.position, shootingTip.rotation);
            Vector2 direction = topOfPlayer.transform.position - transform.position;
            bulletInstance.GetComponent<Rigidbody2D>().AddForce(Vector2.down * force);
           // bulletInstance.GetComponent<Rigidbody2D>().velocity = direction * force;

            nextShotTime = Time.time + timebtwShots;

        }


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minimumDistance);
    }
}
