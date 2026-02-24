using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class destroyParticle : MonoBehaviour
{
    PhotonView view;
    public float destroyTime;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        view.RPC(nameof(DestroyObject), RpcTarget.AllBuffered);
       
    }
    [PunRPC]
    void DestroyObject()
    {
        Destroy(this.gameObject,destroyTime);
    }
}
