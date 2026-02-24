using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBulletScript : MonoBehaviour
{
   
    public GameObject HitEffect;
    public int damage;
   

   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("World"))
        {
            Destroy(gameObject);
            Instantiate(HitEffect, transform.position, Quaternion.identity);
           
        }
        if (collision.gameObject.CompareTag("PowerUp"))
        {
            Destroy(gameObject);
            Instantiate(HitEffect, transform.position, Quaternion.identity);

        }
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            Instantiate(HitEffect, transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<EnemyScript>().TakeDamage(damage);

        }
        if (collision.gameObject.tag == "Boss")
        {
            Destroy(gameObject);
            Instantiate(HitEffect, transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<BossHealthScript>().TakeDamage(damage);

        }
        if (collision.gameObject.tag == "Enemy5")
        {
            Destroy(gameObject);
            Instantiate(HitEffect, transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<TakeDamageandDisappear>().TakeDamage(damage);
        }
        if (collision.gameObject.tag == "ConsPow")
        {
            Destroy(gameObject);
            Instantiate(HitEffect, transform.position, Quaternion.identity);

        }
        if (collision.gameObject.tag == "PowerUp")
        {
            Destroy(gameObject);
            Instantiate(HitEffect, transform.position, Quaternion.identity);

        }
    }
   

    // Update is called once per frame
    void Update()
    {

        Destroy(gameObject, 15f);
    }
}
