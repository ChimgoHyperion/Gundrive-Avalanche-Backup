using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freezeEnemy : MonoBehaviour
{
    public GameObject iceBlock;
    public GameObject iceBreakEffect;
    public float waitTime;
    FollowEnemyScript script;
    Rigidbody2D rb;
    public GameObject dustEffect;
    EnemyManager enem;
    float enemtimebtwshots;
    public AudioClip hitRock;
    public AudioClip IceThaw;
    public AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        script = GetComponent<FollowEnemyScript>();
        rb = GetComponent<Rigidbody2D>();
        enem = FindObjectOfType<EnemyManager>();
        enemtimebtwshots = enem.timebtwshots;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "World")
        {
          //  Instantiate(dustEffect, transform.position, Quaternion.identity);
          //  source.PlayOneShot(hitRock);
        }
        if (collision.gameObject.tag == "Enemy")
        {
          //  Instantiate(dustEffect, transform.position, Quaternion.identity);
        }
        if (collision.gameObject.tag == "Enemy5")
        {
          //  Instantiate(dustEffect, transform.position, Quaternion.identity);
        }
        if (collision.gameObject.tag == "Player")
        {
          //  Instantiate(dustEffect, transform.position, Quaternion.identity);
        }
    }
    IEnumerator Unfreeze()
    {
        yield return new WaitForSeconds(waitTime);
        rb.gravityScale = 0;
      //  enem.timebtwshots = enemtimebtwshots;
       // script.enabled = true;
        Instantiate(iceBreakEffect, transform.position, Quaternion.identity);
        source.PlayOneShot(IceThaw);
        iceBlock.SetActive(false);

    }
    public void Freeze()
    {
        iceBlock.SetActive(true);
      //  enem.timebtwshots = 0;
        rb.gravityScale = 1000;
       // script.enabled = false;
        StartCoroutine(Unfreeze());
    }
}
