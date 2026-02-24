using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
//using UnityEngine.Purchasing;
//using UnityEngine.Purchasing.Extension;


public class TransactionManager : MonoBehaviour
{

 
    // Start is called before the first frame update
    void Start()
    {
       

    }
 
   

   
  

    public void AddVirtual(int amount)// when player earns
    {
        //if (Time.time - LoggedInManager.Instance.LastCallsTime < LoggedInManager.Instance.LeaderboardApiCallInterval)
        //{
        //    Debug.Log("System Busy, try again later");
        //    return;
        //}
        //LoggedInManager.Instance.LastCallsTime = Time.time;
        var request = new AddUserVirtualCurrencyRequest
        {
            VirtualCurrency = "CN",
            Amount = amount

        };
        PlayFabClientAPI.AddUserVirtualCurrency(request, OnAddSuccess, OnAddError);
    }
    void OnAddSuccess(ModifyUserVirtualCurrencyResult result)
    {
        CoinBalanceHolder.Instance.virtualCurrencyBalance = result.Balance;
    }
    void OnAddError(PlayFabError error)
    {
        Debug.Log(error);
    }
    public void SubtractVirtual(int amount)// when player spends
    {
        //if (Time.time - LoggedInManager.Instance.LastCallsTime < LoggedInManager.Instance.PurchasingApiCallInterval)
        //{
        //    Debug.Log("System Busy, try again later");
        //    return;
        //}
        LoggedInManager.Instance.LastCallsTime = Time.time;

        if (CoinBalanceHolder.Instance.virtualCurrencyBalance < amount) {

            Debug.Log("Insufficient"); return;
        }
        var request = new SubtractUserVirtualCurrencyRequest
        {
            VirtualCurrency = "CN",
            Amount = amount

        };
        PlayFabClientAPI.SubtractUserVirtualCurrency(request, OnSubtractSuccess, OnSubtractError);
    }

    void OnSubtractSuccess(ModifyUserVirtualCurrencyResult result)
    {
        CoinBalanceHolder.Instance.virtualCurrencyBalance = result.Balance;
    }
    void OnSubtractError(PlayFabError error)
    {
        Debug.Log(error);
    }


   

}
