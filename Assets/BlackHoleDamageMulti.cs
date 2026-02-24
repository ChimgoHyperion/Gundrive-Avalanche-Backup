using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleDamageMulti : NetworkBehaviour
{
    public GameObject explosionEffect;
    public GameObject finishEffect;
    public int damage;
    public float radius;
    public float force;
    public float waitTime;

    public float intensity;
    public float time;
    buttonSoundHolder soundHolder;

    public LayerMask layertoHit;
    public GameObject Attacker;
    // Start is called before the first frame update
    void Start()
    {
        soundHolder = FindObjectOfType<buttonSoundHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(MakeKinematic());
        StartCoroutine(destroy());

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
      
        if (collision.gameObject.tag == "Player")
        {
            RPC_processCollision();
          
        }
       
    }

    [Rpc]
    void RPC_processCollision()
    {

        ScreenShake.instance.shakeCamera(intensity, time);
      

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, layertoHit);

        foreach (Collider2D obj in colliders)
        {
           
            MultiplayerMoveAndShoot Target = obj.GetComponent<MultiplayerMoveAndShoot>();
            if (Target != null)
            {
                Target.TakeDamage_RPC(damage);
                Target.RegisterAttacker(Attacker);
            }
        }
        //  gameObject.SetActive(false);
    }
    IEnumerator destroy()
    {
        yield return new WaitForSeconds(waitTime);
        RPC_ShowEffects();
    }

    [Rpc]
    void RPC_ShowEffects()
    {
        Instantiate(finishEffect, transform.position, Quaternion.identity);
        soundHolder.EnemyDeath();
        Destroy(gameObject);
        FusionNetworkManager.runnerInstance.Despawn(GetComponent<NetworkObject>());
    }
    IEnumerator MakeKinematic()
    {
        yield return new WaitForSeconds(2f);
        GetComponent<Rigidbody2D>().isKinematic = true; // for the blackhole gun
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
