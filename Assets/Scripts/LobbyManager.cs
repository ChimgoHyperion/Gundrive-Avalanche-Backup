using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public InputField roomInputField;
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public GameObject SkinPanel;
    public Text roomName;

    public RoomItem roomItemPrefab;
    List<RoomItem> roomItemsList = new List<RoomItem>();
    public Transform contentObject;

    public float timebtwUpdates = 1.5f;
    float nextUpdatetime;

    List<PlayerItem> playerItemsList = new List<PlayerItem>();
    public PlayerItem playerItemPrefab;
    public Transform playerItemParent;

    public GameObject playbutton;
    public GameObject MapScrollView;
    public int selectedLevel;
    public GameObject LoadingText, LoadingImage;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.JoinLobby();
        selectedLevel = PlayerPrefs.GetInt("SelectedMulti");
        if (selectedLevel == 0)
        {
            PlayerPrefs.SetInt("SelectedMulti",10);
        }
    }
    public void OnClickCreate()
    {
        if (roomInputField.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions() { MaxPlayers = 5, BroadcastPropsChangeToAll=true });
            // issue it starts the game without play game btn being pressed
            
        }
    }
    public override void OnJoinedRoom()
    {
        // base.OnJoinedRoom();
        lobbyPanel.SetActive(false);
        SkinPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = "Lobby:" +  PhotonNetwork.CurrentRoom.Name;
        // update list of players in the lobby 
        UpdatePlayerList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }
    // Update is called once per frame
    void Update()
    {
          if (PhotonNetwork.IsMasterClient&& PhotonNetwork.CurrentRoom.PlayerCount>=2)
          {
              playbutton.SetActive(true);
              MapScrollView.SetActive(true);
          }
          else
          {
              playbutton.SetActive(false);
              MapScrollView.SetActive(false);
          } 
        // for testing in offline mode we turn off
        selectedLevel = PlayerPrefs.GetInt("SelectedMulti");
    }
    // for selecting level
    public void SelectLevel(int levelNumber)
    {
        PlayerPrefs.SetInt("SelectedMulti", levelNumber);
    }
    public void OnClickPlayButton()
    {
        // PhotonNetwork.LoadLevel("Multiplayer Game Scene");// will be modified to load the selected level
        photonView.RPC(nameof(ShowLoading), RpcTarget.AllBuffered);
        PhotonNetwork.LoadLevel(selectedLevel);
       // PhotonNetwork.CurrentRoom.IsOpen = false; // supposed to shut off others from joining after game stars will put in pawn players script to test

    }
    [PunRPC] public void ShowLoading()
    {
        LoadingText.SetActive(true);
        LoadingImage.SetActive(true);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(Time.time>= nextUpdatetime)
        {
            UpdateRoomList(roomList);
            nextUpdatetime = Time.time + timebtwUpdates;
        }
        
    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        foreach (RoomItem item in roomItemsList)
        {
            Destroy(item.gameObject);
        }
        roomItemsList.Clear();
        foreach( RoomInfo room in list)
        {
            RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
            newRoom.SetRoomName(room.Name);
            roomItemsList.Add(newRoom);
        }

    }
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
    public void OnClickLeftRoom()
    {
        PhotonNetwork.LeaveRoom();
        
    }
    public override void OnLeftRoom()
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        SkinPanel.SetActive(true);
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    void UpdatePlayerList()
    {
        foreach(PlayerItem item in playerItemsList)
        {
            Destroy(item.gameObject);
        }
        playerItemsList.Clear();
        if(PhotonNetwork.CurrentRoom == null)
        {
            return;
        }
        foreach(KeyValuePair<int,Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
          
            newPlayerItem.SetPlayerInfo(player.Value);
            if(player.Value == PhotonNetwork.LocalPlayer)
            {
              //  newPlayerItem.ApplyLocalChanges();
            }
            playerItemsList.Add(newPlayerItem);
        }
    }

}
