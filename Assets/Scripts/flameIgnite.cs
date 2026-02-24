using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class flameIgnite : MonoBehaviour
{
    public GameObject remnantObject;
    public int damage;
    public int shooterId;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "World")
        {
            Instantiate(remnantObject, transform.position, Quaternion.identity);
            PhotonNetwork.Instantiate(remnantObject.name, transform.position, Quaternion.identity);
        }
        if (collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<freezeEnemy>().Freeze();

            collision.gameObject.GetComponent<EnemyScript>().TakeDamage(damage);


        }
       /* if(collision.gameObject.name=="Enemy 2")
        {
            collision.gameObject.GetComponent<FollowEnemyScript>().speed = 0;
        }*/
        if (collision.tag == "Boss")
        {
          //  collision.gameObject.GetComponent<freezeEnemy>().Freeze();

            collision.gameObject.GetComponent<BossHealthScript>().TakeDamage(damage);


        }
        if (collision.tag == "Enemy5")
        {
            collision.gameObject.GetComponent<freezeEnemy>().Freeze();
            collision.gameObject.GetComponent<TakeDamageandDisappear>().TakeDamage(4);
        }
        if (collision.gameObject.tag == "Player")
        {
          
            collision.gameObject.GetComponent<MultiplayerMoveAndShoot>().TakeDamage_RPC(damage);
            
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
           
            collision.gameObject.GetComponent<MultiplayerMoveAndShoot>().TakeDamage_RPC(damage);

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
