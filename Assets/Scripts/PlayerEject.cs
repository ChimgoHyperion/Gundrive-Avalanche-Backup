using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerEject : MonoBehaviourPunCallbacks
{
    public Text userName;
    public Text deathcount;

    Player player;

    public void Initialize(Player player) // find where this function should be called
    {
        userName.text = player.NickName;
        this.player = player;
        updateStats();
    }
    void updateStats()
    {
        if (player.CustomProperties.TryGetValue("Deaths", out object deaths))
        {
            deathcount.text = deaths.ToString();
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == player)
        {
            if (changedProps.ContainsKey("Deaths"))
            {
                // deathcounter should be in player manager and is stored in a hashtable
                updateStats();
            }

        }
    }

    public void EjectPlayer()
    {
        
        PhotonNetwork.CloseConnection(this.player);
       
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
