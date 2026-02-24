using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public int damage;
    public GameObject snow;
   // public GameObject ice;
    private const int damagefactor = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<MovementandShooting>().SlowLyDamage(damagefactor);
              Instantiate(snow,transform.position, Quaternion.identity);
           // Instantiate(ice, transform.position, Quaternion.identity);
        }
        if (other.gameObject.CompareTag("World"))
        {
            Instantiate(snow, transform.position, Quaternion.identity);
          //  Instantiate(ice, transform.position, Quaternion.identity);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
      
        if (other.gameObject.tag == "World")
        {
          
        }
    }
}
