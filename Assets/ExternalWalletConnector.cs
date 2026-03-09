// ExternalWalletConnector.cs
// Drop this on a GameObject in your scene.
// Requires: ThirdwebManager prefab in scene with your Client ID set.

using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Thirdweb;
using Thirdweb.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExternalWalletConnector : MonoBehaviour
{
    // ?????????????????????????????????????????????
    //  Inspector Fields — drag your UI here
    // ?????????????????????????????????????????????
    [Header("Chain")]
    [Tooltip("80002 = Polygon Amoy (testnet) | 137 = Polygon Mainnet")]
    public ulong ChainId = 43113;

    [Header("UI References")]
    public Button ConnectButton;
    public Button DisconnectButton;
    public TextMeshProUGUI StatusText;      // shows address or status messages
    public GameObject ConnectedPanel,ConnectWalletbtnObj;      // shown when wallet is connected
    public GameObject DisconnectedPanel,DisconnectWalletbtnObj;   // shown when wallet is NOT connected
    
    // ?????????????????????????????????????????????
    //  Wallet IDs for the Reown modal
    //  (these are fixed IDs assigned by Reown/WalletConnect)
    // ?????????????????????????????????????????????
    private static readonly string METAMASK_ID = "c57ca95b47569778a828d19178114f4db188b89b763c899ba0be274e97267d96";
    private static readonly string RAINBOW_ID = "18388be9ac2d02726dbac9777c96efaac06d744b2f6d580fccdd4127a6d01fd1";
    private static readonly string TRUST_ID = "541d5dcd4ede02f3afaf75bf8e3e4c4f1fb09edb5fa6c4377ebf31c2785d9adf";

    // ?????????????????????????????????????????????
    //  Unity Lifecycle
    // ?????????????????????????????????????????????
    private void Start()
    {
        // Wire up buttons (you can also do this in the Inspector via OnClick)
        ConnectButton.onClick.AddListener(ConnectWallet);
        DisconnectButton.onClick.AddListener(DisconnectWallet);

        ShowDisconnectedState();
    }
    private void Update()
    {
       
    }
    // ?????????????????????????????????????????????
    //  PUBLIC: Call from button or other scripts
    // ?????????????????????????????????????????????

    /// <summary>
    /// Opens the Reown modal so the player can pick MetaMask, Rainbow, Trust, etc.
    /// </summary>
    public async void ConnectWallet()
    {
        SetStatus("Opening wallet selector...");

        try
        {
            var options = new WalletOptions(
                provider: WalletProvider.ReownWallet,   // ? THIS is what makes it work (not WalletConnect)
                chainId: ChainId,
                reownOptions: new ReownOptions(
                    projectId: null,             // uses ThirdwebManager's built-in project ID
                    name: null,
                    description: null,
                    url: null,
                    iconUrl: null,
                    includedWalletIds: null,
                    excludedWalletIds: null,
                    // These wallets are shown first in the modal
                    featuredWalletIds: new string[]
                    {
                        METAMASK_ID,
                        RAINBOW_ID,
                        TRUST_ID,
                    }
                )
            );

            // ConnectWallet stores the result in ThirdwebManager.Instance.ActiveWallet automatically
            var wallet = await ThirdwebManager.Instance.ConnectWallet(options);

            string address = await wallet.GetAddress();

            PlayerPrefs.SetString("ConnectedWalletAddress", address);
            ShowConnectedState(address);
        }
        catch (Exception e)
        {
            SetStatus("Connection cancelled or failed.");
            Debug.LogError($"[WalletConnector] {e.Message}");
        }
    }

    /// <summary>
    /// Disconnects the wallet and resets the UI.
    /// </summary>
    public async void DisconnectWallet()
    {
        try
        {
            if (ThirdwebManager.Instance.ActiveWallet != null)
            {
                await ThirdwebManager.Instance.ActiveWallet.Disconnect();
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[WalletConnector] Disconnect warning: {e.Message}");
        }
        finally
        {
            ShowDisconnectedState();
        }
    }

    // ?????????????????????????????????????????????
    //  Convenience property — use this in OTHER scripts
    //  e.g.  ExternalWalletConnector.ActiveWallet
    // ?????????????????????????????????????????????

    /// <summary>
    /// Quick access to the active wallet from any other script.
    /// Usage:  var wallet = ExternalWalletConnector.ActiveWallet;
    /// </summary>
    public static IThirdwebWallet ActiveWallet => ThirdwebManager.Instance?.ActiveWallet;

    /// <summary>
    /// Returns true if a wallet is currently connected.
    /// </summary>
    public static bool IsConnected => ThirdwebManager.Instance?.ActiveWallet != null;

    // ?????????????????????????????????????????????
    //  UI Helpers
    // ?????????????????????????????????????????????
    private void ShowConnectedState(string address)
    {
        // Shorten address:  0x1234...abcd
        //string shortAddress = $"{address[..6]}...{address[^4..]}";
        //SetStatus($"Connected:\n{shortAddress}");

        SetStatus($"{address}");
        ConnectedPanel?.SetActive(true);
        DisconnectedPanel?.SetActive(false);

        ConnectWalletbtnObj.SetActive(false);
        DisconnectWalletbtnObj.SetActive(true);


        FindObjectOfType<TokenBalanceDisplay>().ButtonViewBalance();
    }

    private void ShowDisconnectedState()
    {
        SetStatus("No wallet connected.");
        ConnectedPanel?.SetActive(false);
        DisconnectedPanel?.SetActive(true);

        ConnectWalletbtnObj.SetActive(true);
        DisconnectWalletbtnObj.SetActive(false);
    }

    private void SetStatus(string message)
    {
        if (StatusText != null)
            StatusText.text = message;

        Debug.Log($"[WalletConnector] {message}");
    }


   
}
