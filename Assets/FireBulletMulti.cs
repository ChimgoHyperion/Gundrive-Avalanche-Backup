using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using static Fusion.NetworkBehaviour.ChangeDetector;

public class BulletMulti : MonoBehaviour
{
    public GameObject HitEffect;
    public int damage;
    public float radius;
    public LayerMask layertoHit;



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("World"))
        {
            RPC_InstantiateEffect();

        }
        if (collision.gameObject.CompareTag("PowerUp"))
        {
            RPC_InstantiateEffect();

        }
       
        if (collision.gameObject.tag == "ConsPow")
        {
            RPC_InstantiateEffect();

        }
        if (collision.gameObject.tag == "PowerUp")
        {
            RPC_InstantiateEffect();

        }
        if (collision.gameObject.tag == "Player")
        {
            
            RPC_InstantiateEffect();
           
          //  collision.gameObject.GetComponent<MultiplayerMoveAndShoot>().TakeDamage_RPC(damage);
        }
    }

    [Rpc]
    void RPC_InstantiateEffect()
    {
        Destroy(gameObject);
        //  FusionNetworkManager.runnerInstance.Despawn(this.gameObject.GetComponent<NetworkObject>());
        Instantiate(HitEffect, transform.position, Quaternion.identity);
    }

    private void DamageSurroundingEnemies()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, layertoHit);

        foreach (Collider2D obj in colliders)
        {

            MultiplayerMoveAndShoot enemy = obj.GetComponent<MultiplayerMoveAndShoot>();

            
            if (enemy != null)
            {
                enemy.TakeDamage_RPC(damage);
            }

        }
    }
    // Update is called once per frame
    void Update()
    {
        DamageSurroundingEnemies();
        Destroy(gameObject, 5f);

      //  FusionNetworkManager.runnerInstance.Despawn(this.gameObject.GetComponent<NetworkObject>());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
