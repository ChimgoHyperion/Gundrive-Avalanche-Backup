using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1ControllerScript : MonoBehaviour
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
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target!= null)
        StartCoroutine(wait());
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
   
}
