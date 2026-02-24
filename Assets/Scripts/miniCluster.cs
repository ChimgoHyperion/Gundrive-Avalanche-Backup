using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class miniCluster : MonoBehaviour
{
   
    IEnumerator destroyBYTime()
    {
        yield return new WaitForSeconds(WaitTime);
        view.RPC(nameof(DestroyObject), RpcTarget.AllBuffered);
        
    }
    [PunRPC]
    void DestroyObject()
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        StartCoroutine(destroyBYTime());
    }
   

    public float speed;
    private Rigidbody2D rb;
    public GameObject impactEfect;
    public float destroyTime;
    public float WaitTime;
    public int damage;
    public float radius;
    public float force;
    public LayerMask layertoHit;
    private PhotonView view;
    public float intesity, time;

    public int shooterId;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();
    }
  
    private void OnCollisionEnter2D(Collision2D collision)
    {
       

        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(wait());
        }
        if (collision.gameObject.tag == "World")
        {
            StartCoroutine(wait());
        }

        if (collision.gameObject.tag == "Enemy")
        {
            StartCoroutine(wait());
        }
        if (collision.gameObject.tag == "Enemy5")
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
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(destroyTime);
        PhotonNetwork.Instantiate(impactEfect.name, transform.position, Quaternion.identity);
        ScreenShake.instance.shakeCamera(intesity, time);
        Explode();
       
        GetComponent<PhotonView>().RPC(nameof(DestroyObject), RpcTarget.AllBuffered);
        PhotonNetwork.Destroy(gameObject);
        Debug.Log("destroyed");


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BottomDeath")
        {
            StartCoroutine(wait());
            
        }
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

            TakeDamageandDisappear enem5 = obj.GetComponent<TakeDamageandDisappear>();

            BossHealthScript boss = obj.GetComponent<BossHealthScript>();

            MultiplayerMoveAndShoot player = obj.GetComponent<MultiplayerMoveAndShoot>();

            if (enem != null)
            {
                enem.TakeDamage(damage);
            }
            if (enem5 != null)
            {
                enem5.TakeDamage(damage);
            }
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }
            if(player!= null)
            {
                if (player.GetComponent<PhotonView>().ViewID != shooterId)
                {
                   // player.setshooter(shooterId);
                }
                if (player.GetComponent<PhotonView>().ViewID == shooterId)
                {
                   // player.setshooter(0);
                }
                
                player.TakeDamage_RPC(damage);
               
                PhotonNetwork.Destroy(gameObject);
                // check
                //  player.GetComponent<PhotonView>().RPC("ReduceHealth", RpcTarget.AllBuffered, damage);
            }

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
   
}
