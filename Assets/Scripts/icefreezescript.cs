using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class icefreezescript : MonoBehaviour
{
    public int damage;

    private void OnParticleCollision(GameObject collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<freezeEnemy>().Freeze();


        }
        if (collision.tag == "Enemy5")
        {
            collision.gameObject.GetComponent<freezeEnemy>().Freeze();
        }
    }
}
