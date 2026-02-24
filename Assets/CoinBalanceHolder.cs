using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class CoinBalanceHolder : MonoBehaviour
{
    public static CoinBalanceHolder Instance;

    public bool hasInitialised;

    public int virtualCurrencyBalance;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);

    }

    public void GetInventoryCoinBalance()// called after login
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result =>
        {
            hasInitialised = true;
            virtualCurrencyBalance = result.VirtualCurrency["CN"];
            Debug.Log($"Balance set: {virtualCurrencyBalance}");
        }, OnInitializeError);
    }
    private void OnInitializeError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }


    public void SubtractVirtualCurrency(int amount)// when player spends
    {

        // 30 requests in 90 seconds is the individual player limit
        //if (CoinBalanceHolder.Instance.virtualCurrencyBalance < amount)
        //{

        //    Debug.Log("Insufficient"); return;
        //}
        var request = new SubtractUserVirtualCurrencyRequest
        {
            VirtualCurrency = "CN",
            Amount = amount

        };
        PlayFabClientAPI.SubtractUserVirtualCurrency(request, OnSubtractSuccess, OnSubtractError);
    }

    public void AddVirtualCurrency(int amount)// when player spends
    {

        // 30 requests in 90 seconds is the individual player limit
        //if (CoinBalanceHolder.Instance.virtualCurrencyBalance < amount)
        //{

        //    Debug.Log("Insufficient"); return;
        //}
        var request = new AddUserVirtualCurrencyRequest
        {
            VirtualCurrency = "CN",
            Amount = amount

        };
        PlayFabClientAPI.AddUserVirtualCurrency(request, OnAddSuccess, OnAddError);
    }

    void OnSubtractSuccess(ModifyUserVirtualCurrencyResult result)
    {
        virtualCurrencyBalance = result.Balance;
    }
    void OnSubtractError(PlayFabError error)
    {

    }

    // In CoinBalanceHolder.cs
    // public void SetVirtualCurrency(int amount)
    // {
    //     virtualCurrencyBalance = amount;
    //     // Save to PlayerPrefs or your persistence system
    //     PlayerPrefs.SetInt("VirtualCurrency", virtualCurrencyBalance);
    //     PlayerPrefs.Save();
    // }

    void OnAddSuccess(ModifyUserVirtualCurrencyResult result)
    {
        virtualCurrencyBalance = result.Balance;
    }
    void OnAddError(PlayFabError error)
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
}
