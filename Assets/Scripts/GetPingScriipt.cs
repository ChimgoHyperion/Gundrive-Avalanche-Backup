using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GetPingScriipt : MonoBehaviourPunCallbacks
{
    public Text PingText;
    
    // Start is called before the first frame update
    void Start()
    {
       // PhotonNetwork.JoinRoom(PhotonNetwork.CurrentRoom.Name);
    }

    // Update is called once per frame
    void Update()
    {
        PingText.text = "PING " + PhotonNetwork.GetPing();
    }
    // as game manager

    // doesnt work
    public GameObject PlayerFeed;
    public GameObject FeedGrid;

    private void OnPhotonPlayerConnected(Player player)
    {
        Debug.Log("Works");
        GameObject Obj = Instantiate(PlayerFeed, new Vector2(0, 0), Quaternion.identity);
        Obj.transform.SetParent(FeedGrid.transform, false);
        Obj.GetComponent<Text>().text = player.NickName + "joined the game";
        Obj.GetComponent<Text>().color = Color.green;
    }
    private void OnPhotonPlayerDisconnected(Player player)
    {
        Debug.Log("Works");
        GameObject Obj = Instantiate(PlayerFeed, new Vector2(0, 0), Quaternion.identity);
        Obj.transform.SetParent(FeedGrid.transform, false);
        Obj.GetComponent<Text>().text = player.NickName + "left the game";
        Obj.GetComponent<Text>().color = Color.red;
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        Debug.Log("Works");
        GameObject Obj = Instantiate(PlayerFeed, new Vector2(0, 0), Quaternion.identity);
        Obj.transform.SetParent(FeedGrid.transform, false);
        Obj.GetComponent<Text>().text = player.NickName + "left the game";
        Obj.GetComponent<Text>().color = Color.red;
    }
    IEnumerator CloseRoom()
    {
        yield return new WaitForSeconds(5f); // wait for 5 sedconds
        PhotonNetwork.CurrentRoom.IsOpen = false; // makes it impossible for room to be joined
        PhotonNetwork.CurrentRoom.IsVisible = false; // makes room invisible to those who never join
    }
}
