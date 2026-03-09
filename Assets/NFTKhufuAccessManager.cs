using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Thirdweb;
using Thirdweb.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NFTKhufuAccessManager : MonoBehaviour
{
    [Header("Contract")]
    public string NFTContractAddress = "0x498457D377245060449b4C026c58807e24073674";
    public ulong ChainId = 43113;

    [Header("UI")]
    public Button ClaimButton;
    public Button MapButton;
    public TextMeshProUGUI StatusText;

    private ThirdwebContract _nftContract;

    // ?? Runs every time this screen opens ??????????????????????????????????

    private async void OnEnable()
    {
        //  MapButton.interactable = false;
        SetStatus("Checking access...");

        try
        {
            _nftContract = await ThirdwebManager.Instance.GetContract(
                address: NFTContractAddress,
                chainId: ChainId
            );

            await CheckOwnership();
        }
        catch (Exception e)
        {
            SetStatus("Could not reach contract.");
            Debug.LogError($"[NFT] {e.Message}");
        }
    }
    public async void ClaimNFT()
    {
        if (ThirdwebManager.Instance.ActiveWallet == null)
        {
            SetStatus("Connect your wallet first.");
            return;
        }

        ClaimButton.interactable = false;
        SetStatus("Claiming...");

        try
        {
            string playerAddress = await ThirdwebManager.Instance.ActiveWallet.GetAddress();

            // Native currency placeholder address — same on every EVM chain
            string nativeCurrency = "0xEeeeeEeeeEeEeeEeEeEeeEEEeeeeEeeeeeeeEEeE";

            byte[] emptyBytes32 = new byte[32]; // 32 zero bytes = bytes32(0)

                    var allowlistProof = new object[]
                    {
            new byte[][] { emptyBytes32 },                      // proof as actual bytes
            BigInteger.Zero,                                    // quantityLimitPerWallet
            BigInteger.Zero,                                    // pricePerToken
            "0x0000000000000000000000000000000000000000"        // currency
                    };

            var receipt = await ThirdwebContract.Write(
                ThirdwebManager.Instance.ActiveWallet,
                _nftContract,
               "claim",// "function claim(address _receiver, uint256 _tokenId, uint256 _quantity, address _currency, uint256 _pricePerToken, (bytes32[] proof, uint256 quantityLimitPerWallet, uint256 pricePerToken, address currency) _allowlistProof, bytes _data)",
                0,              // weiValue (no AVAX sent since it's free)
                new object[] { playerAddress, 1, 1, nativeCurrency, 0, allowlistProof, new byte[0] }

            );

            Debug.Log($"[NFT] Claimed! TX: {receipt.TransactionHash}");
            SetStatus("NFT Claimed! ?");
            await CheckOwnership();
        }
        catch (Exception e)
        {
            ClaimButton.interactable = true;
            SetStatus("Claim failed. Try again.");
            Debug.LogError($"[NFT] Claim error: {e.Message}");
        }
    }

    // ?? Check Ownership — ERC1155 version ??????????????????????????????????????
    private async System.Threading.Tasks.Task CheckOwnership()
    {
        if (ThirdwebManager.Instance.ActiveWallet == null)
        {
            SetStatus("Connect your wallet first.");
            return;
        }

        string playerAddress = await ThirdwebManager.Instance.ActiveWallet.GetAddress();

        var balance = await ThirdwebContract.Read<BigInteger>(
            contract: _nftContract,
            method: "balanceOf",
            parameters: new object[] { playerAddress, 1 }  // tokenId 0
        );

        bool ownsNFT = balance > 0;

        if (MapButton != null)
            MapButton.interactable = ownsNFT;
        GameState.PlayerOwnsMapNFT = ownsNFT;
        ClaimButton.interactable = !ownsNFT;

        SetStatus(ownsNFT
            ? "Access granted. Map unlocked!"
            : "Claim the NFT to unlock the map.");
    }

   

    private void SetStatus(string msg)
    {
        if (StatusText != null) StatusText.text = msg;
    }


    // GameState.cs
    // A simple persistent data holder — no MonoBehaviour needed
    // Just drop this file in your project, no scene setup required

    public static class GameState
    {
        public static bool PlayerOwnsMapNFT = false;
    }
}
