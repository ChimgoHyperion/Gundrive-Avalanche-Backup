using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerMangerr : MonoBehaviour
{
    public int deaths;
    public PhotonView PV;
    public int kills;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    public void GetDeath()
    {
        PV.RPC(nameof(RPC_GetDeath), PV.Owner); 
        
    }
    [PunRPC]
    void RPC_GetDeath()
    {
      //  deaths++;

        Hashtable hash = new Hashtable();
        hash.Add("Deaths", deaths + 1);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
   
    public void GetKill()
    {
        PV.RPC(nameof(RPC_GetKill), PV.Owner);

    }
    [PunRPC]
    void RPC_GetKill()
    {
      //  kills++;
        Hashtable hashkill = new Hashtable();
        hashkill.Add("Kills", kills + 1);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashkill);
    }

    public static PlayerMangerr Find (Player player)
    {
        return FindObjectsOfType<PlayerMangerr>().SingleOrDefault(x => x.PV.Owner == player);
    }
  
}
