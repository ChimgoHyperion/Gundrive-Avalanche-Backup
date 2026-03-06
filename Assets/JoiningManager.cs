using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Thirdweb;
using Thirdweb.Unity;
using UnityEngine;
using UnityEngine.UI;

public class JoiningManager : MonoBehaviour
{
    private string stakeToPlayAddress =
         "0xE4bBd8966D2e833C18F4255AaB1f4DF785c7B5E1";

    private string usdcAddress =
        "0x5425890298aed601595a70AB815c96711a31Bc65";

    private ThirdwebContract stakeToPlay;
    private ThirdwebContract usdc;

    public InputField RoomToJoinField;
    [SerializeField] int MatchID;
    public UnityEngine.UI.Button JoinMatchBtn;

    async void Start()
    {
        stakeToPlay = await ThirdwebManager.Instance.GetContract(
            address: stakeToPlayAddress,
            chainId: 43113
        );

        usdc = await ThirdwebManager.Instance.GetContract(
            address: usdcAddress,
            chainId: 43113
        );
    }

    private void Update()
    {
        if (RoomToJoinField.text.Length < 1)
        {
            JoinMatchBtn.interactable = false;
        }
        else
        {
            JoinMatchBtn.interactable = true;
        }

        if (int.TryParse(RoomToJoinField.text, out int value))
        {
          //  Debug.Log("You entered: " + value);
            MatchID = value;
        }
        else
        {
            // Debug.LogWarning("Invalid input! Please enter a valid integer.");

        }
    }

    public void ButtonClickToJoinMatch()
    {
        JoinMatch(MatchID);
    }
    // Call this when player clicks "Join Match" button
    // matchId = the ID from Photon room name
    public async void JoinMatch(int matchId)
    {
        try
        {
            // ?? STEP 1 — Get Match Info First ?????????????
            // We need to know how much USDC to approve
            Debug.Log("Getting match info...");

            // Read the match details from contract
            var matchInfo = await ThirdwebContract.Read<object[]>(
                contract: stakeToPlay,
                method: "getMatch",
                parameters: new object[] { matchId }
            );

            // stakeAmount is the 3rd return value from getMatch
            // getMatch returns: id, playerCount, stakeAmount, totalPot, status
            BigInteger stakeAmount = (BigInteger)matchInfo[2];

            Debug.Log("Match stake amount: " + stakeAmount);

            // ?? STEP 2 — Approve USDC ?????????????????????
            Debug.Log("Approving USDC...");

            await ThirdwebContract.Write(
                wallet: ThirdwebManager.Instance.ActiveWallet,
                contract: usdc,
                method: "approve",
                weiValue: 0,
                parameters: new object[] {
                    stakeToPlayAddress,  // who can spend
                    stakeAmount          // exact amount to approve
                }
            );

            Debug.Log("USDC Approved!");

            // ?? STEP 3 — Join The Match ???????????????????
            Debug.Log("Joining match " + matchId + "...");

            var receipt = await ThirdwebContract.Write(
                wallet: ThirdwebManager.Instance.ActiveWallet,
                contract: stakeToPlay,
                method: "JoinMatch",  // capital J as written in contract
                weiValue: 0,
                parameters: new object[] { matchId }
            );

            Debug.Log("Joined Match! TX: " + receipt.TransactionHash);

            // ?? STEP 4 — Join Photon Room ?????????????????
            // Join the Photon room using match ID
            PlayerPrefs.SetInt("currentMatchId", matchId);
            // PhotonNetwork.JoinRoom(matchId.ToString());
            FusionNetworkManager.networkManagerInstance.ConnecToSpecificStakingSession(matchId);
        }
        catch (System.Exception e)
        {
            Debug.LogError("JoinMatch failed: " + e.Message);
        }
    }
}
