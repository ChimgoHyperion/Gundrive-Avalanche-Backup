using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class MiniBombProjectileMulti : NetworkBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    public GameObject impactEfect;
    public float destroyTime;
    public int damage;
    public float radius;
    public float force;
    public LayerMask layertoHit;

    public float intesity, time;

    public GameObject Attacker;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void destroyProjectile()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "World")
        {
            StartCoroutine(wait());


        }

        if (collision.gameObject.tag == "Enemy")
        {
            StartCoroutine(wait());


        }
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(wait());

        }
        if (collision.gameObject.tag == "Boss")
        {
            StartCoroutine(wait());

        }
        if (collision.gameObject.tag == "ConsPow")
        {
            StartCoroutine(wait());

        }
        if (collision.gameObject.tag == "PowerUp")
        {
            StartCoroutine(wait());

        }
        if (collision.gameObject.tag == "BottomDeath")
        {
            StartCoroutine(wait());


        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BottomDeath")
        {
            StartCoroutine(wait());


        }
    }
    [Rpc]
    private void RPC_Explode()
    {
        Instantiate(impactEfect, transform.position, Quaternion.identity);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, layertoHit);

        foreach (Collider2D obj in colliders)
        {
            Vector2 direction = obj.transform.position - transform.position;
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(direction * force);


            }
            MultiplayerMoveAndShoot enem = obj.GetComponent<MultiplayerMoveAndShoot>();


            if (enem != null)
            {
                enem.TakeDamage_RPC(damage);
                enem.RegisterAttacker(Attacker);
            }
            
           

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

   
    IEnumerator wait()
    {
        yield return new WaitForSeconds(destroyTime);
       
        ScreenShake.instance.shakeCamera(intesity, time);
        RPC_Explode();
        Destroy(gameObject);
        FusionNetworkManager.runnerInstance.Despawn(GetComponent<NetworkObject>());
    }
    // Update is called once per frame
    void Update()
    {



    }
}
