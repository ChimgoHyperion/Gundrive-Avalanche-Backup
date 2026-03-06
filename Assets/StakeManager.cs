using Photon.Pun;
using Photon.Realtime;
using System.Numerics;
using Thirdweb;
using Thirdweb.Unity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StakeManager : MonoBehaviour
{
    private string stakeToPlayAddress =
        "0xE4bBd8966D2e833C18F4255AaB1f4DF785c7B5E1";

    private string usdcAddress =
        "0x5425890298aed601595a70AB815c96711a31Bc65";

    private ThirdwebContract stakeToPlay;
    private ThirdwebContract usdc;


    public InputField amountToStakeField;
    [SerializeField] int amountToStake;
    public UnityEngine.UI.Button ApprovebtntoCreateMatch;

    public int matchID;
    async void Start()
    {
        // Connect to both contracts
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
        if (amountToStakeField.text.Length < 1)
        {
            ApprovebtntoCreateMatch.interactable = false;
        }else
        {
            ApprovebtntoCreateMatch.interactable = true;
        }

        if (int.TryParse(amountToStakeField.text, out int value))
        {
            Debug.Log("You entered: " + value);
            amountToStake = value;
        }
        else
        {
           // Debug.LogWarning("Invalid input! Please enter a valid integer.");

        }
        matchID = PlayerPrefs.GetInt("currentMatchId");
    }

    // Call this when player clicks "Create Match" button
    // stakeAmountUSDC = whole number e.g. 5 = 5 USDC
    public async void CreateMatch()//(int stakeAmountUSDC)
    {


        int stakeAmountUSDC = amountToStake;
        // Convert to 6 decimals
        // 5 USDC = 5 * 1000000 = 5000000
        int stakeAmount = stakeAmountUSDC * 1000000;

        try
        {
            // ?? STEP 1 — Approve USDC ??????????????????
            // Allow StakeToPlay to take USDC from player
            Debug.Log("Approving USDC...");

            await ThirdwebContract.Write(
                wallet: ThirdwebManager.Instance.ActiveWallet,
                contract: usdc,
                method: "approve",
                weiValue: 0,
                parameters: new object[] {
                    stakeToPlayAddress,  // who can spend it
                    stakeAmount          // how much they can spend
                }
            );

            Debug.Log("USDC Approved!");

            // ?? STEP 2 — Create The Match ???????????????
            Debug.Log("Creating match...");

            var receipt = await ThirdwebContract.Write(
                wallet: ThirdwebManager.Instance.ActiveWallet,
                contract: stakeToPlay,
                method: "createMatch",
                weiValue: 0,
                parameters: new object[] { stakeAmount }
            );

            Debug.Log("Match Created! TX: " + receipt.TransactionHash);

            // ?? STEP 3 — Get The Match ID ???????????????
            // matchGenerator = current match count = latest match ID
            var matchId = await ThirdwebContract.Read<BigInteger>(
                contract: stakeToPlay,
                method: "matchGenerator",
                parameters: new object[] { }
            );

            int currentMatchId = (int)matchId;
            Debug.Log("Match ID: " + currentMatchId);

            // Save match ID — you'll need it for joinMatch
            // and submitScores later
            PlayerPrefs.SetInt("currentMatchId", currentMatchId);

            // Tell Photon to create a room with this match ID
            // PhotonNetwork.CreateRoom(currentMatchId.ToString());

            FusionNetworkManager.networkManagerInstance.CreateStakingRoom(currentMatchId);
        }
        catch (System.Exception e)
        {
            Debug.LogError("CreateMatch failed: " + e.Message);
        }
    }


    // Call this when player clicks "Cancel Match" button
    // Only the match CREATOR can cancel
    // Only works if match is still WAITING (nobody joined yet)
    public async void CancelMatch()
    {
        int matchId = matchID;

        try
        {
            // ?? STEP 1 — Check match status first ?????????
            Debug.Log("Checking match status...");

            var matchInfo = await ThirdwebContract.Read<object[]>(
                contract: stakeToPlay,
                method: "getMatch",
                parameters: new object[] { matchId }
            );

            // status is the 5th return value from getMatch
            // 0 = WAITING, 1 = ACTIVE, 2 = FINISHED, 3 = CANCELLED
            int status = (int)matchInfo[4];

            if (status != 0)
            {
                Debug.LogError("Can only cancel a WAITING match!");
                // Show UI message to player
                return;
            }

            // ?? STEP 2 — Cancel The Match ?????????????????
            Debug.Log("Cancelling match " + matchId + "...");

            var receipt = await ThirdwebContract.Write(
                wallet: ThirdwebManager.Instance.ActiveWallet,
                contract: stakeToPlay,
                method: "cancelMatch",
                weiValue: 0,
                parameters: new object[] { matchId }
            );

            Debug.Log("Match Cancelled! USDC refunded to your wallet");
            Debug.Log("TX: " + receipt.TransactionHash);

            // ?? STEP 3 — Close Photon Room ????????????????
            // Leave the Photon room since match is cancelled
            //if (PhotonNetwork.InRoom)
            //{
            //    PhotonNetwork.LeaveRoom();
            //}

            FusionNetworkManager.networkManagerInstance.LeaveSession();

            // Clear saved match ID
            PlayerPrefs.DeleteKey("currentMatchId");

        }
        catch (System.Exception e)
        {
            Debug.LogError("CancelMatch failed: " + e.Message);
        }
    }



}
