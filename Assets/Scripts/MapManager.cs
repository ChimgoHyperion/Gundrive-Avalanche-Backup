using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MapManager : MonoBehaviour
{
    public MapStates mapstates;
    public Button map2, map3, map4,map5;
    public int TotalCoins;
    public Button buy2, buy3, buy4,buy5;
    private string mapStatesPath;


    // Start is called before the first frame update
    void Start()
    {
      //  mapstates.map4Unlocked = false;
        //mapStatesPath = $"{Application.persistentDataPath}/MapStates.json";
        //if (File.Exists(mapStatesPath))
        //{
        //    string json = File.ReadAllText(mapStatesPath);
        //    mapstates = JsonUtility.FromJson<MapStates>(json);
        //}
        //TotalCoins = coinsStoring.instance.CoinsStored;
        RerenderMaps();
    }

   
    // Update is called once per frame
    void Update()
    {
        //TotalCoins = PlayerPrefs.GetInt("Coins");
    }
    public void Reset()
    {
        File.Delete(mapStatesPath);

        
    }

    public void BuyMap2(int price)
    {
        if (TotalCoins >= price)
        {
            PlayerPrefs.SetInt("Coins", TotalCoins - price);
            mapstates.map2Unlocked = true;
         //   RerenderMaps();
            SaveJson();
        }
    }
    public void BuyMap3(int price)
    {
        if (TotalCoins >= price)
        {
            PlayerPrefs.SetInt("Coins", TotalCoins - price);
            mapstates.map3Unlocked = true;
          //  RerenderMaps();
            SaveJson();
        }
    }
    public void BuyMap4(int price)
    {

        /*if (coinsStoring.instance.HighScore>= 4000)
        {
            mapstates.map4Unlocked = true;
            RerenderMaps();
            SaveJson();
        }*/
        if (TotalCoins >= price)
        {
            PlayerPrefs.SetInt("Coins", TotalCoins - price);
            mapstates.map4Unlocked = true;
          //  RerenderMaps();
            SaveJson();
        }
    }
    public void BuyMap5(int price)
    {

        /*if (coinsStoring.instance.HighScore>= 4000)
        {
            mapstates.map4Unlocked = true;
            RerenderMaps();
            SaveJson();
        }*/
        if (TotalCoins >= price)
        {
            PlayerPrefs.SetInt("Coins", TotalCoins - price);
            mapstates.map5Unlocked = true;
           // RerenderMaps();
            SaveJson();
        }
    }
    private void RerenderMaps()
    {
        if (mapstates.map2Unlocked)
        {
            map2.interactable = true;
            buy2.gameObject.SetActive(false);
        }
        if (mapstates.map3Unlocked)
        {
            map3.interactable = true;
            buy3.gameObject.SetActive(false);
        }
        if (mapstates.map4Unlocked)
        {
            map4.interactable = true;
            buy4.gameObject.SetActive(false);
        }
        if (mapstates.map5Unlocked)
        {
            map5.interactable = true;
            buy5.gameObject.SetActive(false);
        }
    }
    private void SaveJson()
    {
        string json = JsonUtility.ToJson(mapstates);
        File.WriteAllText(mapStatesPath, json);
    }
    public void UnlockAll()
    {
        mapstates.map2Unlocked = true;
        mapstates.map3Unlocked = true;
        mapstates.map4Unlocked = true;
        mapstates.map5Unlocked = true;
        RerenderMaps();
        SaveJson();
    }
}
