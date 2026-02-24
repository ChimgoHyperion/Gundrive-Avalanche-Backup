using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class GameManagerMulti : NetworkBehaviour
{
    [Header("For Teams Management")]
    public const string SessionTypeKey = "PlayersInRoom";
 


    [Header("For waiting Management")]
    // waiting room management
    public List<MultiplayerMoveAndShoot> playersInRoom;
    [SerializeField] bool WaitCompleted = false;

    // for countdown
    public float countdownTime = 30f;
    [SerializeField] float countdownTimer;
    public int maxPlayers;

    public bool hasStarted = false;

    [Header("Box Spawning")]
    public string spawnPointTag = "SpawnPoint";
    public List<Transform> spawnPoints;
    public GameObject[] PickUps;

    public static GameManagerMulti Instance;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null) { Instance = this; }


        countdownTimer = countdownTime; // we need to also make sure this doesnt spawn 2 times across the clients' devices
        InvokeRepeating(nameof(CheckSessionProperties), 1f, 3f);


        if (!HasStateAuthority) return;

        // Find all spawn points in scene
        var goList = GameObject.FindGameObjectsWithTag(spawnPointTag);
        spawnPoints = new List<Transform>();
        foreach (var go in goList)
        {
            spawnPoints.Add(go.transform);
        }
           

        Debug.Log($"Found {spawnPoints.Count} spawn points.");

      //  InvokeRepeating(nameof(multiplayerSpawn), 10f, 10f);

    }

    public void CheckSessionProperties()
    {


        if (FusionNetworkManager.runnerInstance.SessionInfo.Properties.TryGetValue(SessionTypeKey, out SessionProperty playersAllowed))
        {

            maxPlayers = (int)playersAllowed;
           
        }
        else
        {
            return;
        }
    }
   

    // Update is called once per frame
    void Update()
    {
        if (playersInRoom != null)
        {
            playersInRoom = new List<MultiplayerMoveAndShoot>(FindObjectsOfType<MultiplayerMoveAndShoot>());
        }
        if (playersInRoom.Count == maxPlayers)
        {
            FusionNetworkManager.runnerInstance.SessionInfo.IsOpen = false;
            if (!WaitCompleted)
            {
                RPC_StartGameSessionTime();
                WaitCompleted = true;
            }


        }
        CountDownTimer();
    }

    [Rpc]
    public void RPC_StartGameSessionTime()
    {

        if (!hasStarted)
        {
            triggeredCountdown = true; // start countdown to waiting room close
            StartCoroutine(GameStartCoroutine());
            hasStarted = true;
        }


    }
    IEnumerator GameStartCoroutine()
    {
      
        yield return new WaitForSeconds(5f); 

        Debug.Log("game start for players");

       
        RPC_BroadCastGameStart();
        // or use runner based functions
        // then disable waiting room ui

        // call gamestart for all players
    }
    [Rpc]
    public void RPC_BroadCastGameStart()
    {
        FusionNetworkManager.networkManagerInstance.waitingRoomUI.SetActive(false);
      
        FusionNetworkManager.networkManagerInstance.LobbyCamera.SetActive(false);
       

         FusionNetworkManager.networkManagerInstance.TimerObj.SetActive(true);


        foreach (MultiplayerMoveAndShoot players in playersInRoom)
        {
            players.GameStart(); // activate rigidbodies of players

           // can start spawning powerups

        }
    }
    public float timeLeft = 5;
    public bool triggeredCountdown = false;
    void CountDownTimer()
    {
        if (triggeredCountdown)
        {

            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                FusionNetworkManager.networkManagerInstance.WaitingRoomCountDownText.text = "Starting in..." + (int)timeLeft + "s";

            }
            else
            {
                triggeredCountdown = false;
                return;

            }
        }

    }


    [Rpc]
    public void EndGameSessionByTime_RPC()
    {
        //LobbyNetworkManager.networkManagerInstance.OnGameOver();

        //scoreboardPanel.GetComponent<CanvasGroup>().alpha = 1;
        //scoreboardPanel.GetComponent<CanvasGroup>().interactable = true;
        //scoreboardPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;



        foreach (MultiplayerMoveAndShoot player in playersInRoom)
        {
            player.GameEnds();



        }


       DeclareWinner_RPC();

    }

    [Rpc]
    void DeclareWinner_RPC()// find the winner
    {
        int highestScore = int.MinValue;
        string winnerName = "No Winner";

        foreach (MultiplayerMoveAndShoot player in playersInRoom)
        {
          //  PlayerController player = playerObject.GetComponent<PlayerController>();

            if (player == null) continue;

            int score = player.NetworkedScore; // This is synced across the network

            if (score > highestScore)
            {
                highestScore = score;
                winnerName = player.NetworkedNickName; // or use Fusion's playerRef.NickName if using PlayerRef
            }
        }

        Debug.Log("Winner is: " + winnerName + " with score: " + highestScore);

        // Optionally, show this on a UI text element
        //  winnerTextUI.text = $"Winner: {winnerName} ({highestScore} points)";

        FusionNetworkManager.networkManagerInstance.WinnerText.text = "Winner: " + winnerName;
    }

    void multiplayerSpawn()
    {
        int SpawnPointsindex = Random.Range(0, spawnPoints.Count);
       

        int boxIndex = Random.Range(0, 1);


        SpawnPickup(PickUps[boxIndex], SpawnPointsindex);

       
    }
    public void SpawnPickup(GameObject prefab, int index)
    {
        if (!HasStateAuthority) return;

        if (spawnPoints != null && index >= 0 && index < spawnPoints.Count)
        {
            var pos = spawnPoints[index].position;
            Runner.Spawn(prefab, pos, Quaternion.identity);
        }
        else
            Debug.LogWarning("Invalid spawn index");
    }
}
