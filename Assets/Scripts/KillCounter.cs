using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class KillCounter : MonoBehaviourPunCallbacks
{
    public int kills;
  
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void AddKills()
    {
        
      photonView.RPC(nameof(AddKillsMulti), RpcTarget.AllBuffered);
        
    }
    [PunRPC]
    public void AddKillsMulti()
    {
        kills++;
      //  PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Kills", kills } });
    }
}
