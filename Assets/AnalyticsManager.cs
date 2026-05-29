using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using Unity.Services.Analytics;
using Unity.Services.Core;

public class AnalyticsManager : MonoBehaviour
{
   // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
    }

    //async void Start()
    //{
    //    try
    //    {
    //        await UnityServices.InitializeAsync();
    //        GiveConsent(); //Get user consent according to various legislations
    //    }
    //    catch 
    //    {
    //      //  Debug.Log(e.ToString());
    //    }
    //}

    //public void GiveConsent()
    //{
    //    // Call if consent has been given by the user
    //    AnalyticsService.Instance.StartDataCollection();
    //    Debug.Log($"Consent has been provided. The SDK is now collecting data!");
    //}

}
