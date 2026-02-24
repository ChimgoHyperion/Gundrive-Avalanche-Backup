using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.EconomyModels;
using UnityEngine;

public class LoggedInManager : MonoBehaviour
{
    public static LoggedInManager Instance { get; private set; }
    public bool isLoggedIn = false;

  
    public float LastCallsTime=0f;
    public float LeaderboardApiCallInterval=5f,PurchasingApiCallInterval=1f;

    // Start is called before the first frame update
    void Awake()
    {
      
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
       
        
        
    }

  
}
