using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using System;

public class GettingServerTime : MonoBehaviour
{
    public TextMeshProUGUI timeText; // Reference to a UI text element

    void Start()
    {
       
    }

    /* public  void GetServerTime()
     {

         PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
         {
             FunctionName = "GetServerTime",
         },
           result =>
           {
               // Log the raw function result for debugging.
               string rawJson = result.FunctionResult.ToString();
               Debug.Log("Raw JSON from CloudScript: " + rawJson);

               // Parse the JSON response using JsonUtility.
               ServerTimeResponse response = JsonUtility.FromJson<ServerTimeResponse>(rawJson);

               // Parse the ISO date string into a DateTime object.
               DateTime serverDateTime = DateTime.Parse(response.serverTime, null, System.Globalization.DateTimeStyles.RoundtripKind);

               // Log the parsed date.
               Debug.Log("Parsed Server Time: " + serverDateTime);

               // Display the server time on the UI, using a custom format.
               if (timeText != null)
               {
                   timeText.text = "Server Time: " + serverDateTime.ToString("yyyy-MM-dd HH:mm:ss");
               }
           },
           error =>
           {
               Debug.LogError("Error getting server time: " + error.ErrorMessage);
           });
     }*/


    // Revised GetServerTime method using a similar format to your example
    public void GetServerTime()
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "GetServerTime"
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnGetServerTime, OnError);
    }

    // Callback for a successful cloud script execution
    void OnGetServerTime(ExecuteCloudScriptResult result)
    {
        // Convert the function result to a string (raw JSON)
        string rawJson = result.FunctionResult.ToString();
        Debug.Log("Raw JSON from CloudScript: " + rawJson);

        // Parse the JSON into our ServerTimeResponse data class
        ServerTimeResponse response = JsonUtility.FromJson<ServerTimeResponse>(rawJson);

        // Convert the ISO date string to a DateTime object
        DateTime serverDateTime = DateTime.Parse(response.serverTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
        Debug.Log("Parsed Server Time: " + serverDateTime);

        // Display the server time on the UI text element
        if (timeText != null)
        {
            timeText.text = "Server Time: " + serverDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }

    // Callback for handling errors
    void OnError(PlayFabError error)
    {
        Debug.LogError("Error getting server time: " + error.ErrorMessage);
    }

}

 

[Serializable]
public class ServerTimeResponse
{
    public string serverTime;
}
