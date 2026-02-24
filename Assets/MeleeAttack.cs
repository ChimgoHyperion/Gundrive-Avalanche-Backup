using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public int Damage;
    public PlayerHealthScript playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit: " + collision.gameObject.name);
        if (collision.gameObject.name== "Player Hurt Box")
        {
            collision.gameObject.GetComponent<PlayerHealthScript>().TakeDamage(Damage);
            playerHealth.RageAmount += 15;
           
        }
        if (collision.gameObject.name == "Enemy Hurt Box")
        {

            playerHealth.RageAmount += 15;
            collision.gameObject.GetComponent<EnemyHealthScript>().TakeDamage(Damage);
        }
    }
}
