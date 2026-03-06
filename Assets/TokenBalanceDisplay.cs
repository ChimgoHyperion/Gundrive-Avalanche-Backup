using System;
using System.Collections;
using Thirdweb;
using Thirdweb.Unity;
using TMPro;
using UnityEngine;
using System.Numerics;

public class TokenBalanceDisplay : MonoBehaviour
{
   
   
    [Header("UI")]
    public TextMeshProUGUI BalanceText;        // Main balance display  e.g. "1,250.00 GOLD"
    public TextMeshProUGUI StatusText;         // Optional small status  e.g. "Updating..."
    public GameObject LoadingSpinner;     // Optional spinner shown while fetching

    

    public async void ButtonViewBalance()
    {
        string playerWallet = await ThirdwebManager.Instance.ActiveWallet.GetAddress();
        CheckSHCBalance(playerWallet);
    }



    private string shooterCoinAddress =
       "0xA2214B51Bc444f4A1065f629F3Aac1C4720f040c";

    private ThirdwebContract shooterCoin;
    // Check player SHC balance
    public async void CheckSHCBalance(string playerWallet)
    {


        // v5 way to get a contract
        shooterCoin = await ThirdwebManager.Instance.GetContract(
            address: shooterCoinAddress,
            chainId: 43113  // Avalanche Fuji
        );



        var balance = await ThirdwebContract.Read<BigInteger>(
            contract: shooterCoin,
            method: "balanceOf",
            parameters: new object[] { playerWallet }
        );

        // Divide by 10^18 to show human readable amount
        float readableBalance = (float)balance / 1e18f;
        Debug.Log("SHC Balance: " + readableBalance);

        BalanceText.text = readableBalance + " SHC";
    }
}
