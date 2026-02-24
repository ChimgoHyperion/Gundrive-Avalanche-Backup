using PlayFab;
using PlayFab.AuthenticationModels;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using Unity.Services.Authentication;
using Unity.Services.Core;


public class IntroPlayfabMana : MonoBehaviour
{
    [Header("Windows")]
    public GameObject nameWindow;
    public GameObject leaderboardWindow;
    public GameObject LoggingInPanel;
    // public GameObject coverimage; // for entering crypto address

    [Header("Display name window")]
    public GameObject nameError;
    public InputField nameInput;

    [Header("Leaderboard")]
    public GameObject rowPrefab;
    public Transform rowsParent;
    bool ison = true;

    string loggedInplayfabID;
    public GameObject noInternetpanel;

    [SerializeField] bool hasInternetConnection;
    public TextMeshProUGUI OnlineStatusText, currentNameText;
    public Button ChangeNameButton, LeaderboardButton;

  
    public string AuthID=null;
    // Start is called before the first frame update
    void Awake()
    {

        //  DontDestroyOnLoad(gameObject);
    }

    IEnumerator CheckInternetLoop()
    {
        while (true)
        {
            bool connected = Application.internetReachability != NetworkReachability.NotReachable;
            OnlineStatusText.text = connected ? "Online" : "Offline";
            OnlineStatusText.color = connected ? Color.green : Color.red;

            ChangeNameButton.interactable = connected ? true : false;// can change name only if you logged in
            LeaderboardButton.interactable = connected ? true : false;
            yield return new WaitForSeconds(5f);


        }
    }
    // To Check for internet connection before loging in
    IEnumerator CheckInternetConnection()
    {


        bool connected = Application.internetReachability != NetworkReachability.NotReachable;
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => AuthID != null);
        if (connected) {
            hasInternetConnection = true;
            Debug.Log("Internet connection available.");
            OnlineStatusText.text = "Online";
            OnlineStatusText.color = Color.green;
          //  Login();
            StartCoroutine(Login());
        }
        else
        {
            hasInternetConnection = false;
            noInternetpanel.SetActive(false);
            Debug.Log("No internet connection.");
            OnlineStatusText.text = "Offline";
            OnlineStatusText.color = Color.white;
        }
     
    }


    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void Start()
    {

        StartCoroutine(CheckInternetConnection());
      

        StartCoroutine(CheckInternetLoop());

    }



    // Update is called once per frame
    void Update()
    {
        //if (LoggedInManager.Instance.isLoggedIn)
        //{
        //    OnlineStatusText.text = "Online";
        //    OnlineStatusText.color = Color.green;
        //}
        //else
        //{
        //    OnlineStatusText.text = "Offline";
        //    OnlineStatusText.color = Color.white;
        //}
    }
    IEnumerator Login()
    {
        if (LoggedInManager.Instance.isLoggedIn)  yield break;

        LoggingInPanel.SetActive(true);





        yield return new WaitUntil(()=>UnityAuthenticationManager.Instance.IDGotten);

        string customId = AuthID;// UnityAuthenticationManager.Instance.AuthID;
        var request = new LoginWithCustomIDRequest
        {
            CustomId = customId,
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
        LoggedInManager.Instance.isLoggedIn = true;
        loggedInplayfabID = result.PlayFabId;
        LoggingInPanel.SetActive(false);
        OnlineStatusText.text = "Online";
        ChangeNameButton.interactable = true;// can change name only if you logged in
        LeaderboardButton.interactable = true;

        Debug.Log("Successful login/account created!");
        string name = null;

        if (result.InfoResultPayload.PlayerProfile != null)
            name = result.InfoResultPayload.PlayerProfile.DisplayName;

        PlayerPrefs.SetString("PlayerNickname", name);

        currentNameText.text = name;

        if (name == null)
            nameWindow.SetActive(true);
        else
            nameWindow.SetActive(false);

        GetLeaderboardOnStart(); // called On first login
        CoinBalanceHolder.Instance.GetInventoryCoinBalance();// called on first login
    }
    public void SubmitNameButton()
    {
        if (nameInput.text.Length >= 1 && nameInput.text.Length < 15)
        {
            PlayerPrefs.SetString("PlayerNickname", nameInput.text);
            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = nameInput.text,// PlayerPrefs.GetString("PlayerNickname"),

            };

            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnDisplayNameUpdateError);
            nameWindow.SetActive(false);
        }


    }
    
    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated display name!");
       
    }
    void OnError(PlayFabError error)
    {
        noInternetpanel.SetActive(true);
        OnlineStatusText.text = "Offline";
        Debug.Log("Error while logging in / creating account!");
        Debug.Log(error.GenerateErrorReport());
    }
    void OnDisplayNameUpdateError(PlayFabError error)
    {
        // show error msga

        noInternetpanel.SetActive(true);
        Debug.Log("Error while logging in / creating account!");
        Debug.Log(error.GenerateErrorReport());
    }
    public void ActivateLeaderboard(bool state)
    {
        leaderboardWindow.SetActive(state);
        //if (ison)
        //{
        //    leaderboardWindow.SetActive(true);



        //    ison = false;
        //}
        //else
        //if (ison == false)
        //{
        //    leaderboardWindow.SetActive(false);
        //  //  GetLeaderboardAroundPlayer();
        //    ison = true;
        //}
    }
  
    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successful Leaderboard sent");
    }
    void OnLeaderboardUpdateError(UpdatePlayerStatisticsResult result)
    {
        Debug.Log(result);
    }
    public void GetLeaderboard()
    {
        // to prevent Throttling
        if (Time.time - LoggedInManager.Instance.LastCallsTime < LoggedInManager.Instance.LeaderboardApiCallInterval)
        {
            return;
        }
        LoggedInManager.Instance.LastCallsTime = Time.time;


        var request = new GetLeaderboardRequest
        {
            StatisticName = "PlatformScore",
            StartPosition = 0,
            MaxResultsCount = 5
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnLeaderBoardGetError);
    }
    void GetLeaderboardOnStart()
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "PlatformScore",
            MaxResultsCount = 5
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardAroundPlayerGet, OnLeaderBoardGetError);
    }
    public void GetLeaderboardAroundPlayer()
    {
        // to prevent Throttling
        if (Time.time - LoggedInManager.Instance.LastCallsTime < LoggedInManager.Instance.LeaderboardApiCallInterval)
        {
            return;
        }
        LoggedInManager.Instance.LastCallsTime = Time.time;
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "PlatformScore",
            MaxResultsCount = 5
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardAroundPlayerGet, OnLeaderBoardGetError);
    }
    // for getting the top players
    void OnLeaderboardGet(GetLeaderboardResult result)
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


    void OnLeaderBoardGetError(PlayFabError err)
    {
        Debug.Log("Leadeboard get api rate error");
        if (err.Error == PlayFabErrorCode.Unknown && err.RetryAfterSeconds > 0)
        {
            Invoke(nameof(GetLeaderboardAroundPlayer), (float)err.RetryAfterSeconds);
            // Retry fxn
        }

    }

    void OnLeaderBoardGetArroundPlayerError(PlayFabError err)
    {

    }
    public void RefreshLogin()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        LoggedInManager.Instance.isLoggedIn = false;
    }



    // Logging in registering using Username and PassWord
    //[Header("UI")]
    //public Text MessageText;
    //public InputField emailInput;
    //public InputField passWordInput;

    //public void RegisterButton()
    //{
    //    if (passWordInput.text.Length < 6)
    //    {
    //        MessageText.text = "Password too short!";
    //        return;
    //    }

    //    var request = new RegisterPlayFabUserRequest
    //    {
    //        Email = emailInput.text,
    //        Password = passWordInput.text,
    //        RequireBothUsernameAndEmail = false
    //    };
    //    PlayFabClientAPI.RegisterPlayFabUser (request,OnRegisterSuccess,OnErrorRegister);
    //}
    //void OnRegisterSuccess(RegisterPlayFabUserResult result)
    //{
    //    MessageText.text = "Registered and Logged in!";
    //}
    //public void LoginButton()
    //{
    //    var request = new LoginWithEmailAddressRequest
    //    {
    //        Email = emailInput.text,
    //        Password = passWordInput.text
    //    };
    //    PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnErrorRegister);
    //}
    //void OnLoginSuccess( LoginResult result)
    //{
    //    MessageText.text = "Logged In!";
    //    Debug.Log("Successful login / account created!");
    //}
    //public void ResetPassWordButton()
    //{
    //    var request = new SendAccountRecoveryEmailRequest
    //    {
    //        Email = emailInput.text,
    //        TitleId = "" // input this later

    //    };
    //    PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnErrorRegister);
    //}
    //void OnPasswordReset(SendAccountRecoveryEmailResult result)
    //{
    //    MessageText.text = "Password reset mail sent!";
    //}
    //void OnErrorRegister( PlayFabError error)
    //{
    //    MessageText.text = error.ErrorMessage;
    //    Debug.Log(error.GenerateErrorReport());
    //}
}
