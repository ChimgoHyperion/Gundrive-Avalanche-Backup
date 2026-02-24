using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Realtime;
using System.IO;
using TMPro;


public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public InputField usernameInput;
    public Text buttonText;
    public TMP_InputField UsernameinputField;
    public GameObject loadingImage,ConnectionSuccessText,connectionfailedText;

    // Start is called before the first frame update
    void Start()
    {
        
      // PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickConnect()
    {
        if (usernameInput.text.Length >= 1 && usernameInput.text.Length < 15 || PlayerPrefs.GetString("PlayerNickname").Length >= 1 && PlayerPrefs.GetString("PlayerNickname").Length < 15)
        {
            // PhotonNetwork.NickName = usernameInput.text; check later for personal view stuff
           // PhotonNetwork.OfflineMode = true;
            PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerNickname");
            PhotonNetwork.AutomaticallySyncScene = true;
            buttonText.text = "Connecting....";
            loadingImage.SetActive(false);
            loadingImage.SetActive(true);
            PhotonNetwork.ConnectUsingSettings(); // will be reenabled when offline mode disabled
            //  LoadBalancingClient.Equals("eu", "za");
          //  PhotonNetwork.ConnectToRegion("za");
        }
      /*  if (UsernameinputField.text.Length >= 1 && UsernameinputField.text.Length <15 || PlayerPrefs.GetString("PlayerNickname").Length >= 1 && PlayerPrefs.GetString("PlayerNickname").Length<15)
        {
            // PhotonNetwork.NickName = usernameInput.text; check later for personal view stuff
          //  PhotonNetwork.OfflineMode = true;
            PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerNickname");
            PhotonNetwork.AutomaticallySyncScene = true;
            buttonText.text = "Connecting....";
             PhotonNetwork.ConnectUsingSettings(); // will be reenabled when offline mode disabled
            //  LoadBalancingClient.Equals("eu", "za");
            //  PhotonNetwork.ConnectToRegion("za");
        }*/

        
    }
    public void ConnectToBestRegion()
    {
        // to connect to best region
        loadingImage.SetActive(false);
        loadingImage.SetActive(true);
        PhotonNetwork.NetworkingClient.AppId= "1b068c8b-bac4-4e1e-88fb-65aa5e26500d";
        PhotonNetwork.ConnectToBestCloudServer();
    }
    public void ConnectToRegion(string region)
    {
        loadingImage.SetActive(false);
        loadingImage.SetActive(true);
        PhotonNetwork.NetworkingClient.AppId = "1b068c8b-bac4-4e1e-88fb-65aa5e26500d";
        PhotonNetwork.ConnectToRegion(region);
    }
    public override void OnConnectedToMaster()
    {
        //   base.OnConnectedToMaster();
        loadingImage.SetActive(false);
      //  connectionfailedText.SetActive(false);
        ConnectionSuccessText.SetActive(true);
        SceneManager.LoadScene("Lobby");

    }
    private void OnFailedToConnectToMasterServer()
    {
      //  connectionfailedText.SetActive(true);
    }
    public override void OnJoinedLobby()
    {
        // base.OnJoinedLobby();
    }
}
