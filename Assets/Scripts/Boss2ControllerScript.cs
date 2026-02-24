using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2ControllerScript : MonoBehaviour
{
    public float Rotspeed;
    public GameObject energyBalls;
    public float energyBallForce;
    public Transform shootingTip;
    Transform target;
    Rigidbody2D rb;
    public float timebtwShots;
    private float nextShotTime;
    AudioSource source;
    public AudioClip clip;
     BossHealthScript healthscript;
    public Color currentColor;
    public SpriteRenderer mainBody;
    public float waittime;

   

    Transform point1, point2, point3;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        healthscript = GetComponent<BossHealthScript>();

        point1 = GameObject.Find("point1").transform;
        point2 = GameObject.Find("point2").transform;
        point3 = GameObject.Find("point3").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
            StartCoroutine(wait());

        if (healthscript.health == 150)
        {
            StartCoroutine(Teleport());

        }else if (healthscript.health == 100)
        {
            StartCoroutine(Teleport());

        }else if(healthscript.health == 50){

            StartCoroutine(Teleport());
        }
    }
    IEnumerator wait()
    {
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        //  direction.Normalize();
        yield return new WaitForSeconds(2f);
        fireAtPlayer();
    }
    public void fireAtPlayer()
    {
        if (Time.time > nextShotTime)
        {
            if (target != null)
            {
                GameObject bulletInstance = Instantiate(energyBalls, shootingTip.position, shootingTip.rotation);
                Vector2 direction = target.position - transform.position;

                bulletInstance.GetComponent<Rigidbody2D>().AddForce(direction * energyBallForce, ForceMode2D.Impulse);
                source.PlayOneShot(clip);


                nextShotTime = Time.time + timebtwShots;
            }


        }


    }
    IEnumerator Teleport()
    {
        mainBody.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(waittime);

        int index = Random.Range(1, 3);
        if(index== 1)
        {
            transform.position = point1.position;
        }
        if (index == 2)
        {
            transform.position = point2.position;
        }
        if (index == 3)
        {
            transform.position = point3.position;
        }
        
        yield return new WaitForSeconds(0.5f);
        mainBody.GetComponent<SpriteRenderer>().enabled = true;

    }
}
