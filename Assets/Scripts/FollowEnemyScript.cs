using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemyScript : MonoBehaviour
{
    public float speed;
   [SerializeField]  Transform target;
    [SerializeField] float targetOffset;
    public float minimumDistance;
    public GameObject projectile;
    public float timebtwShots;
    private float nextShotTime;
     float force;
    [SerializeField] float origForce;
 
    public Transform shootingTip;
    Rigidbody2D rb;
    private Vector2 velocity = Vector2.zero;
    public float projectileForce;
    EnemyManager enemyManager;

    AudioSource source;
    public AudioClip clip;
    freezeEnemy freezeEnem;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        enemyManager = FindObjectOfType<EnemyManager>();
        timebtwShots = enemyManager.timebtwshots;
        source = GetComponent<AudioSource>();
        freezeEnem = GetComponent<freezeEnemy>();
    }
 
    // Update is called once per frame
    void Update()
    {
        timebtwShots = enemyManager.timebtwshots;
        /* if(target!=null)
         if(Vector2.Distance(transform.position,target.position)> minimumDistance)
         {
           //  transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
             transform.position = Vector2.SmoothDamp(transform.position, target.position, ref velocity, speed);

         } */

      //  Invoke("targetOffsetChange", 5f);

    }
    private void FixedUpdate()
    {

        if (freezeEnem.iceBlock.activeInHierarchy == true)
        {
            force = 0;
            StopAllCoroutines();
        }
        else
        {
            force = origForce;
            if (target != null)
            {
                StartCoroutine(wait());
            }
        }
       
    }
    IEnumerator wait()
    {
        if (Vector2.Distance(transform.position, target.position) > minimumDistance)
        {
           
            Vector2 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
          //  shootingTip.Rotate
          //  direction.Normalize();
          //   transform.Translate(direction * speed);
           //  rb.AddForce(direction * force * Time.deltaTime, ForceMode2D.Impulse);
          //  rb.MovePosition((Vector2)transform.position + direction * force * Time.deltaTime);
             rb.velocity = direction * force * Time.deltaTime;
            yield return new WaitForSeconds(4f);
            fireAtPlayer();
        }
    }
    public void fireAtPlayer()
    {
        if (Time.time > nextShotTime)
        {
            if (target != null)
            {
                anim.SetTrigger("Squish");

                GameObject bulletInstance = Instantiate(projectile, shootingTip.position, shootingTip.rotation);
              
                Vector2 direction = target.position - transform.position;
                //   bulletInstance.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
                bulletInstance.GetComponent<Rigidbody2D>().AddForce( direction * projectileForce,ForceMode2D.Impulse);
                source.PlayOneShot(clip);
              //  bulletInstance.GetComponent<Rigidbody2D>().MovePosition((Vector2) shootingTip.position + direction * projectileForce);

                nextShotTime = Time.time + timebtwShots;
            }
           
            
        }

       
    }
    void targetOffsetChange()
    {
         targetOffset = Random.Range(0, 3);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minimumDistance);
    }
}
