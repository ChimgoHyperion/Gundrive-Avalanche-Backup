using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricDetector : MonoBehaviour
{
   
    public int damage;
    public int bossDamage;
    public int shooterId;

    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyScript>().slowlyDamage(damage);

        }
        if (collision.tag == "Boss")
        {
            collision.gameObject.GetComponent<BossHealthScript>().slowlyDamage(bossDamage);

        }
        if (collision.tag == "Enemy5")
        {
            collision.gameObject.GetComponent<TakeDamageandDisappear>().slowlyDamage(damage);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyScript>().TakeDamage(damage);

        }
        if (collision.tag == "Enemy5")
        {
            collision.gameObject.GetComponent<TakeDamageandDisappear>().TakeDamage(damage);
        }
       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
       
    }
    // Update is called once per frame
    void Update()
    {

    }
}
