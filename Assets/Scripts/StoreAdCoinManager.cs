using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreAdCoinManager : MonoBehaviour
{
    
    public int coinsStored;
    void Start()
    {
        coinsStored = PlayerPrefs.GetInt("Coins");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SaveCoins()
    {
        PlayerPrefs.SetInt("Coins", coinsStored + 50);
       // PlayerPrefs.Save();
    }
}
