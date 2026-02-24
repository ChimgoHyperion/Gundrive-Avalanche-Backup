using UnityEngine;
using Reown.AppKit.Unity;
using System.Threading.Tasks;
using Nethereum.Web3;
using Reown.Sign.Models;
using System.Numerics;
using System;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;
using Nethereum.Hex.HexTypes;
using System.Linq;
using UnityEngine.Purchasing.MiniJSON;
/// <summary>
/// Web3Manager: Singleton edition with simple Pinata IPFS uploader + ERC721 mint helper.
/// - Configure Pinata API keys, ERC721 contract address and ABI in inspector.
/// - MintNftAsync uploads image + metadata to IPFS (Pinata) then calls contract.mint(to, tokenURI).
/// </summary>
public class Web3Manager : MonoBehaviour
{
    // --- Singleton
    public static Web3Manager Instance { get; private set; }
    // Replace with your Cloudinary details
    public string cloudName = "norvirae";
    public string uploadPreset = "your_unsigned_upload_preset";
    public string public_id = "your_unsigned_upload_preset";

    public string api_key = "";

    // OR if using API key/secret you’ll need signed upload


    // --- Existing fields (kept)
    private TextAsset abiFile;

    [Header("UI Elements")]
    public UnityEngine.UI.Button oneDollar;
    public UnityEngine.UI.Button connectButton;
    public UnityEngine.UI.Button threeDollar;
    public UnityEngine.UI.Button sevenDollar;

    public UnityEngine.UI.Button claimButton; // Add this
    public TextMeshProUGUI claimBalanceText;
    public GameObject ClaimSuccessUI; // Add this
    public GameObject ClaimFailUI;
    public GameObject PurchaseSuccessUI;
    public GameObject PurchaseFailUI;
    public TextMeshProUGUI CoinBalanceText;

    [Header("Prices")]
    public string firstPrice = "1.99";
    public string secondPrice = "3.99";
    public string thirdPrice = "7.99";
    public float claimAmount = 100f;
    public float oneGundEquals = 0.000667f;// 1 Gund => 0.000667f

    [Header("Contract Addresses")]
    private string erc721ContractAddressNFT = "0xF09aC86286Ee1270ABdf8a8Ec7fC32de1Ab01cE3";
    private const string tokenContractAddress = "0x8A4B3FC60024fAcd4c665C0a0C15Abd26Bc079Af";
    private const string marketplaceAddress = "0xFaD217d48BdFc58d62B7b46b936810B48790aF71";

    [Header("Marketplace Wallet (for Claims)")]
    [Tooltip("Private key of the marketplace wallet that holds GUND tokens for rewards")]
    public string marketplacePrivateKey = ""; // Store securely!
    [Tooltip("Address of the marketplace wallet")]
    public string marketplaceWalletAddress = "0xFaD217d48BdFc58d62B7b46b936810B48790aF71";

    private TextAsset nftAbiFile;
    [Tooltip("The ERC721 contract address that implements mint(to, tokenURI)")]

    private string pinataApiKey = "17392ddea9c8fe0db97a";
    [Tooltip("Pinata API Secret Key")]
    private string pinataSecretApiKey = "0c9ab5672035523ecaee564a39fbb12ee735fef830bb74be64837dc685eb3c09";

    private Account? account; // Make it nullable since Account is a struct
    private bool isWalletConnected = false;
    private bool isInitialized = false;

    [Serializable] public class AlchemyId { public string tokenId; }
    [Serializable] public class AlchemyMetadata { public string name; public string description; public string image; }
    [Serializable] public class AlchemyMedia { public string gateway; }
    public string alchemyKey = "key";


    [Serializable]
    private class AlchemyV3Response
    {
        public AlchemyNft[] ownedNfts;
        public string pageKey;
    }

    [Serializable]
    public class AlchemyNft
    {
        public AlchemyContract contract;
        public string tokenId;
        public string tokenType;
        public string name;
        public string description;
        public string tokenUri;
        public AlchemyImage image;
        public AlchemyRaw raw;
        public string balance;
        public string timeLastUpdated;
        // keep other fields you may want; JsonUtility will ignore missing ones
    }

    [Serializable]
    public class AlchemyContract
    {
        public string address;
        public string name;
        public string symbol;
        public string tokenType;
    }
    [Serializable]
    public class AlchemyImage
    {
        public string cachedUrl;
        public string thumbnailUrl;
        public string pngUrl;
        public string originalUrl;
        public string contentType;
        public long size;
    }
    [Serializable] public class AlchemyRaw { public object metadata; public string tokenUri; public string error; }

    // Add this class to store NFT data
    [Serializable]
    public class UserNftData
    {
        public uint tokenId;
        public string name;
        public string description;
        public string imageUrl;
        public string tokenUri;
    }
    // Add these serializable classes at the top of your Web3Manager class
    [System.Serializable]
    public class NftMetadata
    {
        public string name;
        public string description;
        public string image;
        public NftAttribute[] attributes;
    }

    [System.Serializable]
    public class NftAttribute
    {
        public string trait_type;
        public string value;
    }

    private List<SimpleNftData> _userNfts = new List<SimpleNftData>();


