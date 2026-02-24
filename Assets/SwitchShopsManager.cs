using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchShopsManager : MonoBehaviour
{
    // Start is called before the first frame update

    public CanvasGroup  SkinSection, CoinSection,WithdrawSection;
    void Start()
    {
        switch (ShopSelectionManager.instance.selectedShopName)
        {
            case "CoinShop":
               ActivateCoinSection();
                break;
            case "SkinShop":
               ActivateSkinsSection();
                break;
            case "WithdrawSection":
               ActivateWithdrawalSection();
                break;

        }
    }

    public void ActivateSkinsSection()
    {
       // only skin section is visible


        SkinSection.alpha = 1.0f;
        SkinSection.interactable = true;
        SkinSection.blocksRaycasts = true;


        CoinSection.alpha = 0f;
        CoinSection.interactable = false;
        CoinSection.blocksRaycasts = false;

        WithdrawSection.alpha = 0f;
        WithdrawSection.interactable = false;
        WithdrawSection.blocksRaycasts = false;
    }

    public void ActivateCoinSection()
    {
        SkinSection.alpha = 0f;
        SkinSection.interactable = false;
        SkinSection.blocksRaycasts = false;


        CoinSection.alpha = 1.0f;
        CoinSection.interactable = true;
        CoinSection.blocksRaycasts = true;

        WithdrawSection.alpha = 0f;
        WithdrawSection.interactable = false;
        WithdrawSection.blocksRaycasts = false;
    }

    public void ActivateWithdrawalSection()
    {
        SkinSection.alpha = 0f;
        SkinSection.interactable = false;
        SkinSection.blocksRaycasts = false;


        CoinSection.alpha = 0f;
        CoinSection.interactable = false;
        CoinSection.blocksRaycasts = false;

        WithdrawSection.alpha = 1.0f;
        WithdrawSection.interactable = true;
        WithdrawSection.blocksRaycasts = true;
    }

    
   
}
