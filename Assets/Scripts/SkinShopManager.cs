using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SkinShopManager : MonoBehaviour
{
    public int TotalCoins;


    public int[] SkinPrices;
    public int CurrentIDPrice;

    public GameObject[] BuyButtonsInScene;

    public Button[] SkinSelectButtonsInScene;

    List<string> allSkins = new List<string>
    {
      "AdaezeSkin","Nnae-MechaSkin","KhufuSkin",  "AlohaSkin", "SandySkin", "GhostSkin", "PineappleSkin","Dr.DispenserSkin","BryanSkin"
        ,"Dr.VenethorSkin","NewbaccaSkin","AngelSkin","AlienSkin"

        // add ninja skin later
    };

    Dictionary<string, string> ownedSkins = new Dictionary<string, string>();

    public Dictionary<string, GameObject> SkinBuyButtons = new Dictionary<string, GameObject>();
    public Dictionary<string, Button> SkinSelectionButtons = new Dictionary<string, Button>();

    public Dictionary<string, int> SkinPricesDictionary = new Dictionary<string, int>();

    public TextMeshProUGUI CoinBalanceText;
    public GameObject InsufficientBalanceUI;


    public static bool hasFetchedSkins = false;
    public static List<string> cachedOwnedSkins = new List<string>();

    public GameObject ProccessingUI;
    // Start is called before the first frame update
    void Start()
    {
        TotalCoins = CoinBalanceHolder.Instance.virtualCurrencyBalance;

        // skin buy btns
        SkinBuyButtons["AdaezeSkin"] = BuyButtonsInScene[0];
        SkinBuyButtons["Nnae-MechaSkin"] = BuyButtonsInScene[1];
        SkinBuyButtons["KhufuSkin"] = BuyButtonsInScene[2];
        SkinBuyButtons["AlohaSkin"] = BuyButtonsInScene[3];
        SkinBuyButtons["SandySkin"] = BuyButtonsInScene[4];
        SkinBuyButtons["GhostSkin"] = BuyButtonsInScene[5];
        SkinBuyButtons["PineappleSkin"] = BuyButtonsInScene[6];
        SkinBuyButtons["Dr.DispenserSkin"] = BuyButtonsInScene[7];
        SkinBuyButtons["BryanSkin"] = BuyButtonsInScene[8];
        SkinBuyButtons["Dr.VenethorSkin"] = BuyButtonsInScene[9];
        SkinBuyButtons["NewbaccaSkin"] = BuyButtonsInScene[10];
        SkinBuyButtons["AngelSkin"] = BuyButtonsInScene[11];
        SkinBuyButtons["AlienSkin"] = BuyButtonsInScene[12];

        // skin select buttons

        SkinSelectionButtons["AdaezeSkin"] = SkinSelectButtonsInScene[0];
        SkinSelectionButtons["Nnae-MechaSkin"] = SkinSelectButtonsInScene[1];
        SkinSelectionButtons["KhufuSkin"] = SkinSelectButtonsInScene[2];
        SkinSelectionButtons["AlohaSkin"] = SkinSelectButtonsInScene[3];
        SkinSelectionButtons["SandySkin"] = SkinSelectButtonsInScene[4];
        SkinSelectionButtons["GhostSkin"] = SkinSelectButtonsInScene[5];
        SkinSelectionButtons["PineappleSkin"] = SkinSelectButtonsInScene[6];
        SkinSelectionButtons["Dr.DispenserSkin"] = SkinSelectButtonsInScene[7];
        SkinSelectionButtons["BryanSkin"] = SkinSelectButtonsInScene[8];
        SkinSelectionButtons["Dr.VenethorSkin"] = SkinSelectButtonsInScene[9];
        SkinSelectionButtons["NewbaccaSkin"] = SkinSelectButtonsInScene[10];
        SkinSelectionButtons["AngelSkin"] = SkinSelectButtonsInScene[11];
        SkinSelectionButtons["AlienSkin"] = SkinSelectButtonsInScene[12];

        if (!hasFetchedSkins)// getting from playfab
        {
            if (LoggedInManager.Instance.isLoggedIn)
                GetOwnedSkinsFromPlayfab();// load what is in storage
        }
        else
        {
            Debug.Log("Using cached Gun list!");
            if (LoggedInManager.Instance.isLoggedIn)
            {
                ApplyOwnedSkins(); // Optional: Use this if you want to re-apply skin states in this scene
            }

        }

        // another function will exist which. a, checks the wallet for existence of skin id. if 
        // the id exists, make an api call to save the skin on the player's playfab account (saveskinasowned). 

        if (Web3Manager.Instance.IsWalletConnected())
        {
           StartCoroutine(delayBeforeWalletCheck());
        }
    }
    // Update is called once per frame
    void Update()
    {
        CoinBalanceText.text = "Coins :" + CoinBalanceHolder.Instance.virtualCurrencyBalance.ToString();

       

    }
    IEnumerator delayBeforeWalletCheck()
    {
        yield return new WaitForSeconds(3f);
        GetOwnedSkinsFromWallet();
    }
    void GetOwnedSkinsFromWallet()
    {
       var userNFTs = Web3Manager.Instance.getUserNftSkins();
        foreach (SimpleNftData NFTData in userNFTs)
        {
            Debug.Log(NFTData.name +"is owned");

            ownedSkins[NFTData.name] = "owned";

            cachedOwnedSkins.Add(NFTData.name); // cache skins
          //   hasFetchedSkins = true; // Set flag to prevent future API calls
             ApplyOwnedSkins();
            SaveSkinAsOwned(NFTData.name);
        }
        //if (skin id exists in wallet)
        //{
        //    Debug.Log(Skin_ID + " is owned; // debug 
        //    ownedSkins[Skin_ID] = "owned"; // add the skinid(name) to the ownedskins dictionary.

        //    cachedOwnedSkins.Add(Skin_ID); // cache skins
        //    hasFetchedSkins = true; // Set flag to prevent future API calls
        //    ApplyOwnedSkins();

        //    call save function to save it to the database on playfab 
        ///  SaveSkinAsOwned(skinID);


        //    SkinBuyButtons[Skin_ID].SetActive(false);// deactivate buy buttons
        //    SkinSelectionButtons[Skin_ID].interactable = true; // can select skin. (we can create extra buttons as an alternative)
        //}


        //else
        //{
        //    Debug.Log(Skin_ID + " is NOT owned.");
        //    ownedSkins[Skin_ID] = "not_owned";

        //    // Example: Disable the Gun button
        //    // yourGunButtonDictionary[GunID].interactable = false;
        //    SkinBuyButtons[Skin_ID].SetActive(true);// dont allow selection btn to show
        //    SkinSelectionButtons[Skin_ID].interactable = false;
        //}
    }
    void GetOwnedSkinsFromPlayfab()
    {
        if (Time.time - LoggedInManager.Instance.LastCallsTime < LoggedInManager.Instance.PurchasingApiCallInterval)
        {
            return;
        }
        LoggedInManager.Instance.LastCallsTime = Time.time;

        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            foreach (string Skin_ID in allSkins)
            {
                if (result.Data != null && result.Data.ContainsKey(Skin_ID) && result.Data[Skin_ID].Value == "owned")
                {
                    Debug.Log(Skin_ID + " is owned!");
                    ownedSkins[Skin_ID] = "owned";

                    cachedOwnedSkins.Add(Skin_ID);
                    hasFetchedSkins = true; // Set flag to prevent future API calls
                    ApplyOwnedSkins();
                    // Example: Enable the Gun Selection button
                    // yourGunButtonDictionary[GunID].interactable = true;

                    SkinBuyButtons[Skin_ID].SetActive(false);// deactivate buy buttons
                    SkinSelectionButtons[Skin_ID].interactable = true; // can select skin. (we can create extra buttons as an alternative)
                }
                else
                {
                    Debug.Log(Skin_ID + " is NOT owned.");
                    ownedSkins[Skin_ID] = "not_owned";

                    // Example: Disable the Gun button
                    // yourGunButtonDictionary[GunID].interactable = false;
                    SkinBuyButtons[Skin_ID].SetActive(true);// dont allow selection btn to show
                    SkinSelectionButtons[Skin_ID].interactable = false;
                }
            }
        },
        error =>
        {
            Debug.LogError("Failed to get user data: " + error.GenerateErrorReport());
        });
    }

    void SaveSkinAsOwned(string Skin_ID)
    {
        //if (Time.time - LoggedInManager.Instance.LastCallsTime < LoggedInManager.Instance.LeaderboardApiCallInterval)
        //{
        //    return;
        //}
        //LoggedInManager.Instance.LastCallsTime = Time.time;

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
        {
            { Skin_ID, "owned" }
        }
        };

        PlayFabClientAPI.UpdateUserData(request, async (result) =>
        {
            Debug.Log("Saved " + Skin_ID + " as owned.");
            ownedSkins[Skin_ID] = "owned"; // Update local cache too
            await Web3Manager.Instance.MintNftAsync(Skin_ID, "made_in_aba");
            SkinBuyButtons[Skin_ID].SetActive(false);// deactivate buy button
            SkinSelectionButtons[Skin_ID].interactable = true;
            ProccessingUI.SetActive(false);

            cachedOwnedSkins.Add(Skin_ID);
            // Mint NFT after purchase
            ApplyOwnedSkins();
        },
        error =>
        {
            Debug.LogError("Failed to save skin: " + error.GenerateErrorReport());
        });
    }

    void ApplyOwnedSkins()
    {
        foreach (string Skin_ID in cachedOwnedSkins)
        {
            // Enable buttons or unlock visuals for each owned Gun
            if (SkinBuyButtons.ContainsKey(Skin_ID))
            {
                SkinBuyButtons[Skin_ID].SetActive(false);
                SkinSelectionButtons[Skin_ID].interactable = true;
            }
        }
    }
    public void CurrentIDSetter(int ID)
    {
        CurrentIDPrice = ID;
    }
    public void BuySkin(string SkinName)
    {

        if (CoinBalanceHolder.Instance.virtualCurrencyBalance >= SkinPrices[CurrentIDPrice])
        {
            //int newCoinBalance = TotalCoins - GunPrices[CurrentIDPrice];// subtract balance
            //CoinBalanceHolder.Instance.virtualCurrencyBalance = newCoinBalance;
            CoinBalanceHolder.Instance.SubtractVirtualCurrency(SkinPrices[CurrentIDPrice]); // update virtual currency

            // to prevent button spamming

             ProccessingUI.SetActive(true);
            // save state in backend
            SaveSkinAsOwned(SkinName);
        }
        else
        {
            InsufficientBalanceUI.SetActive(true);
            return;
            // throw balance error message
        }


    }


   

}
