using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FusionNetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public static NetworkRunner runnerInstance;
    public static FusionNetworkManager networkManagerInstance;
    public string LobbyName = "default";
    [Header("To collect player data")]
   // [SerializeField] TMP_InputField EnterRoomIDInputField;
    public InputField EnterRoomIDInputField; // tmpro doesnt work on web for some reason
    public GameObject GameSessionManagement, lobbyPanel,waitingRoomUI, LobbyCamera, TimerObj;
    public GameObject playerPrefab;
    public TextMeshProUGUI RoomID,connectionStateText, WaitingRoomCountDownText,WinnerText;
    public Button CreateRoomBtn, JoinRoomBtn;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitialOffering());
        // mapAudioSource.clip = MapMusicList[SelectedMap]; // needs to be changed to an activation systemb
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            Destroy(gameObject);
        }
    }
    IEnumerator InitialOffering()
    {
        yield return new WaitForSeconds(2f);
        // singleton for the network runner itself
        runnerInstance = gameObject.GetComponent<NetworkRunner>();

        if (runnerInstance == null)
        {
            runnerInstance = gameObject.AddComponent<NetworkRunner>(); // testing code removal because of fusion voice client
        }
        // singleton for the lobbynetwork manager
        if (networkManagerInstance == null) { networkManagerInstance = this; }

        yield return new WaitForSeconds(1f);

        runnerInstance.JoinSessionLobby(SessionLobby.Shared, LobbyName);

    }
    // Update is called once per frame
    void Update()
    {
        if (runnerInstance != null)
        {
            RoomID.text = runnerInstance.SessionInfo.Name;
        }
       
        PlayerCountText.text = PlayerCount.ToString();
    }
    // creating room for 4 players
    public void Create4PlayerGame(string gameType)
    {
       // WaitingLobbyUI.SetActive(true);

        int randomInt = UnityEngine.Random.Range(1000, 9999);
        string randomSessionName = "Rm-" + randomInt.ToString();

        // customizing the Room/Session's properties
        var customProperties = new Dictionary<string, SessionProperty>();
        // a way of assigning or pairing the 
        // key of the customproperties dictionary to the value of 'gameType' , which also a string representing the selected gametype
        customProperties["Type"] = gameType;

        runnerInstance.StartGame(new StartGameArgs()
        {
            PlayerCount = 4,
            SessionName = randomSessionName,
            GameMode = GameMode.Shared,
            SessionProperties = customProperties
        });
    }


    public void CreateRoomWithPlayerCount()
    {
        int randomInt = UnityEngine.Random.Range(1000, 9999);
        string randomSessionName = "Rm-" + randomInt.ToString();

        // customizing the Room/Session's properties
        var customProperties = new Dictionary<string, SessionProperty>();
        // a way of assigning or pairing the 
        // key of the customproperties dictionary to the value of 'gameType' , which also a string representing the selected gametype
        customProperties["PlayersInRoom"] = PlayerCount;

        runnerInstance.StartGame(new StartGameArgs()
        {
            PlayerCount = PlayerCount,// set by the function argument
            SessionName = randomSessionName,
            GameMode = GameMode.Shared,
            SessionProperties = customProperties
        });
    }
    // creating room for 2 player
    public void CreateQuick1v1(string gameType)
    {
        int randomInt = UnityEngine.Random.Range(1000, 9999);
        string randomSessionName = "Rm-" + randomInt.ToString();

        // customizing the Room/Session's properties
        var customProperties = new Dictionary<string, SessionProperty>();
        // a way of assigning or pairing the 
        // key of the customproperties dictionary to the value of 'gameType' , which also a string representing the selected gametype
        customProperties["Type"] = gameType;

        runnerInstance.StartGame(new StartGameArgs()
        {
            PlayerCount = 2,
            SessionName = randomSessionName,
            GameMode = GameMode.Shared,
            SessionProperties = customProperties
        });

    }
    public async void ConnecToSpecificSession()
    {
        if (EnterRoomIDInputField.text.Length > 0 && EnterRoomIDInputField.text.Length < 5)
        {
            await runnerInstance.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                SessionName = "Rm-" + EnterRoomIDInputField.text,


            });
        }


        string enteredCode = EnterRoomIDInputField.text;

    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runnerInstance.SessionInfo.Properties.TryGetValue("IsLocked", out SessionProperty lockState))
        {
            if ((bool)lockState == true)
            {
                runner.Disconnect(player);
            }


        }
        if (runnerInstance.SessionInfo.PlayerCount == 1)
        {
            // spawn a single instance of team manager
            StartCoroutine(SpawnManagers());
        }

        StartCoroutine(SequenceAfterRoomCreation(player));

       
    }
   
    IEnumerator SpawnManagers()
    {
        yield return new WaitForSeconds(1f);
        
        runnerInstance.Spawn(GameSessionManagement, Vector2.zero);
        yield return new WaitForSeconds(2f);
       // runnerInstance.Spawn(WeaponSpawner, Vector2.zero);
    }
    IEnumerator SequenceAfterRoomCreation(PlayerRef player)
    {

       

        // wait before spawning player object
        yield return new WaitForSeconds(1f);
        if (player == runnerInstance.LocalPlayer)
        {
            lobbyPanel.SetActive(false);

            waitingRoomUI.SetActive(true);
           
            NetworkObject playernetworkobject = runnerInstance.Spawn(playerPrefab, new Vector2(0,0), Quaternion.identity);
            runnerInstance.SetPlayerObject(player, playernetworkobject);

          

        }

    }
    public void LeaveSession()
    {
        // runnerInstance.Disconnect(runnerInstance.LocalPlayer);
        runnerInstance.Shutdown(true, ShutdownReason.Ok);
        // in the Onshutdown method scene is reloaded.

        //lobbyCanvasgroup.SetActive(true);
        Time.timeScale = 1;

        // rejoin lobby( either through reloading the scene or restarting the network runner)

    }

    public void LeaveAndEnterMainMenu()
    {
        //   runnerInstance.Disconnect(runnerInstance.LocalPlayer);
        runnerInstance.Shutdown(true, ShutdownReason.Ok);
        SceneManager.LoadScene("Main Menu");

        Destroy(gameObject);
    }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        connectionStateText.text = "Connection success";
        connectionStateText.color = Color.green;

        CreateRoomBtn.interactable = true;
        JoinRoomBtn.interactable = true;
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);// reload scene
    }


   
    public int PlayerCount = 2;    // Tracks the current playerCount 
    public TextMeshProUGUI PlayerCountText;
   

    public void ScrollLeft()
    {
       
        if (PlayerCount > 2)
        {
            PlayerCount--;
        }
       
      
    }

    public void ScrollRight()
    {
       

        if (PlayerCount < 4)
        {
            PlayerCount++;
        }
    }

   
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
       
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
       
    }

   

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        
    }

   
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
       
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
       
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
       
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }

  
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
       
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
       
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }
}
