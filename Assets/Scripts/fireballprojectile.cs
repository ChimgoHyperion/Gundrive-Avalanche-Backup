using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class fireballprojectile : MonoBehaviour
{
    public GameObject explosionEffect;
    public GameObject RemnantEffect;
    public int damage;
    public float radius;
    public float force;
    public LayerMask layertoHit;

    public float intensity;
    public float time;
    public int shooterId;

    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("World"))
        {

            processCollision();
          PhotonNetwork.Instantiate(RemnantEffect.name, transform.position, Quaternion.identity);
        }

        if (collision.gameObject.tag == "Player")
        {
            processCollision();

            collision.gameObject.GetComponent<MultiplayerMoveAndShoot>().TakeDamage_RPC(damage);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            processCollision();
            collision.gameObject.GetComponent<EnemyScript>().TakeDamage(damage);
        }
        if (collision.gameObject.tag == "Boss")
        {
            processCollision();
            collision.gameObject.GetComponent<BossHealthScript>().TakeDamage(damage);
        }
        if (collision.gameObject.tag == "Enemy5")
        {
            processCollision();
            collision.gameObject.GetComponent<TakeDamageandDisappear>().TakeDamage(damage);
        }
        if (collision.gameObject.tag == "ConsPow")
        {
            processCollision();

        }
        if (collision.gameObject.tag == "PowerUp")
        {
            processCollision();

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BottomDeath"))
        {
            processCollision();
           PhotonNetwork.Instantiate(RemnantEffect.name, transform.position, Quaternion.identity);
        }
    }
    void processCollision()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        ScreenShake.instance.shakeCamera(intensity, time);
        Explode();
        gameObject.SetActive(false);
        GetComponent<PhotonView>().RPC(nameof(DestroyObject), RpcTarget.AllBuffered);
        DestroyObject();
    }
    [PunRPC] void DestroyObject()
    {
        Destroy(this.gameObject);
    }
    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, layertoHit);

        foreach (Collider2D obj in colliders)
        {
          /*  Vector2 direction = obj.transform.position - transform.position;
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(direction * force);


            }*/
            EnemyScript enem = obj.GetComponent<EnemyScript>();
            MultiplayerMoveAndShoot player = obj.GetComponent<MultiplayerMoveAndShoot>();

            if (enem != null)
            {
                enem.TakeDamage(damage);
            }
            if (player!= null)
            {
                if (player.GetComponent<PhotonView>().ViewID != shooterId)
                {
                  //  player.setshooter(shooterId);
                }
                if (player.GetComponent<PhotonView>().ViewID == shooterId)
                {
                  //  player.setshooter(0);
                }
                player.TakeDamage_RPC(damage);
               
             // player.GetComponent<PhotonView>().RPC("ReduceHealth", RpcTarget.AllBuffered, damage);
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
