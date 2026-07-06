using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles automatic PlayFab login on scene start, display-name setup,
/// and leaderboard fetch/display. Also monitors connectivity so UI can
/// reflect online/offline state.
/// </summary>
public class IntroPlayfabMana : MonoBehaviour
{
    [Header("Windows")]
    [SerializeField] private GameObject nameWindow;
    [SerializeField] private GameObject leaderboardWindow;
    [SerializeField] private GameObject loggingInPanel;
    [SerializeField] private GameObject noInternetPanel;

    [Header("Display Name")]
    [SerializeField] private GameObject nameError;
    [SerializeField] private InputField nameInput;

    [Header("Leaderboard")]
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private Transform rowsParent;

    [Header("Status UI")]
    [SerializeField] private TextMeshProUGUI onlineStatusText;
    [SerializeField] private TextMeshProUGUI currentNameText;
    [SerializeField] private Button changeNameButton;
    [SerializeField] private Button leaderboardButton;

    [Header("Auth")]
    public string AuthID = null;

    // --- Connectivity ---
    // We can't independently "ping" for connectivity before authenticating: the
    // PlayFab SDK throws a client-side "Must be logged in" guard on nearly every
    // ClientAPI call pre-session, and pinging an external domain (e.g. Google) fails
    // in WebGL builds due to CORS - third-party domains don't send an
    // Access-Control-Allow-Origin header, so the browser blocks it regardless of
    // host. Application.internetReachability is also unreliable on WebGL, since the
    // browser sandbox doesn't expose real network state. So the login attempt
    // itself doubles as the connectivity check (see ConnectivityMonitorLoop/Login).
    private const float ConnectivityPollInterval = 5f;

    private bool hasInternetConnection;
    private string loggedInPlayFabId;
    private bool loginAttempted;

    #region Unity Lifecycle

    private void Start()
    {
        StartCoroutine(ConnectivityMonitorLoop());
    }

    #endregion

    #region Connectivity

    /// <summary>
    /// Continuously polls real connectivity (not just Unity's reachability enum),
    /// updates status UI, and kicks off login the first time we detect we're online.
    /// </summary>
    private IEnumerator ConnectivityMonitorLoop()
    {
        // We can't ping PlayFab for connectivity before logging in - the SDK throws
        // "Must be logged in to call this method" as a client-side guard on nearly
        // every ClientAPI call, even ones like GetTime that don't require a session
        // server-side. So instead, the login attempt itself is the connectivity
        // check: success means online, failure (for connectivity reasons) means
        // offline, and we retry on a timer until it succeeds.
        while (!LoggedInManager.Instance.isLoggedIn)
        {
            if (!loginAttempted)
            {
                loginAttempted = true;
                StartCoroutine(Login());
            }

            yield return new WaitForSeconds(ConnectivityPollInterval);
        }

        hasInternetConnection = true;
        UpdateOnlineStatusUI(true);
    }

