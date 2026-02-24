using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.EconomyModels;
using TMPro;
using UnityEngine;
using PlayFab.ClientModels;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class GunShopManager : MonoBehaviour
{
    public int TotalCoins;
   

    public int[] GunPrices;
    public int CurrentIDPrice;

    public GameObject[] BuyButtonsInScene, SelectButtons;


    List<string> allGuns = new List<string>
    {
      "FireBall_Gun", "Laser_Gun", "DeathRay_Gun","BlackHole_Gun"
    
    };

    Dictionary<string, string> ownedGuns = new Dictionary<string, string>();

    public Dictionary<string, GameObject> GunBuyButtons = new Dictionary<string, GameObject>();

    public Dictionary<string, int> GunPricesDictionary = new Dictionary<string, int>();

    public TextMeshProUGUI CoinBalanceText;
    public GameObject InsufficientBalanceUI;

    public Sprite[] WeaponSprites;
    public Image CurrentWeaponImage;
    public int currentWeaponSelection;

    public static bool hasFetchedGuns = false;
    public static List<string> cachedOwnedGuns = new List<string>();

    public GameObject ProccessingUI;
    // Start is called before the first frame update
    void Start()
    {
        TotalCoins = CoinBalanceHolder.Instance.virtualCurrencyBalance;


      //  GunBuyButtons["Riffle_Gun"] = BuyButtonsInScene[0];
       
        GunBuyButtons["FireBall_Gun"] = BuyButtonsInScene[0];
        GunBuyButtons["Laser_Gun"] = BuyButtonsInScene[1];
        GunBuyButtons["DeathRay_Gun"] = BuyButtonsInScene[2];
        GunBuyButtons["BlackHole_Gun"] = BuyButtonsInScene[2];

        if (!hasFetchedGuns)
        {
            if (LoggedInManager.Instance.isLoggedIn)
                GetOwnedGunsFromPlayfab();// load what is in storage
        }
        else
        {
            Debug.Log("Using cached Gun list!");
            if (LoggedInManager.Instance.isLoggedIn)
            {
                ApplyOwnedGuns(); // Optional: Use this if you want to re-apply skin states in this scene
            }

        }



    }
    // Update is called once per frame
    void Update()
    {
        CoinBalanceText.text = "Coins :"+CoinBalanceHolder.Instance.virtualCurrencyBalance.ToString();

        if (PlayerPrefs.HasKey("SelectedGun"))
        {
            currentWeaponSelection = PlayerPrefs.GetInt("SelectedGun");

            CurrentWeaponImage.sprite = WeaponSprites[currentWeaponSelection];
        }
        else
        {
            currentWeaponSelection = 0;
            CurrentWeaponImage.sprite = WeaponSprites[0];
        }

    }

    void GetOwnedGunsFromPlayfab()
    {
        // we should use static variables instead to cache, unless a purchase was made instead of always calling the api
        if (Time.time - LoggedInManager.Instance.LastCallsTime < LoggedInManager.Instance.PurchasingApiCallInterval)
        {
            return;
        }
        LoggedInManager.Instance.LastCallsTime = Time.time;

        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            foreach (string Gun_ID in allGuns)
            {
                if (result.Data != null && result.Data.ContainsKey(Gun_ID) && result.Data[Gun_ID].Value == "owned")
                {
                    Debug.Log(Gun_ID + " is owned!");
                    ownedGuns[Gun_ID] = "owned";

                    cachedOwnedGuns.Add(Gun_ID);
                    hasFetchedGuns = true; // Set flag to prevent future API calls
                    ApplyOwnedGuns();
                    // Example: Enable the Gun Selection button
                    // yourGunButtonDictionary[GunID].interactable = true;

                    GunBuyButtons[Gun_ID].SetActive(false);// allow selection to show
                }
                else
                {
                    Debug.Log(Gun_ID + " is NOT owned.");
                    ownedGuns[Gun_ID] = "not_owned";

                    // Example: Disable the Gun button
                    // yourGunButtonDictionary[GunID].interactable = false;
                    GunBuyButtons[Gun_ID].SetActive(true);// dont allow selection btn to show
                }
            }
        },
        error =>
        {
            Debug.LogError("Failed to get user data: " + error.GenerateErrorReport());
        });
    }

    void SaveGunAsOwned(string GunID)
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
            { GunID, "owned" }
        }
        };

        PlayFabClientAPI.UpdateUserData(request, result =>
        {
            Debug.Log("Saved " + GunID + " as owned.");
            ownedGuns[GunID] = "owned"; // Update local cache too
            GunBuyButtons[GunID].SetActive(false);// deactivate buy button
            ProccessingUI.SetActive(false);

            cachedOwnedGuns.Add(GunID);
            ApplyOwnedGuns();
        },
        error =>
        {
            Debug.LogError("Failed to save Gun: " + error.GenerateErrorReport());
        });
    }

    void ApplyOwnedGuns()
    {
        foreach (string Gun_ID in cachedOwnedGuns)
        {
            // Enable buttons or unlock visuals for each owned Gun
            if (GunBuyButtons.ContainsKey(Gun_ID))
            {
                GunBuyButtons[Gun_ID].SetActive(false);
                
            }
        }
    }


    public void CurrentIDSetter(int ID)
    {
        CurrentIDPrice = ID;
    }
    public void BuyGun(string GunName)
    {
       
        if (CoinBalanceHolder.Instance.virtualCurrencyBalance >= GunPrices[CurrentIDPrice])
        {
            //int newCoinBalance = TotalCoins - GunPrices[CurrentIDPrice];// subtract balance
            //CoinBalanceHolder.Instance.virtualCurrencyBalance = newCoinBalance;
            CoinBalanceHolder.Instance.SubtractVirtualCurrency(GunPrices[CurrentIDPrice]); // update virtual currency
            ProccessingUI.SetActive(true);
            // save state in backend
             SaveGunAsOwned(GunName);
        }
        else
        {
            InsufficientBalanceUI.SetActive(true);
            return;
            // throw balance error message
        }


    }


    public void SelectGunForBattle(int ID)
    {
        PlayerPrefs.SetInt("SelectedGun", ID);// this will be used in the multiplayer scene to select from the gun array
        
    }








   
    
}
