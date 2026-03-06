// RewardManager.cs
using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Thirdweb.Unity;

public class RewardManager : MonoBehaviour
{
    [Header("Thirdweb Engine Settings")]
    public string EngineUrl = "https://YOUR-ENGINE-URL.engine.thirdweb.com";
    public string EngineAccessToken = "YOUR_ENGINE_ACCESS_TOKEN";
    public string BackendWallet = "0xYOUR_DEV_WALLET_ADDRESS";

    [Header("Contract")]
    public string ContractAddress = "0x1A93BADBdC083C7b360500FD73432E2Ef36ddC37";
    public string ChainId = "43113";

    // ?? Call this when a player wins / earns a reward ??????????????????????
    public async void RewardWithSHCToken(int amount, string playerAddress = null)
    {
        try
        {
            if (string.IsNullOrEmpty(playerAddress))
                playerAddress = await ThirdwebManager.Instance.ActiveWallet.GetAddress();

            string amountInWei = (new System.Numerics.BigInteger(amount)
                                 * System.Numerics.BigInteger.Pow(10, 18)).ToString();

            string json = $@"{{
                ""functionName"": ""rewardPlayer"",
                ""args"": [""{playerAddress}"", ""{amountInWei}""]
            }}";

            string url = $"{EngineUrl}/contract/{ChainId}/{ContractAddress}/write";

            // ?? Use the awaitable helper instead of direct await ???????????
            string response = await SendPostRequest(url, json, EngineAccessToken, BackendWallet);

            Debug.Log($"[Reward] ? Sent {amount} SHC to {playerAddress} | {response}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[Reward] ? {e.Message}");
        }
    }

    // ?? Awaitable web request helper ???????????????????????????????????????
    // Wraps UnityWebRequest in a Task so it works with async/await
    private Task<string> SendPostRequest(string url, string json, string accessToken, string backendWallet)
    {
        var tcs = new TaskCompletionSource<string>();

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {accessToken}");
        request.SetRequestHeader("x-backend-wallet-address", backendWallet);

        var operation = request.SendWebRequest();

        // Called by Unity when the request finishes
        operation.completed += _ =>
        {
            if (request.result == UnityWebRequest.Result.Success)
            {
                tcs.SetResult(request.downloadHandler.text);
            }
            else
            {
                tcs.SetException(new Exception(
                    $"HTTP {request.responseCode}: {request.downloadHandler.text}"
                ));
            }
            request.Dispose();
        };

        return tcs.Task;
    }
}