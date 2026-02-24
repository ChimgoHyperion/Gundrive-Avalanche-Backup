using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSelectionManager : MonoBehaviour
{
    // Start is called before the first frame update

    public string selectedShopName;
    public static ShopSelectionManager instance;
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectCoinShop()
    {
        selectedShopName = "CoinShop";
    }
    public void SelectSkins()
    {
        selectedShopName = "SkinShop";
    }

    public void SelectWithdrawal()
    {
        selectedShopName = "WithdrawSection";
    }
}
