using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4KillScript : MonoBehaviour
{
    public int Enemydamage;
    public int PlayerDamage;
    

    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyScript>().slowlyDamage(Enemydamage);

        }
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<MovementandShooting>().TakeDamage(PlayerDamage);

        }
        if (collision.tag == "Enemy5")
        {
            collision.gameObject.GetComponent<TakeDamageandDisappear>().slowlyDamage(Enemydamage);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyScript>().TakeDamage(Enemydamage);

        }
        if (collision.tag == "Enemy5")
        {
            collision.gameObject.GetComponent<TakeDamageandDisappear>().TakeDamage(Enemydamage);
        }
       
    }
}
