using Fusion;
using Thirdweb;
using UnityEngine;
using System.Numerics;
using System.Collections.Generic;
using Thirdweb.Unity;

public class SubmitScoresManager : NetworkBehaviour
{
    private string stakeToPlayAddress = 
        "0xE4bBd8966D2e833C18F4255AaB1f4DF785c7B5E1";

    private ThirdwebContract stakeToPlay;

    // These are Photon Fusion networked variables
    // All players can see them but only State Authority writes them
    [Networked, Capacity(4)]
    public NetworkArray<NetworkString<_64>> NetworkedWalletAddresses { get; }

    [Networked, Capacity(4)]
    public NetworkArray<int> NetworkedScores { get; }

    async void Start()
    {
        stakeToPlay = await ThirdwebManager.Instance.GetContract(
            address: stakeToPlayAddress,
            chainId: 43113
        );
    }

    //  STEP 1 — Each player registers their wallet 
    // Call this when player connects their wallet at game start
    [Rpc]
    public void RPC_RegisterWallet(
        string walletAddress, 
        PlayerRef player)
    {
        // State Authority stores each player's wallet
        int playerIndex = player.AsIndex;
        NetworkedWalletAddresses.Set(playerIndex, walletAddress);
        
        Debug.Log($"Player {playerIndex} registered wallet: {walletAddress}");
    }

    // ?? STEP 2 — Update scores during match
    // Only State Authority can update scores
    public void UpdatePlayerScore(PlayerRef player, int newScore)
    {
        //if (!HasStateAuthority) return;
        
        int playerIndex = player.AsIndex;
        NetworkedScores.Set(playerIndex, newScore);
        
        Debug.Log($"Player {playerIndex} score updated: {newScore}");
    }

    //  STEP 3 — Match ends, submit to blockchain 
    // Only State Authority submits scores
    // Call this from your Photon match end event
    public async void OnMatchEnd(int matchId)
    {
        // ONLY the host/state authority submits scores
        //if (!HasStateAuthority) 
        //{
        //    Debug.Log("Not state authority — skipping submitScores");
        //    return;
        //}

        Debug.Log("Match ended! Submitting scores to blockchain...");

        try
        {
            // Build arrays from networked variables 
            List<string> wallets = new List<string>();
            List<int> scores = new List<int>();

            for (int i = 0; i < NetworkedWalletAddresses.Length; i++)
            {
                string wallet = NetworkedWalletAddresses[i].ToString();
                int score = NetworkedScores[i];

                // Only include players with valid wallets
                if (!string.IsNullOrEmpty(wallet) && wallet != "0")
                {
                    wallets.Add(wallet);
                    scores.Add(score);
                    Debug.Log($"Player {i}: {wallet} ? Score: {score}");
                }
            }

            // Need at least 2 players to submit
            if (wallets.Count < 2)
            {
                Debug.LogError("Need at least 2 players!");
                return;
            }

            // Convert to arrays for contract call
            string[] playerAddresses = wallets.ToArray();
            int[]    playerScores    = scores.ToArray();

            //  Submit to blockchain 
            var receipt = await ThirdwebContract.Write(
                wallet:     ThirdwebManager.Instance.ActiveWallet,
                contract:   stakeToPlay,
                method:     "submitScores",
                weiValue:   0,
                parameters: new object[] 
                { 
                    matchId,         // which match
                    playerAddresses, // wallet addresses
                    playerScores     // their scores
                }
            );

            Debug.Log("Scores submitted! Winners paid!");
            Debug.Log("TX: " + receipt.TransactionHash);// Notify all players that rewards were paid
            RPC_NotifyRewardsPaid(receipt.TransactionHash);
        }
        catch (System.Exception e)
        {
            Debug.LogError("submitScores failed: " + e.Message);
        }
    }

    // STEP 4 — Notify all players of payout 
    [Rpc]
    public void RPC_NotifyRewardsPaid(string txHash)
    {
        Debug.Log("Winners have been paid! TX: " + txHash);
        // Show winners screen UI here
        // UIManager.Instance.ShowWinnersScreen();
    }
}