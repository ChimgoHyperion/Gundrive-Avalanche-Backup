using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RemoveGun : MonoBehaviour
{
    public Transform Guncontainer;
    public WeaponHolderMulti weaponHolder;
    PhotonView view;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
    }


    [PunRPC]
    public void RPCRemoveGuns()
    {
        foreach( Transform  gun in Guncontainer)
        {
         //   weaponHolder.hasWeapon = false;
            if(gun.gameObject.activeSelf== true)
            {
                gun.gameObject.SetActive(false);
            }
        }
    }
    public void RemoveGuns()
    {
        view.RPC(nameof(RPCRemoveGuns), RpcTarget.AllBuffered);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
