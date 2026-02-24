using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class firebulletprojectile : MonoBehaviour
{

    public GameObject HitEffect;
    public int damage;
    PhotonView view;
    public int shooterId;

    private void Start()
    {
        view = GetComponent<PhotonView>(); 
    }

    [PunRPC]
    void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer== LayerMask.NameToLayer("PowerBox"))
        {
          //  PhotonNetwork.Instantiate(HitEffect.name, transform.position, Quaternion.identity);
            view.RPC(nameof(DestroyObject), RpcTarget.AllBuffered);
        }

        if (collision.gameObject.tag == "Player")
        {
            PhotonNetwork.Instantiate(HitEffect.name, transform.position, Quaternion.identity);
           
           
            collision.gameObject.GetComponent<MultiplayerMoveAndShoot>().TakeDamage_RPC(damage);
            view.RPC(nameof(DestroyObject), RpcTarget.AllBuffered);
        }

       
        

        if (collision.gameObject.CompareTag("World"))
        {
            PhotonNetwork.Instantiate(HitEffect.name, transform.position, Quaternion.identity);
            view.RPC(nameof(DestroyObject), RpcTarget.AllBuffered);
          
        }
        if (collision.gameObject.CompareTag("PowerUp"))
        {
          //  PhotonNetwork.Instantiate(HitEffect.name, transform.position, Quaternion.identity);
            view.RPC(nameof(DestroyObject), RpcTarget.AllBuffered);
            
        }
     
        if (collision.gameObject.tag == "ConsPow")
        {
           
          //  PhotonNetwork.Instantiate(HitEffect.name, transform.position, Quaternion.identity);
            view.RPC(nameof(DestroyObject), RpcTarget.AllBuffered);
        }
        if (collision.gameObject.tag == "PowerUp")
        {
            
          //  PhotonNetwork.Instantiate(HitEffect.name, transform.position, Quaternion.identity);
            view.RPC(nameof(DestroyObject), RpcTarget.AllBuffered);
        }
        

       
    }


    // Update is called once per frame
    void Update()
    {
        if(gameObject!=null)
        view.RPC(nameof(DestroyAfterSec), RpcTarget.AllBuffered,3f);
    }
    [PunRPC]
    void DestroyAfterSec(float time)
    {
      // PhotonNetwork.Destroy(this.gameObject, 6f);
        Destroy(gameObject,time);
    }
   
}