    private void UpdateOnlineStatusUI(bool connected)
    {
        onlineStatusText.text = connected ? "Online" : "Offline";
        onlineStatusText.color = connected ? Color.green : Color.red;

        changeNameButton.interactable = connected;
        leaderboardButton.interactable = connected;

        // Fixed: previously this branch called SetActive(false) on no connection,
        // hiding the "no internet" panel exactly when it should be shown.
        noInternetPanel.SetActive(!connected);
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RefreshLogin()
    {
        LoggedInManager.Instance.isLoggedIn = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion

    #region Login

    private IEnumerator Login()
    {
        if (LoggedInManager.Instance.isLoggedIn)
            yield break;

        loggingInPanel.SetActive(true);

        yield return new WaitUntil(() => UnityAuthenticationManager.Instance.IDGotten);

        string customId = AuthID;

        var request = new LoginWithCustomIDRequest
        {
            CustomId = customId,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        LoggedInManager.Instance.isLoggedIn = true;
        loggedInPlayFabId = result.PlayFabId;
        loggingInPanel.SetActive(false);

        onlineStatusText.text = "Online";
        changeNameButton.interactable = true;
        leaderboardButton.interactable = true;

        Debug.Log("Successful login/account created!");

        string displayName = result.InfoResultPayload?.PlayerProfile?.DisplayName;

        if (!string.IsNullOrEmpty(displayName))
        {
            PlayerPrefs.SetString("PlayerNickname", displayName);
            currentNameText.text = displayName;
            nameWindow.SetActive(false);
        }
        else
        {
            nameWindow.SetActive(true);
        }

        GetLeaderboardAroundPlayer();
        CoinBalanceHolder.Instance.GetInventoryCoinBalance();
    }

    private void OnLoginError(PlayFabError error)
    {
        Debug.LogWarning("Error while logging in / creating account: " + error.GenerateErrorReport());

        // HttpCode 0 / ConnectionError / ServiceUnavailable indicate the request never
        // actually reached PlayFab or PlayFab itself is down - that's a real connectivity
        // issue. Anything else (e.g. disabled API features, bad params) is a config/API
        // error and should NOT be reported as "no internet".
        bool isConnectivityIssue = error.HttpCode == 0
            || error.Error == PlayFabErrorCode.ConnectionError
            || error.Error == PlayFabErrorCode.ServiceUnavailable;

        if (isConnectivityIssue)
        {
            noInternetPanel.SetActive(true);
            onlineStatusText.text = "Offline";
            loginAttempted = false; // safe to retry once connectivity actually returns
        }
        else
        {
            // A real API/config error (e.g. "Player creations have been disabled for
            // this API"). Retrying on a timer just spams PlayFab with the same failure -
            // surface it instead of looping the offline panel.
            Debug.LogError("Login failed for a non-connectivity reason - check PlayFab title settings " +
                "(e.g. 'Allow client to automatically create new players'). Will not auto-retry.");
        }
    }

    #endregion

    #region Display Name

    public void SubmitNameButton()
    {
        string name = nameInput.text;

        if (name.Length < 1 || name.Length >= 15)
        {
            if (nameError != null)
                nameError.SetActive(true);
            return;
        }

        PlayerPrefs.SetString("PlayerNickname", name);

        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdateSuccess, OnDisplayNameUpdateError);
        nameWindow.SetActive(false);
    }

    private void OnDisplayNameUpdateSuccess(UpdateUserTitleDisplayNameResult result)
    {
        currentNameText.text = result.DisplayName;
        Debug.Log("Updated display name!");
    }

    private void OnDisplayNameUpdateError(PlayFabError error)
    {
        noInternetPanel.SetActive(true);
        Debug.LogWarning("Error while updating display name: " + error.GenerateErrorReport());
    }

    #endregion

    #region Leaderboard

    public void ActivateLeaderboard(bool state)
    {
        leaderboardWindow.SetActive(state);
    }

    public void GetLeaderboardTop()
    {
        if (!CanCallLeaderboardApi()) return;

        var request = new GetLeaderboardRequest
        {
            StatisticName = "PlatformScore",
            StartPosition = 0,
            MaxResultsCount = 5
        };

        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnLeaderboardGetError);
    }

    public void GetLeaderboardAroundPlayer()
    {
        if (!CanCallLeaderboardApi()) return;

        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "PlatformScore",
            MaxResultsCount = 5
        };

        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardAroundPlayerGet, OnLeaderboardGetError);
    }

    private bool CanCallLeaderboardApi()
    {
        if (Time.time - LoggedInManager.Instance.LastCallsTime < LoggedInManager.Instance.LeaderboardApiCallInterval)
            return false;

        LoggedInManager.Instance.LastCallsTime = Time.time;
        return true;
    }

    private void OnLeaderboardGet(GetLeaderboardResult result)
    {
        PopulateLeaderboardRows(result.Leaderboard);
    }

    private void OnLeaderboardAroundPlayerGet(GetLeaderboardAroundPlayerResult result)
    {
        PopulateLeaderboardRows(result.Leaderboard);
    }

    private void PopulateLeaderboardRows(System.Collections.Generic.List<PlayerLeaderboardEntry> entries)
    {
        foreach (Transform child in rowsParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var entry in entries)
        {
            GameObject row = Instantiate(rowPrefab, rowsParent);
            Text[] texts = row.GetComponentsInChildren<Text>();

            if (texts.Length < 3)
            {
                Debug.LogWarning("Leaderboard row prefab is missing expected Text fields.");
                continue;
            }

            texts[0].text = (entry.Position + 1).ToString();
            texts[1].text = entry.DisplayName;
            texts[2].text = entry.StatValue.ToString();

            bool isLocalPlayer = entry.PlayFabId == loggedInPlayFabId;
            Color rowColor = isLocalPlayer ? Color.cyan : texts[0].color;
            texts[0].color = rowColor;
            texts[1].color = rowColor;
            texts[2].color = rowColor;
        }
    }

    private void OnLeaderboardGetError(PlayFabError error)
    {
        Debug.LogWarning("Leaderboard API rate/error: " + error.GenerateErrorReport());

        if (error.Error == PlayFabErrorCode.Unknown && error.RetryAfterSeconds > 0)
        {
            Invoke(nameof(GetLeaderboardAroundPlayer), (float)error.RetryAfterSeconds);
        }
    }

    #endregion
}