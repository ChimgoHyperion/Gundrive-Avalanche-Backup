using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class PickUpMulti : NetworkBehaviour
{

   
    public int GunIndex;

    public float deathTime;
    public Button button;




    void Start()
    {
       
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "World")
        {
            MakeStatic();
        }

        if (collision.gameObject.tag == "Player")
        {
            MakeStatic();
        }
       
    }
    void MakeStatic()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static; // must this Be RPCed???
    }
   

   
    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, deathTime);

       // FusionNetworkManager.runnerInstance.Despawn(this.gameObject.GetComponent<NetworkObject>());

    }
   
}
