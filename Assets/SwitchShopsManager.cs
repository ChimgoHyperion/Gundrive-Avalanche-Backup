using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchShopsManager : MonoBehaviour
{
    // Start is called before the first frame update

    public CanvasGroup  SkinSection, NFTClaimSection;
    void Start()
    {
        switch (ShopSelectionManager.instance.selectedShopName)
        {
            case "NFTClaimSection":
               ActivateNFTClaimSection();
                break;
            case "SkinShop":
               ActivateSkinsSection();
                break;
           

        }
    }

    public void ActivateSkinsSection()
    {
       // only skin section is visible


        SkinSection.alpha = 1.0f;
        SkinSection.interactable = true;
        SkinSection.blocksRaycasts = true;



        NFTClaimSection.alpha = 0f;
        NFTClaimSection.interactable = false;
        NFTClaimSection.blocksRaycasts = false;
    }

    public void ActivateNFTClaimSection()
    {
        SkinSection.alpha = 0f;
        SkinSection.interactable = false;
        SkinSection.blocksRaycasts = false;


        NFTClaimSection.alpha = 1.0f;
        NFTClaimSection.interactable = true;
        NFTClaimSection.blocksRaycasts = true;

      
    }

   
    
   
}
