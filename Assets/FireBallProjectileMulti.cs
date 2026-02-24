using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class FireBallProjectileMulti : NetworkBehaviour
{
    public GameObject explosionEffect;
    public GameObject RemnantEffect;
    public int damage;
    public float radius;
    public float force;
    public LayerMask layertoHit;

    public float intensity;
    public float time;

    public GameObject Attacker;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("World"))
        {

            RPC_processCollision();
            Instantiate(RemnantEffect, transform.position, Quaternion.identity);
        }

        if (collision.gameObject.tag == "Player")
        {
            RPC_processCollision();

            collision.gameObject.GetComponent<MultiplayerMoveAndShoot>().TakeDamage_RPC(damage);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            RPC_processCollision();
            collision.gameObject.GetComponent<EnemyScript>().TakeDamage(damage);
        }
        if (collision.gameObject.tag == "Boss")
        {
            RPC_processCollision();
            collision.gameObject.GetComponent<BossHealthScript>().TakeDamage(damage);
        }
        if (collision.gameObject.tag == "Enemy5")
        {
            RPC_processCollision();
            collision.gameObject.GetComponent<TakeDamageandDisappear>().TakeDamage(damage);
        }
        if (collision.gameObject.tag == "ConsPow")
        {
            RPC_processCollision();

        }
        if (collision.gameObject.tag == "PowerUp")
        {
            RPC_processCollision();

        }
        // for testing combat game
        if (collision.gameObject.tag == "Ground")
        {
            RPC_processCollision();

        }
        if (collision.gameObject.CompareTag("BottomDeath"))
        {
            RPC_processCollision();
            RPC_SpawnEffect();// maybe useless
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.CompareTag("BottomDeath"))
        //{
        //    RPC_processCollision();
        //    RPC_SpawnEffect();// maybe useless
        //}
    }

    [Rpc]
    void RPC_processCollision()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        ScreenShake.instance.shakeCamera(intensity, time);
        Explode();
        Destroy(gameObject,0.1f);
        FusionNetworkManager.runnerInstance.Despawn(GetComponent<NetworkObject>());
    }
    [Rpc]
    void RPC_SpawnEffect()
    {
        Instantiate(RemnantEffect, transform.position, Quaternion.identity);
    }
    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, layertoHit);

        foreach (Collider2D obj in colliders)
        {
            Vector2 direction = obj.transform.position - transform.position;
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(direction * force);


            }
            MultiplayerMoveAndShoot Target = obj.GetComponent<MultiplayerMoveAndShoot>();
            if (Target != null)
            {
                Target.TakeDamage_RPC(damage);
                Target.RegisterAttacker(Attacker);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    IEnumerator spawnFire()
    {

        yield return new WaitForSeconds(2f);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
