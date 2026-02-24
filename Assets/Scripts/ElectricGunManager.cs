using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricGunManager : MonoBehaviour
{
    public int TotalCoins;
    public int Rand;
    public GameObject ElectricbuyButton;
    public GameObject SpecialOffersPanel;
    public GameObject CoinPanel;
    // Start is called before the first frame update
    void Start()
    {
        TotalCoins = PlayerPrefs.GetInt("Coins");
        CheckIfGunBought();
        if((PlayerPrefs.GetInt("HasElectric") == 0))
        {
            RandomCheck();
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BuyGun(int price)
    {
        if (TotalCoins >= price)
        {
            ElectricbuyButton.SetActive(false);
            PlayerPrefs.SetInt("Coins", TotalCoins - price);
            PlayerPrefs.SetInt("HasElectric", 1);
        }
       
    }
    void CheckIfGunBought()
    {
        if (PlayerPrefs.GetInt("HasElectric") == 0)
        {
            ElectricbuyButton.SetActive(true);
        }
        if (PlayerPrefs.GetInt("HasElectric") == 1)
        {
            ElectricbuyButton.SetActive(false);// disable the buy button
            SpecialOffersPanel.SetActive(false); // stop showing the offer
            CoinPanel.SetActive(true);
        }
    }
    void RandomCheck()
    {
         Rand = Random.Range(0, 3);
        switch (Rand)
        {
            case 0:
                SpecialOffersPanel.SetActive(false);
                break;
            case 1:
                SpecialOffersPanel.SetActive(true);
                CoinPanel.SetActive(false);
                break;
            case 2:
                SpecialOffersPanel.SetActive(false);
                CoinPanel.SetActive(true);
                break;
            case 3:
                SpecialOffersPanel.SetActive(true);
                CoinPanel.SetActive(false);
                break;

        }
    }
    public void ClosePanel()
    {
        SpecialOffersPanel.SetActive(false);
        CoinPanel.SetActive(true);
    }
}
