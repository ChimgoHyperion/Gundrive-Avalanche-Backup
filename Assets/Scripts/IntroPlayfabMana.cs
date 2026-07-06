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
    // NOTE: A lightweight HEAD request is used instead of Application.internetReachability.
    // internetReachability is unreliable on WebGL builds - browsers don't expose real
    // network state to Unity the way native platforms do, so it can report "connected"
    // when the page has no actual route out, or fail to update when connectivity returns.
    private const string ConnectivityCheckUrl = "https://clients3.google.com/generate_204";
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
        while (true)
        {
            yield return StartCoroutine(CheckRealConnectivity(connected =>
            {
                hasInternetConnection = connected;
                UpdateOnlineStatusUI(connected);
            }));

            if (hasInternetConnection && !loginAttempted && !LoggedInManager.Instance.isLoggedIn)
            {
                loginAttempted = true;
                StartCoroutine(Login());
            }

            yield return new WaitForSeconds(ConnectivityPollInterval);
        }
    }

    private IEnumerator CheckRealConnectivity(System.Action<bool> onResult)
    {
        using (UnityWebRequest request = UnityWebRequest.Head(ConnectivityCheckUrl))
        {
            request.timeout = 5;
            yield return request.SendWebRequest();

            bool connected = request.result == UnityWebRequest.Result.Success;
            onResult?.Invoke(connected);
        }
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
        noInternetPanel.SetActive(true);
        onlineStatusText.text = "Offline";
        loginAttempted = false; // allow the connectivity loop to retry later
        Debug.LogWarning("Error while logging in / creating account: " + error.GenerateErrorReport());
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