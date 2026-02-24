using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlameFreeze : MonoBehaviour
{
    public GameObject remnantObject;
    public int damage;
    public int damageForBoss;
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
            collision.gameObject.GetComponent<BossHealthScript>().slowlyDamage(damageForBoss);

        }
        if (collision.tag == "Enemy5")
        {
            collision.gameObject.GetComponent<TakeDamageandDisappear>().slowlyDamage(damage);
        }
        if (collision.gameObject.tag == "Player")
        {
           
          //  collision.gameObject.GetComponent<MultiplayerMoveAndShoot>().SlowLyDamage(damage);
           
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "World")
        {
            Instantiate(remnantObject, transform.position, Quaternion.identity); // note this script is instatioating for both single and multiPlayer 
          
        }
        if (collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyScript>().TakeDamage(damage);

        }
        if (collision.tag == "Enemy5")
        {
            collision.gameObject.GetComponent<TakeDamageandDisappear>().TakeDamage(damage);
        }
        if (collision.gameObject.tag == "Player")
        {
           
          //  collision.gameObject.GetComponent<MultiplayerMoveAndShoot>().TakeDamage_RPC(damage);
           
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
          
           // collision.gameObject.GetComponent<MultiplayerMoveAndShoot>().TakeDamage_RPC(damage);

        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
          
           // collision.gameObject.GetComponent<MultiplayerMoveAndShoot>().SlowLyDamage(damage);

        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