    public async void Start()
    {
        try
        {
            await GDNFTFetchers.InitializeWeb3();
            await GDNFTFetchers.FetchAllTokensForOwner("0x19219ab0E7DBbA5B887A1f2Dc6EC3C0D10576628");
            GDNftFetcher.SetAlchemyKey(alchemyKey);
            // await GDNftFetcher.GetGDNftsAsync("0x19219ab0E7DBbA5B887A1f2Dc6EC3C0D10576628");

            // var nfts = await FetchUserNftsAsync("0x19219ab0E7DBbA5B887A1f2Dc6EC3C0D10576628");
            // Debug.Log(Json.Serialize(nfts) + " NTFTa");

            await InitializeWeb3();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to initialize Web3: {ex.Message}");
        }
    }

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // DontDestroyOnLoad(gameObject);
    }


    // --- Rest of your original lifecycle / UI logic (kept mostly intact)
    void Update()
    {
        if (CoinBalanceText != null)
            CoinBalanceText.text = "Coins :" + CoinBalanceHolder.Instance.virtualCurrencyBalance.ToString();

        if (claimBalanceText != null)
            claimBalanceText.text = (CoinBalanceHolder.Instance.virtualCurrencyBalance * oneGundEquals).ToString() + " GUND";
        updateConnectButton();
    }

    // Add this function to your Web3Manager class
    public async Task<List<UserNftData>> FetchUserNftsAsync(string userAddress = null)
    {
        List<UserNftData> userNfts = new List<UserNftData>();

        if (!IsReadyForTransactions())
        {
            Debug.LogError("Not ready for transactions (init/wallet).");
            return userNfts;
        }

        if (string.IsNullOrEmpty(erc721ContractAddressNFT) || nftAbiFile == null)
        {
            Debug.LogError("ERC721 contract address or ABI file is not set.");
            return userNfts;
        }

        try
        {
            // Use connected account address if none provided
            if (string.IsNullOrEmpty(userAddress))
            {
                userAddress = account.Value.Address;
            }

            string abi = nftAbiFile.text;

            // Get the number of NFTs owned by the user
            var balance = await AppKit.Evm.ReadContractAsync<BigInteger>(
                erc721ContractAddressNFT,
                abi,
                "tokensOfOwner",
                new object[] { userAddress }
            );
            Debug.Log($"User owns {balance} NFTs");


            int nftCount = (int)balance;
            Debug.Log($"User owns {nftCount} NFTs");

            if (nftCount == 0)
            {
                Debug.Log("User owns no NFTs");
                return userNfts;
            }

            // Fetch each NFT's details
            for (int i = 0; i < nftCount; i++)
            {
                try
                {
                    // Get tokenId by index
                    var tokenId = await AppKit.Evm.ReadContractAsync<BigInteger>(
                        erc721ContractAddressNFT,
                        abi,
                        "tokenOfOwnerByIndex",
                        new object[] { userAddress, i }
                    );

                    // Get tokenURI for this tokenId
                    var tokenUri = await AppKit.Evm.ReadContractAsync<string>(
                        erc721ContractAddressNFT,
                        abi,
                        "tokenURI",
                        new object[] { tokenId }
                    );

                    Debug.Log($"Token {i}: ID={tokenId}, URI={tokenUri}");

                    // Fetch and parse metadata
                    var nftData = await FetchNftMetadataAsync((uint)tokenId, tokenUri);
                    if (nftData != null)
                    {
                        userNfts.Add(nftData);
                        Debug.Log($"Successfully fetched NFT: {nftData.name} (ID: {nftData.tokenId})");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to fetch NFT at index {i}: {ex.Message}");
                    continue; // Skip this NFT and continue with the next one
                }
            }

            Debug.Log($"Successfully fetched {userNfts.Count} NFTs out of {nftCount} total");
            return userNfts;
        }
        catch (Exception ex)
        {
            Debug.LogError($"FetchUserNftsAsync failed: {ex.Message}");
            return userNfts;
        }
    }

    /// <summary>
    /// Creates a Web3 instance with the marketplace wallet's private key
    /// </summary>
    private Web3 GetMarketplaceWeb3()
    {
        if (string.IsNullOrEmpty(marketplacePrivateKey))
        {
            throw new Exception("Marketplace private key not configured!");
        }

        // Get the RPC URL from the current chain
        string rpcUrl = "https://testnet.hashio.io/api"; // Hedera Testnet RPC

        var account = new Nethereum.Web3.Accounts.Account(marketplacePrivateKey);
        var web3 = new Web3(account, rpcUrl);

        return web3;
    }

    // Helper function to fetch and parse NFT metadata
    private async Task<UserNftData> FetchNftMetadataAsync(uint tokenId, string tokenUri)
    {
        if (string.IsNullOrEmpty(tokenUri))
        {
            Debug.LogWarning($"Empty tokenURI for token {tokenId}");
            return null;
        }

        try
        {
            // Handle different URI formats (IPFS, HTTP, etc.)
            // string metadataUrl = ConvertToHttpUrl(tokenUri);
            string metadataUrl = tokenUri;


            using (UnityWebRequest uwr = UnityWebRequest.Get(metadataUrl))
            {
                uwr.timeout = 30;
                await SendRequestAsync(uwr);

                if (uwr.result == UnityWebRequest.Result.Success)
                {
                    string metadataJson = uwr.downloadHandler.text;
                    Debug.Log($"Metadata for token {tokenId}: {metadataJson}");

                    // Parse the metadata JSON
                    var metadata = JsonUtility.FromJson<NftMetadata>(metadataJson);

                    return new UserNftData
                    {
                        tokenId = tokenId,
                        name = metadata.name ?? $"Token #{tokenId}",
                        description = metadata.description ?? "",
                        imageUrl = ConvertToHttpUrl(metadata.image ?? ""),
                        tokenUri = tokenUri
                    };
                }
                else
                {
                    Debug.LogError($"Failed to fetch metadata for token {tokenId}: {uwr.error}");
                    Debug.LogError($"URL: {metadataUrl}");

                    // Return basic info even if metadata fetch fails
                    return new UserNftData
                    {
                        tokenId = tokenId,
                        name = $"Token #{tokenId}",
                        description = "Metadata unavailable",
                        imageUrl = "",
                        tokenUri = tokenUri
                    };
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error fetching metadata for token {tokenId}: {ex.Message}");

            // Return basic info even if metadata fetch fails
            return new UserNftData
            {
                tokenId = tokenId,
                name = $"Token #{tokenId}",
                description = "Error loading metadata",
                imageUrl = "",
                tokenUri = tokenUri
            };
        }
    }

    // Helper function to convert IPFS URLs to HTTP gateway URLs
    private string ConvertToHttpUrl(string uri)
    {
        if (string.IsNullOrEmpty(uri))
            return uri;

        // Convert IPFS URLs to HTTP gateway URLs
        if (uri.StartsWith("ipfs://"))
        {
            string hash = uri.Substring(7); // Remove "ipfs://"
            return $"https://gateway.pinata.cloud/ipfs/{hash}";
            // Alternative gateways you could use:
            // return $"https://ipfs.io/ipfs/{hash}";
            // return $"https://cloudflare-ipfs.com/ipfs/{hash}";
        }

        // Return as-is if already HTTP/HTTPS
        return uri;
    }

    // Public method to call from UI or other components
    public async void RefreshUserNfts()
    {
        if (!IsWalletConnected())
        {
            Debug.LogWarning("Wallet not connected. Cannot fetch NFTs.");
            return;
        }

        Debug.Log("Refreshing user NFTs...");
        var nfts = await FetchUserNftsAsync();

        // You can store these in a class variable or trigger events
        _userNfts.Clear();
        _userNfts.AddRange(nfts.Select(nft => new SimpleNftData
        {
            name = nft.name,
            // Map other properties as needed
        }));

        Debug.Log($"Refreshed NFTs: Found {nfts.Count} NFTs");

        // Optional: Trigger an event or callback to notify UI
        OnUserNftsUpdated?.Invoke(nfts);
    }

    // Optional: Event to notify when NFTs are updated
    public System.Action<List<UserNftData>> OnUserNftsUpdated;

    public List<SimpleNftData> getUserNftSkins()
    {
        return _userNfts;
    }
    public void updateConnectButton()
    {
        if (connectButton == null) return;
        Text buttonText = connectButton.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            if (AppKit.IsAccountConnected)
            {
                buttonText.text = "Disconnect Wallet";

                Image connectButtonImage = connectButton.GetComponent<Image>();
                Color newColor;
                if (ColorUtility.TryParseHtmlString("#20F059", out newColor))
                {
                    connectButtonImage.color = newColor;
                }
            }
            else
            {
                buttonText.text = "Connect Wallet";
                Color textColor;
                if (ColorUtility.TryParseHtmlString("#ffffff", out textColor))
                {
                    buttonText.color = textColor;
                }
                Image connectButtonImage = connectButton.GetComponent<Image>();
                Color newColor;
                if (ColorUtility.TryParseHtmlString("#000000", out newColor))
                {
                    connectButtonImage.color = newColor;
                }
            }
        }
    }

    /// <summary>
    /// Claims GUND tokens by sending them from the marketplace wallet to the user's connected wallet
    /// Uses the marketplace wallet's private key to sign the transaction
    /// Waits for transaction confirmation
    /// </summary>
    /// <returns>Transaction hash if successful, null otherwise</returns>
    public async Task<string> ClaimGUNDTokens()
    {
        if (!AppKit.IsAccountConnected || !account.HasValue)
        {
            Debug.LogError("User wallet not connected!");
            return null;
        }

        if (string.IsNullOrEmpty(marketplacePrivateKey))
        {
            Debug.LogError("Marketplace private key not configured!");
            return null;
        }

        if (CoinBalanceHolder.Instance.virtualCurrencyBalance == 0)
        {
            Debug.LogError("You do not have any coins!");
            return null;
        }

        float amountToSend = CoinBalanceHolder.Instance.virtualCurrencyBalance * oneGundEquals;

        try
        {
            string recipientAddress = account.Value.Address; // User's connected wallet
            Debug.Log($"Claiming {amountToSend} GUND tokens from marketplace to {recipientAddress}");

            // Create Web3 instance with marketplace wallet
            var web3 = GetMarketplaceWeb3();

            // Load the ERC20 contract
            var contract = web3.Eth.GetContract(abiFile.text, tokenContractAddress);

            // Get the transfer function
            var transferFunction = contract.GetFunction("transfer");

            // Get token decimals
            var decimalsFunction = contract.GetFunction("decimals");
            var decimals = await decimalsFunction.CallAsync<int>();

            // Calculate amount in wei
            BigInteger amount = new BigInteger(amountToSend) * BigInteger.Pow(10, decimals);

            Debug.Log($"Sending {amount} tokens (with {decimals} decimals) to {recipientAddress}");

            // Estimate gas
            var gas = await transferFunction.EstimateGasAsync(
                marketplaceWalletAddress,
                null,
                null,
                recipientAddress,
                amount
            );

            Debug.Log($"Estimated gas: {gas}");

            // Create HexBigInteger for gas with buffer
            var gasLimit = new HexBigInteger(gas.Value * 2);

            // Send the transaction and get transaction hash (don't wait for receipt to avoid timeout)
            var txHash = await transferFunction.SendTransactionAsync(
                from: marketplaceWalletAddress,
                gas: gasLimit,
                gasPrice: null,
                value: new HexBigInteger(0),
                functionInput: new object[] { recipientAddress, amount }
            );

            Debug.Log($"GUND Claim transaction sent! Hash: {txHash}");
            Debug.Log($"Sending {amountToSend} GUND tokens from marketplace to {recipientAddress}");

            return txHash;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to claim GUND tokens: {ex.Message}");
            Debug.LogError($"Stack trace: {ex.StackTrace}");
            return null;
        }
    }


    /// <summary>
    /// Checks the GUND token balance of the marketplace wallet
    /// </summary>
    public async Task<float> GetMarketplaceGUNDBalance()
    {
        try
        {
            var web3 = GetMarketplaceWeb3();
            var contract = web3.Eth.GetContract(abiFile.text, tokenContractAddress);

            var balanceFunction = contract.GetFunction("balanceOf");
            var decimalsFunction = contract.GetFunction("decimals");

            var balance = await balanceFunction.CallAsync<BigInteger>(marketplaceWalletAddress);
            var decimals = await decimalsFunction.CallAsync<int>();
            Debug.Log($"LUMA {balance}");

            float finalBalance = (float)(balance / BigInteger.Pow(10, decimals));

            Debug.Log($"Marketplace wallet GUND balance: {finalBalance}");
            return finalBalance;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to get marketplace balance: {ex.Message}");
            return 0f;
        }
    }

    private IEnumerator HandleClaimTokens()
    {
        if (!AppKit.IsAccountConnected || !account.HasValue)
        {
            Debug.LogError("Wallet not connected!");
            if (ClaimFailUI != null) ClaimFailUI.SetActive(true);
            yield break;
        }

        // Check marketplace wallet balance first
        var balanceTask = GetMarketplaceGUNDBalance();
        yield return new WaitUntil(() => balanceTask.IsCompleted);

        float marketplaceBalance = balanceTask.Result;
        float requiredAmount = CoinBalanceHolder.Instance.virtualCurrencyBalance * oneGundEquals;

        if (marketplaceBalance < requiredAmount)
        {
            Debug.LogError($"Insufficient marketplace balance. Required: {requiredAmount}, Available: {marketplaceBalance}");
            if (ClaimFailUI != null) ClaimFailUI.SetActive(true);
            yield break;
        }

        // Disable claim button during transaction
        if (claimButton != null) claimButton.interactable = false;

        bool claimResult = false;
        yield return StartCoroutine(ClaimGUNDTokensCoroutine(result => claimResult = result));

        // Re-enable claim button
        if (claimButton != null) claimButton.interactable = true;

        // Show appropriate UI
        if (claimResult)
        {
            if (ClaimSuccessUI != null) ClaimSuccessUI.SetActive(true);
            Debug.Log($"Successfully claimed {requiredAmount} GUND tokens!");

            // Reset virtual currency balance after successful claim
            CoinBalanceHolder.Instance.SubtractVirtualCurrency(CoinBalanceHolder.Instance.virtualCurrencyBalance);
        }
        else
        {
            if (ClaimFailUI != null) ClaimFailUI.SetActive(true);
            Debug.LogError("Failed to claim GUND tokens");
        }
    }
    /// <summary>
    /// Coroutine wrapper for claiming GUND tokens (for UI button usage)
    /// </summary>
    /// <summary>
    /// Coroutine wrapper for claiming GUND tokens (for UI button usage)
    /// </summary>
    public IEnumerator ClaimGUNDTokensCoroutine(System.Action<bool> callback)
    {
        var task = ClaimGUNDTokens();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError($"Claim failed: {task.Exception.GetBaseException().Message}");
            callback(false);
        }
        else
        {
            bool success = !string.IsNullOrEmpty(task.Result);
            callback(success);
        }
    }


    private async Task InitializeWeb3()
    {
        // Load ABI file (your ERC20 ABI for balance/transfer)
        abiFile = Resources.Load<TextAsset>("erc20.abi");
        if (abiFile == null)
        {
            Debug.LogError("Failed to load erc20.abi from Resources folder!");
            return;
        }

        nftAbiFile = Resources.Load<TextAsset>("erc721.abi");
        if (nftAbiFile == null)
        {
            Debug.LogError("Failed to load erc20.abi from Resources folder!");
            return;
        }

        try
        {
            if (!AppKit.IsInitialized)
            {

                // Initialize AppKit with proper error handling
                await AppKit.InitializeAsync(new AppKitConfig
                {
                    projectId = "9f997d84b749cc6d47d8be7b1d1a4fd8",
                    metadata = new Reown.AppKit.Unity.Metadata(
                        name: "Gun Drive",
                        description: "Shooter Game",
                        url: "https://blackhards.com",
                        iconUrl: "https://www.blackhards.com/images/Blackhards-white-logo.svg"
                    ),
                    supportedChains = new[]
                    {
                        new Chain(ChainConstants.Namespaces.Evm,
                            chainReference: "296",
                            name: "Hedera Testnet",
                            nativeCurrency: new Currency("Hedera", "HBAR", 18),
                            blockExplorer: new BlockExplorer("HashScan", "https://hashscan.io/testnet"),
                            rpcUrl: "https://testnet.hashio.io/api",
                            isTestnet: true,
                            imageUrl: "https://hedera.com/assets/images/hbar-logo.png"
                        ),

                }
                });

                isInitialized = true;
                Debug.Log("AppKit initialized successfully!");
                // var nfts = await FetchUserNftsAsync("0x19219ab0E7DBbA5B887A1f2Dc6EC3C0D10576628");

                // Set up event handlers
                AppKit.AccountConnected += async (sender, eventArgs) =>
                {
                    MyAccountConnectedHandler(eventArgs.Account);
                    Debug.Log("Account Connected Successfully!..");
                    _userNfts = await GDNFTFetchers.FetchAllTokensForOwner(eventArgs.Account.Address);
                    // _userNfts = await GDNftFetcher.GetGDNftsAsync(eventArgs.Account.Address ?? "0x19219ab0E7DBbA5B887A1f2Dc6EC3C0D10576628");

                    // await FetchFilteredGDNftsForOwnerAlchemyV3Async(eventArgs.Account.Address ?? "0x19219ab0E7DBbA5B887A1f2Dc6EC3C0D10576628");

                };

                AppKit.AccountDisconnected += (sender, eventArgs) =>
                {
                    MyAccountDisconnectedHandler();
                };
            }

            // Try to resume session
            await ResumeSession();

            // Initialize buttons after everything is set up
            InitializeBuyButtons();
            ConnectWallet();
        }
        catch (Exception ex)
        {
            Debug.LogError($"AppKit initialization failed: {ex.Message}");
            isInitialized = false;
        }
    }

    private void OnAccountConnected(object sender, Connector.AccountConnectedEventArgs e)
    {
        MyAccountConnectedHandler(e.Account);
    }

    private void OnAccountDisconnected(object sender, Connector.AccountDisconnectedEventArgs e)
    {
        MyAccountDisconnectedHandler();
    }

    public void InitializeBuyButtons()
    {
        if (oneDollar == null || threeDollar == null || sevenDollar == null)
        {
            Debug.LogError("Please attach all Buy buttons in the inspector!");
            return;
        }

        // Clear any existing listeners
        oneDollar.onClick.RemoveAllListeners();
        threeDollar.onClick.RemoveAllListeners();
        sevenDollar.onClick.RemoveAllListeners();

        // Add new listeners
        oneDollar.onClick.AddListener(() => StartCoroutine(HandlePurchase(firstPrice, 3000)));
        threeDollar.onClick.AddListener(() => StartCoroutine(HandlePurchase(secondPrice, 8000)));
        sevenDollar.onClick.AddListener(() => StartCoroutine(HandlePurchase(thirdPrice, 20000)));

        // Add claim button listener if assigned
        if (claimButton != null)
        {
            claimButton.onClick.RemoveAllListeners();
            claimButton.onClick.AddListener(() => StartCoroutine(HandleClaimTokens()));
        }

        // Attach small UI feedback components if you have them
        AttachOrConfigureFeedback(oneDollar, 0.92f, true);
        AttachOrConfigureFeedback(threeDollar, 0.92f, true);
        AttachOrConfigureFeedback(sevenDollar, 0.92f, true);
        AttachOrConfigureFeedback(connectButton, 0.92f, true);
        AttachOrConfigureFeedback(claimButton, 0.92f, true); // Add this
    }

    private void AttachOrConfigureFeedback(UnityEngine.UI.Button btn, float pressScale = 0.92f, bool enableTint = true)
    {
        if (btn == null) return;
        var fb = btn.gameObject.GetComponent<UIButtonFeedback>();
        if (fb == null) fb = btn.gameObject.AddComponent<UIButtonFeedback>();
        fb.pressScale = pressScale;
        fb.scaleDuration = 0.08f;
        fb.enableTint = enableTint;
        fb.clickTint = new Color(0.16f, 0.63f, 1f, 1f); // tweak color to taste
        fb.tintDuration = 0.15f;
        fb.tintHold = 0.08f;
    }

    private IEnumerator HandlePurchase(string price, int virtualCurrency)
    {

        if (!AppKit.IsAccountConnected || !account.HasValue)
        {
            Debug.LogError("Wallet not connected!");
            yield break;
        }

        // Disable buttons during purchase
        SetButtonsInteractable(false);

        bool purchaseResult = false;
        yield return StartCoroutine(ProcessPurchaseCoroutine(price, virtualCurrency, result => purchaseResult = result));

        // Re-enable buttons
        SetButtonsInteractable(true);

        // Show appropriate UI
        if (purchaseResult)
        {
            PurchaseSuccessUI?.SetActive(true);
        }
        else
        {
            PurchaseFailUI?.SetActive(true);
        }
    }

    private IEnumerator ProcessPurchaseCoroutine(string price, int virtualCurrency, System.Action<bool> callback)
    {
        var task = ProcessPurchaseAsync(price, virtualCurrency);
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError($"Purchase failed: {task.Exception.GetBaseException().Message}");
            callback(false);
        }
        else
        {
            callback(task.Result);
        }
    }

    private async Task<bool> ProcessPurchaseAsync(string price, int virtualCurrency)
    {
        try
        {
            int bal = await GetERC20TokenBal(account.Value.Address);

            if (!decimal.TryParse(price, out decimal priceDecimal))
            {
                Debug.LogError($"Price is not a valid number: {price}");
                return false;
            }

            float bundleAmount = (float)priceDecimal; // Convert to float for token comparison

            if (bal < bundleAmount)
            {
                return false;
            }


            // Send the ERC20 token transaction
            string transactionHash = await SendERC20Token(bundleAmount);

            if (string.IsNullOrEmpty(transactionHash))
            {
                Debug.LogError("Transaction failed - no hash returned");
                return false;
            }

            // Add virtual currency only after successful transaction
            CoinBalanceHolder.Instance.AddVirtualCurrency(virtualCurrency);
            Debug.Log($"Purchase Successful! Transaction hash: {transactionHash}");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Purchase process failed: {ex.Message}");
            return false;
        }
    }

    private void SetButtonsInteractable(bool interactable)
    {
        if (oneDollar != null) oneDollar.interactable = interactable;
        if (threeDollar != null) threeDollar.interactable = interactable;
        if (sevenDollar != null) sevenDollar.interactable = interactable;
    }

    public async Task ResumeSession()
    {
        if (!AppKit.IsInitialized)
        {
            Debug.LogError("AppKit not initialized yet!");
            return;
        }

        try
        {
            // Try to resume account connection from the last session
            var resumed = await AppKit.ConnectorController.TryResumeSessionAsync();

            if (resumed && AppKit.Account != null)
            {
                // Continue to the game
                MyAccountConnectedHandler(AppKit.Account);
                Debug.Log("Session resumed successfully!");
            }
            else
            {
                // No previous session or session expired
                Debug.Log("No previous session found. User needs to connect wallet.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to resume session: {ex.Message}");
        }
    }

    public void ConnectWallet()
    {
        if (!AppKit.IsInitialized)
        {
            Debug.LogError("AppKit not initialized yet!");
            return;
        }

        try
        {
            AppKit.OpenModal();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to open wallet modal: {ex.Message}");
        }
    }

    public void DisconnectWallet()
    {
        if (!AppKit.IsInitialized)
        {
            Debug.LogError("AppKit not initialized yet!");
            return;
        }

        try
        {
            AppKit.DisconnectAsync();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to disconnect wallet: {ex.Message}");
        }
    }

    public void MyAccountConnectedHandler(Account connectedAccount)
    {
        account = connectedAccount;
        isWalletConnected = true;
        Debug.Log($"Wallet connected successfully! Address: {account.Value.Address}");
    }

    public void MyAccountDisconnectedHandler()
    {
        account = null;
        isWalletConnected = false;
        Debug.Log("Wallet disconnected!");
    }

    public async void SendEth()
    {
        if (!AppKit.IsAccountConnected || !account.HasValue)
        {
            Debug.LogError("Wallet not connected!");
            return;
        }

        try
        {
            const string toAddress = "0xd8dA6BF26964aF9D7eEd9e03E53415D37aA96045";
            BigInteger amount = Web3.Convert.ToWei(0.001);
            string result = await AppKit.Evm.SendTransactionAsync(toAddress, amount);

            Debug.Log("Transaction hash: " + result);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to send ETH: {ex.Message}");
        }
    }

    public async void GetEthBal()
    {
        if (!AppKit.IsAccountConnected || !account.HasValue)
        {
            Debug.LogError("Wallet not connected!");
            return;
        }

        try
        {
            BigInteger balance = await AppKit.Evm.GetBalanceAsync(account.Value.Address);
            Debug.Log($"ETH Balance: {Web3.Convert.FromWei(balance)} ETH");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to get ETH balance: {ex.Message}");
        }
    }

    public async Task<int> GetERC20TokenBal(string ownerAddressX)
    {
        if (!AppKit.IsInitialized)
        {
            Debug.LogError("AppKit not initialized yet!");
            return 0;
        }

        try
        {
            const string contractAddress = tokenContractAddress;
            string ownerAddress = ownerAddressX;
            string abi = abiFile.text;

            if (string.IsNullOrEmpty(abi))
            {
                Debug.LogError("ABI file is empty or not loaded!");
                return 0;
            }

            var evm = AppKit.Evm;
            var tokenBalance = await evm.ReadContractAsync<BigInteger>(contractAddress, abi, "balanceOf", new object[]
            {
                ownerAddress
            });

            var decimals = await evm.ReadContractAsync<BigInteger>(contractAddress, abi, "decimals");

            var finalBalance = tokenBalance / BigInteger.Pow(10, (int)decimals);
            Debug.Log($"ERC20 Token Balance: {finalBalance}");
            return (int)finalBalance;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to get ERC20 token balance: {ex.Message}");
            return 0;
        }
    }

    public async Task<string> SendERC20Token(float amt)
    {
        if (!AppKit.IsInitialized)
        {
            Debug.LogError("AppKit not initialized yet!");
            return null;
        }

        if (!AppKit.IsAccountConnected || !account.HasValue)
        {
            Debug.LogError("Wallet not connected!");
            return null;
        }

        try
        {
            const string contractAddress = tokenContractAddress;
            const string recipientAddress = marketplaceAddress;
            string abi = abiFile.text;

            if (string.IsNullOrEmpty(abi))
            {
                Debug.LogError("ABI file is empty or not loaded!");
                return null;
            }

            // Get token decimals to properly format the amount
            var decimals = await AppKit.Evm.ReadContractAsync<BigInteger>(contractAddress, abi, "decimals");
            BigInteger amount = new BigInteger(amt) * BigInteger.Pow(10, (int)decimals);

            // Check current allowance
            var currentAllowance = await GetAllowance(contractAddress, abi, account.Value.Address, recipientAddress);

            // If allowance is insufficient, request approval
            if (currentAllowance < amount)
            {
                Debug.Log($"Insufficient allowance. Current: {currentAllowance}, Required: {amount}");

                var approvalResult = await ApproveToken(contractAddress, abi, recipientAddress, amount);
                if (string.IsNullOrEmpty(approvalResult))
                {
                    Debug.LogError("Token approval failed!");
                    return null;
                }

                Debug.Log($"Token approval successful! Transaction hash: {approvalResult}");

                // Wait a moment for the approval transaction to be mined
                await Task.Delay(2000);

                // Verify the approval went through
                var newAllowance = await GetAllowance(contractAddress, abi, account.Value.Address, recipientAddress);
                if (newAllowance < amount)
                {
                    Debug.LogError("Approval transaction may not have been mined yet. Please try again in a moment.");
                    return null;
                }
            }
            else
            {
                Debug.Log($"Sufficient allowance available: {currentAllowance}");
            }

            // Proceed with the transfer
            var arguments = new object[]
            {
                recipientAddress,
                amount
            };


            // Estimate gas amount for transfer
            var gasAmount = await AppKit.Evm.EstimateGasAsync(contractAddress, abi, "transfer", arguments: arguments);

            // Send transfer transaction
            var result = await AppKit.Evm.WriteContractAsync(contractAddress, abi, "transfer", gasAmount, arguments);
            Debug.Log($"ERC20 Transfer successful! Transaction hash: {result}");
            return result;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to send ERC20 token: {ex.Message}");
            return null;
        }
    }

    private async Task<BigInteger> GetAllowance(string contractAddress, string abi, string ownerAddress, string spenderAddress)
    {
        try
        {
            var arguments = new object[]
            {
                ownerAddress,
                spenderAddress
            };

            var allowance = await AppKit.Evm.ReadContractAsync<BigInteger>(contractAddress, abi, "allowance", arguments);
            return allowance;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to get allowance: {ex.Message}");
            return BigInteger.Zero;
        }
    }

    private async Task<string> ApproveToken(string contractAddress, string abi, string spenderAddress, BigInteger amount)
    {
        try
        {
            var arguments = new object[]
            {
                spenderAddress,
                amount
            };

            // Estimate gas for approval
            var gasAmount = await AppKit.Evm.EstimateGasAsync(contractAddress, abi, "approve", arguments: arguments);

            // Send approval transaction
            var result = await AppKit.Evm.WriteContractAsync(contractAddress, abi, "approve", gasAmount, arguments);
            return result;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to approve token: {ex.Message}");
            return null;
        }
    }

    // Optional: Method to approve maximum amount (common pattern)
    private async Task<string> ApproveMaxToken(string contractAddress, string abi, string spenderAddress)
    {
        try
        {
            // Use maximum uint256 value for unlimited approval
            BigInteger maxAmount = BigInteger.Parse("115792089237316195423570985008687907853269984665640564039457584007913129639935");

            var arguments = new object[]
            {
                spenderAddress,
                maxAmount
            };

            var gasAmount = await AppKit.Evm.EstimateGasAsync(contractAddress, abi, "approve", arguments: arguments);
            var result = await AppKit.Evm.WriteContractAsync(contractAddress, abi, "approve", gasAmount, arguments);

            Debug.Log($"Max approval granted to {spenderAddress}");
            return result;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to approve max token: {ex.Message}");
            return null;
        }
    }

    // Optional: Method to revoke approval (set allowance to 0)
    public async Task<string> RevokeApproval(string spenderAddress)
    {
        try
        {
            const string contractAddress = tokenContractAddress;
            string abi = abiFile.text;

            var arguments = new object[]
            {
                spenderAddress,
                BigInteger.Zero
            };

            var gasAmount = await AppKit.Evm.EstimateGasAsync(contractAddress, abi, "approve", arguments: arguments);
            var result = await AppKit.Evm.WriteContractAsync(contractAddress, abi, "approve", gasAmount, arguments);

            Debug.Log($"Approval revoked for {spenderAddress}");
            return result;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to revoke approval: {ex.Message}");
            return null;
        }
    }

    // Helper method to check if everything is ready for transactions
    public bool IsReadyForTransactions()
    {
        return AppKit.IsInitialized && AppKit.IsAccountConnected && account.HasValue;
    }

    // Public method to get connection status
    public bool IsWalletConnected()
    {
        return AppKit.IsAccountConnected && account.HasValue;
    }

    // Public method to get account address
    public string GetAccountAddress()
    {
        return account.HasValue ? account.Value.Address : "";
    }

    private void OnDestroy()
    {
        // Clean up event handlers if necessary
    }

    // Method to manually trigger wallet connection (for UI button)
    public void OnConnectWalletButtonClick()
    {
        ConnectWallet();
    }

    // Method to manually trigger wallet disconnection (for UI button)
    public void OnDisconnectWalletButtonClick()
    {
        DisconnectWallet();
    }

    // ----------------------------
    // NEW: NFT Minting helpers
    // ----------------------------

    /// <summary>
    /// High-level: upload image -> upload metadata -> mint NFT to connected account
    /// </summary>
    /// <param name="skinImage">Texture2D of the skin (can be created/obtained at runtime)</param>
    /// <param name="skinName">Name/title of the skin</param>
    /// <param name="skinType">Skin type (e.g. "Legendary", "Camo")</param>
    /// <returns>Transaction hash if successful, null otherwise</returns>
    // Replace your MintNftAsync method with this fixed version:
    public async Task<string> MintNftAsync(
        string skinName,
        string skinType
    )
    {

        Debug.Log("MintNFT called");
        if (!IsReadyForTransactions())
        {
            Debug.LogError("Not ready for transactions (init/wallet).");
            return null;
        }

        if (string.IsNullOrEmpty(erc721ContractAddressNFT) || nftAbiFile == null)
        {
            Debug.LogError("ERC721 contract address or ABI file is not set.");
            return null;
        }

        try
        {

            // This will now serialize properly
            string metadataJson = $@"{{
                    ""name"": ""{EscapeJson(skinName)}"",
                    ""description"": ""In-game skin: {EscapeJson(skinName)} ({EscapeJson(skinType)})"",
                    ""image"": ""{SkinNFTUrls.GetUrl(skinName)}"",
                    ""attributes"": [
                        {{
                            ""trait_type"": ""skinType"",
                            ""value"": ""{EscapeJson(skinType)}""
                        }},
                        {{
                            ""trait_type"": ""game"",
                            ""value"": ""Gun Drive""
                        }}
                    ]
                }}";
            Debug.Log($"Metadata JSON: {metadataJson}");

            // Upload metadata to Cloudinary
            string metadataUrl = await UploadJsonToCloudinary(metadataJson, skinName);
            if (string.IsNullOrEmpty(metadataUrl))
            {
                Debug.LogError("Metadata upload failed.");
                return null;
            }

            string tokenUri = metadataUrl;
            Debug.Log($"Metadata uploaded: {tokenUri}");

            // Call contract.mint(to, tokenURI)
            var abi = nftAbiFile.text;
            string toAddress = account.Value.Address;

            // Estimate gas
            BigInteger gasEstimate = await AppKit.Evm.EstimateGasAsync(
                erc721ContractAddressNFT,
                abi,
                "mint",
                arguments: new object[] { toAddress, tokenUri }
            );

            string txHash = await AppKit.Evm.WriteContractAsync(
                erc721ContractAddressNFT,
                abi,
                "mint",
                gasEstimate,
                new object[] { toAddress, tokenUri }
            );

            Debug.Log($"Mint tx sent: {txHash}");
            return txHash;
        }
        catch (Exception ex)
        {
            Debug.LogError($"MintNftAsync failed: {ex.Message}");
            return null;
        }
    }

    // Also update your UploadJsonToCloudinary method to handle errors better:
    public async Task<string> UploadJsonToCloudinary(string jsonString, string skinName)
    {
        if (string.IsNullOrEmpty(jsonString))
        {
            Debug.LogError("Empty JSON");
            return null;
        }

        // Check if required fields are set
        if (string.IsNullOrEmpty(cloudName) || string.IsNullOrEmpty(uploadPreset))
        {
            Debug.LogError("Cloudinary cloudName or uploadPreset not configured");
            return null;
        }

        string url = $"https://api.cloudinary.com/v1_1/{cloudName}/raw/upload";

        // Convert JSON to bytes
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);

        // Create multipart form data
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormFileSection("file", jsonBytes, "metadata.json", "application/json"));
        formData.Add(new MultipartFormDataSection("upload_preset", uploadPreset));

        // Only add public_id if it's not a placeholder
        // if (!string.IsNullOrEmpty(public_id) && public_id != "your_unsigned_upload_preset")
        // {
        string publicId = $"gun_drive/metadata_{skinName.Replace(" ", "_")}.json";
        formData.Add(new MultipartFormDataSection("public_id", publicId));
        formData.Add(new MultipartFormDataSection("filename_override", "true"));

        // }

        // Only add api_key if it's set and not empty
        if (!string.IsNullOrEmpty(api_key))
        {
            formData.Add(new MultipartFormDataSection("api_key", api_key));
        }

        using (UnityWebRequest uwr = UnityWebRequest.Post(url, formData))
        {
            uwr.timeout = 60;

            try
            {
                await SendRequestAsync(uwr);

                if (uwr.result == UnityWebRequest.Result.Success)
                {
                    string response = uwr.downloadHandler.text;
                    Debug.Log($"Cloudinary upload success: {response}");

                    // Extract the secure_url from Cloudinary response
                    CloudinaryResponse res = JsonUtility.FromJson<CloudinaryResponse>(response);
                    return res.secure_url;
                }
                else
                {
                    Debug.LogError($"Cloudinary upload failed: {uwr.error}");
                    Debug.LogError($"Response: {uwr.downloadHandler.text}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Cloudinary upload error: {ex.Message}");
                return null;
            }
        }
    }

    /// <summary>
    /// If you already have a tokenURI (ipfs://...), call this to mint directly.
    /// </summary>
    public async Task<string> MintNftWithTokenUriAsync(string tokenUri, string toAddress = null)
    {
        if (!IsReadyForTransactions())
        {
            Debug.LogError("Not ready for transactions (init/wallet).");
            return null;
        }

        if (string.IsNullOrEmpty(erc721ContractAddressNFT) || nftAbiFile == null)
        {
            Debug.LogError("ERC721 contract address or ABI file is not set.");
            return null;
        }

        try
        {
            if (toAddress == null) toAddress = account.Value.Address;
            var abi = nftAbiFile.text;
            BigInteger gasEstimate = await AppKit.Evm.EstimateGasAsync(erc721ContractAddressNFT, abi, "mint", arguments: new object[] { toAddress, tokenUri });
            string txHash = await AppKit.Evm.WriteContractAsync(erc721ContractAddressNFT, abi, "mint", gasEstimate, new object[] { toAddress, tokenUri });
            Debug.Log($"Mint tx sent: {txHash}");
            return txHash;
        }
        catch (Exception ex)
        {
            Debug.LogError($"MintNftWithTokenUriAsync failed: {ex.Message}");
            return null;
        }
    }

    // ----------------------------
    // Pinata upload helpers
    // ----------------------------

    /// <summary>
    /// Uploads a Texture2D as a file to Pinata's pinFileToIPFS endpoint.
    /// Returns the IPFS CID if successful (or null).
    /// </summary>
    private async Task<string> UploadImageToPinata(Texture2D image, string filename = "file.png")
    {
        if (image == null)
        {
            Debug.LogError("No image provided");
            return null;
        }

        byte[] bytes = image.EncodeToPNG();
        string url = "https://api.pinata.cloud/pinning/pinFileToIPFS";

        WWWForm form = new WWWForm();
        form.AddBinaryData("file", bytes, filename, "image/png");

        using (UnityWebRequest uwr = UnityWebRequest.Post(url, form))
        {
            uwr.SetRequestHeader("pinata_api_key", pinataApiKey);
            uwr.SetRequestHeader("pinata_secret_api_key", pinataSecretApiKey);
            uwr.timeout = 120;

            try
            {
                await SendRequestAsync(uwr);
                string json = uwr.downloadHandler.text;
                string cid = ParseIpfsHashFromPinataResponse(json);
                return cid;
            }
            catch (Exception ex)
            {
                Debug.LogError($"UploadImageToPinata error: {ex.Message}");
                return null;
            }
        }
    }


    [Serializable]
    public class CloudinaryResponse
    {
        public string secure_url;
    }


    /// <summary>
    /// Sends a UnityWebRequest and awaits completion. Throws exception on error.
    /// </summary>
    private Task SendRequestAsync(UnityWebRequest uwr)
    {
        var tcs = new TaskCompletionSource<bool>();
        var op = uwr.SendWebRequest();
        op.completed += _ =>
        {
            // Unity 2020+ has UnityWebRequest.Result
#if UNITY_2020_1_OR_NEWER
            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
#else
            if (uwr.isNetworkError || uwr.isHttpError)
#endif
            {
                tcs.SetException(new Exception($"Network error: {uwr.error} - {uwr.downloadHandler?.text}"));
            }
            else
            {
                tcs.SetResult(true);
            }
        };
        return tcs.Task;
    }

    /// <summary>
    /// Parses a pinning response JSON from Pinata and extracts the IpfsHash field (CID).
    /// Uses regex to be robust to small variations.
    /// </summary>
    private string ParseIpfsHashFromPinataResponse(string json)
    {
        if (string.IsNullOrEmpty(json)) return null;
        var m = Regex.Match(json, "\"IpfsHash\"\\s*:\\s*\"([^\"]+)\"", RegexOptions.IgnoreCase);
        if (m.Success) return m.Groups[1].Value;
        m = Regex.Match(json, "\"IpfsHash\"\\s*:\\s*([^,\\}\\s]+)", RegexOptions.IgnoreCase);
        if (m.Success) return m.Groups[1].Value.Trim('"');
        return null;
    }

    // Optional: escape JSON substrings (if you end up building JSON manually)
    private string EscapeJson(string s)
    {
        if (s == null) return "";
        return s.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }
}
