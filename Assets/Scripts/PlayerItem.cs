using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    public Text playerName;

  //  Image backgroundImage;
  //  public Color highlightColor;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

    public Image playerAvatar;
    public Sprite[] avatars;

    Player player;

    // for skin changing

    public CharacterDataBase characterDB;

    public int SelectedChar;

    // Start is called before the first frame update
    void Start()
    {
        SelectedChar = PlayerPrefs.GetInt("SelectedChar");
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "playerAvatar", SelectedChar } });
    }
    void Update()
    {
        SelectedChar = PlayerPrefs.GetInt("SelectedChar");
       
    }
    

    public void SetPlayerInfo(Player _player)
    {
        playerName.text = _player.NickName;
        player = _player;
        updatePlayerItem(player);
    } 
    public void ApplyLocalChanges()
    {
      
    }
    public void OnPlayerJoinedRoomPersonal()
    {
        playerProperties["playerAvatar"] = playerAvatar; // player avatar properties set to playerAvatar Image
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
   
    // Update is called once per frame
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(player== targetPlayer)
        {
            updatePlayerItem(targetPlayer);
        }
    }
    void updatePlayerItem( Player player)
    {
        if (player.CustomProperties.ContainsKey("playerAvatar"))
        {
            playerAvatar.sprite = avatars[(int)player.CustomProperties["playerAvatar"]];
            playerProperties["playerAvatar"] = (int)player.CustomProperties["playerAvatar"];
        }
        else
        {
            playerProperties["playarAvatar"] = 0;// checkkk
        }
    }
    public void clickedSkin1()
    {
        PlayerPrefs.SetInt("SelectedChar",0);
        playerProperties["playerAvatar"] = 0;
       // playerAvatar.sprite = avatars[0];
        if ((int)playerProperties["playerAvatar"] == 0)
        {
            playerProperties["playerAvatar"] = 0;
            playerAvatar.sprite = avatars[0];
           // playerAvatar.sprite = avatars[(int)player.CustomProperties["playerAvatar"]];
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
    public void clickedSkin2()
    {
        PlayerPrefs.SetInt("SelectedChar",1);
        playerProperties["playerAvatar"] = 1;
       
        if ((int)playerProperties["playerAvatar"] == 1)
        {
            playerProperties["playerAvatar"] = 1;
            playerAvatar.sprite = avatars[1];
          
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
    public void clickedSkin3()
    {
        PlayerPrefs.SetInt("SelectedChar",2);
        playerProperties["playerAvatar"] = 2;

        if ((int)playerProperties["playerAvatar"] == 2)
        {
            playerProperties["playerAvatar"] = 2;
            playerAvatar.sprite = avatars[2];

        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
    public void clickedSkin4()
    {
        PlayerPrefs.SetInt("SelectedChar",3);
        playerProperties["playerAvatar"] = 3;

        if ((int)playerProperties["playerAvatar"] == 3)
        {
            playerProperties["playerAvatar"] = 3;
            playerAvatar.sprite = avatars[3];

        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
    public void clickedSkin5()
    {
        PlayerPrefs.SetInt("SelectedChar",4);
        playerProperties["playerAvatar"] = 4;

        if ((int)playerProperties["playerAvatar"] == 4)
        {
            playerProperties["playerAvatar"] = 4;
            playerAvatar.sprite = avatars[4];

        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
    public void clickedSkin6()
    {
        PlayerPrefs.SetInt("SelectedChar",5);
        playerProperties["playerAvatar"] = 5;

        if ((int)playerProperties["playerAvatar"] == 5)
        {
            playerProperties["playerAvatar"] = 5;
            playerAvatar.sprite = avatars[5];

        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
    public void clickedSkin7()
    {
        PlayerPrefs.SetInt("SelectedChar",6);
        playerProperties["playerAvatar"] = 6;

        if ((int)playerProperties["playerAvatar"] == 6)
        {
            playerProperties["playerAvatar"] = 6;
            playerAvatar.sprite = avatars[6];

        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
    public void clickedSkin8()
    {
        PlayerPrefs.SetInt("SelectedChar",7);
        playerProperties["playerAvatar"] = 7;

        if ((int)playerProperties["playerAvatar"] == 7)
        {
            playerProperties["playerAvatar"] = 7;
            playerAvatar.sprite = avatars[7];

        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
    public void clickedSkin9()
    {
        PlayerPrefs.SetInt("SelectedChar",8);
        playerProperties["playerAvatar"] = 8;

        if ((int)playerProperties["playerAvatar"] == 8)
        {
            playerProperties["playerAvatar"] = 8;
            playerAvatar.sprite = avatars[8];

        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
    public void clickedSkin10()
    {
        PlayerPrefs.SetInt("SelectedChar",9);
        playerProperties["playerAvatar"] = 9;

        if ((int)playerProperties["playerAvatar"] == 9)
        {
            playerProperties["playerAvatar"] = 9;
            playerAvatar.sprite = avatars[9];

        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
    public void clickedSkin11()
    {
        PlayerPrefs.SetInt("SelectedChar",10);
        playerProperties["playerAvatar"] = 10;

        if ((int)playerProperties["playerAvatar"] == 10)
        {
            playerProperties["playerAvatar"] = 10;
            playerAvatar.sprite = avatars[10];

        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
    public void clickedSkin12()
    {
        PlayerPrefs.SetInt("SelectedChar",11);
        playerProperties["playerAvatar"] = 11;

        if ((int)playerProperties["playerAvatar"] == 11)
        {
            playerProperties["playerAvatar"] = 11;
            playerAvatar.sprite = avatars[11];

        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
    public void clickedSkin13()
    {
        PlayerPrefs.SetInt("SelectedChar",12);
        playerProperties["playerAvatar"] = 12;

        if ((int)playerProperties["playerAvatar"] == 12)
        {
            playerProperties["playerAvatar"] = 12;
            playerAvatar.sprite = avatars[12];

        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
   
}
