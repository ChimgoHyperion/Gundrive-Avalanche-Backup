using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particlrburn : MonoBehaviour
{
    public int damage;

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<MovementandShooting>().TakeDamage(damage);
        }
       


    }
}
