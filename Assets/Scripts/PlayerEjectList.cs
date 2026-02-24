using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PlayerEjectList : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform container;
    [SerializeField] GameObject PlayerEjectPrefab;

    Dictionary<Player, PlayerEject> playerejectitems = new Dictionary<Player, PlayerEject>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddplayerEjectItem(player);
        }
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddplayerEjectItem(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }
    void AddplayerEjectItem(Player player)
    {
        PlayerEject item = Instantiate(PlayerEjectPrefab, container).GetComponent<PlayerEject>();
        item.Initialize(player);
        playerejectitems[player] = item;
    }
    void RemovePlayerEjectItem(Player player)
    {
        Destroy(playerejectitems[player].gameObject);
        playerejectitems.Remove(player);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
