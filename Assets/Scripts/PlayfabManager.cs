using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class PlayfabManager : MonoBehaviour
{
    [Header("Windows")]
    public GameObject nameWindow;
    public GameObject leaderboardWindow;
    public GameObject deathpanel;
    bool ison=true;

    [Header("Leaderboard")]
    public GameObject rowPrefab;
    public Transform rowsParent;

    [Header("Display name window")]
    public GameObject nameError;
    public InputField nameInput;

    string loggedInplayfabID;

    // Start is called before the first frame update
    void Start()
    {
        //  Login();
      //  GetLeaderboardAroundPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // activate leaderboard
    public void ActivateLeaderboard()
    {
        if (ison)
        {
            leaderboardWindow.SetActive(true);
            
           
            ison = false;
        } else
        if (ison == false)
        {
            leaderboardWindow.SetActive(false);
           
            ison = true;
        }
      
    }
    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }
    void OnSuccess(LoginResult result)
    {
        loggedInplayfabID = result.PlayFabId;
        Debug.Log("Successful login/account created!");
        string name = null;
        if(result.InfoResultPayload.PlayerProfile!=null)
        name = result.InfoResultPayload.PlayerProfile.DisplayName;

        if (name == null)
            nameWindow.SetActive(true);
       // else
         //   leaderboardWindow.SetActive(true);
    }
    public void SubmitNameButton()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nameInput.text,

        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }
    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated display name!");
        leaderboardWindow.SetActive(true);
    }
    void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in / creating account!");
        Debug.Log(error.GenerateErrorReport());
    }
    // leaderboard part
    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName="PlatformScore",
                    Value= score
                }

            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successful Leaderboard sent");
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "PlatformScore",
            StartPosition = 0,
            MaxResultsCount = 5
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }
    public void GetLeaderboardAroundPlayer()
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "PlatformScore",
            MaxResultsCount = 5
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardAroundPlayerGet, OnError);
    }
    // for getting the top players
    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach(Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }
        foreach(var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            Text[] texts = newGo.GetComponentsInChildren<Text>();
            texts[0].text = (item.Position +1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();

            if (item.PlayFabId == loggedInplayfabID)
            {
                texts[0].color = Color.cyan;
                texts[1].color = Color.cyan;
                texts[2].color = Color.cyan;
            }

            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
        }
    }
    // for getting scores around player

    void OnLeaderboardAroundPlayerGet(GetLeaderboardAroundPlayerResult result)
    {
        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            Text[] texts = newGo.GetComponentsInChildren<Text>();
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();

            if (item.PlayFabId == loggedInplayfabID)
            {
                texts[0].color = Color.cyan;
                texts[1].color = Color.cyan;
                texts[2].color = Color.cyan;
            }
          /*  if (loggedInplayfabID==item.PlayFabId)
            {
                texts[0].color = Color.red;
                texts[1].color = Color.cyan;
                texts[2].color = Color.cyan;
            }*/


            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
        }
    }
}
