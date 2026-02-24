using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
// using WalletConnectUnity.Core.Utils;
using System.Numerics;
using Nethereum.Web3;
using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts.ContractHandlers;
using UnityEngine.Purchasing.MiniJSON;

[System.Serializable]
public class NFTMetadata
{
    public string name;
    public string description;
    public string image;
    public Dictionary<string, object> attributes;
}

// public class TokenDetails
// {
//     public string TokenId { get; set; }
//     public string Owner { get; set; }
//     public string TokenURI { get; set; }
//     public string Name { get; set; }
//     public string Image { get; set; }
//     public string Description { get; set; }
//     public NFTMetadata Metadata { get; set; }
// }

public class SimpleNftData
{
    public string name;
    public string description;
    public string imageUrl;
    public string tokenId;
}

[Function("ownerOf", "address")]
public class OwnerOfFunction : FunctionMessage
{
    [Parameter("uint256", "tokenId", 1)]
    public BigInteger TokenId { get; set; }
}

[Function("tokenURI", "string")]
public class TokenURIFunction : FunctionMessage
{
    [Parameter("uint256", "tokenId", 1)]
    public BigInteger TokenId { get; set; }
}

[Function("tokensOfOwner", "uint256[]")]
public class TokensOfOwnerFunction : FunctionMessage
{
    [Parameter("address", "owner", 1)]
    public string Owner { get; set; }
}

public class GDNFTFetchers : MonoBehaviour
{
    [Header("Contract Configuration")]
    [SerializeField] private static string contractAddress = "0xF09aC86286Ee1270ABdf8a8Ec7fC32de1Ab01cE3";
    [SerializeField] private static string rpcUrl = "https://testnet.hashio.io/api"; // Replace with your RPC URL

    [Header("Owner Address")]
    [SerializeField] private string ownerAddress = "0x19219ab0E7DBbA5B887A1f2Dc6EC3C0D10576628";

    private static Web3 web3;
    private Contract contract;

    private void Start()
    {
        InitializeWeb3();
    }

    public static async Task InitializeWeb3()
    {
        try
        {
            web3 = new Web3(rpcUrl);
            await FetchAllTokensForOwner("0x19219ab0E7DBbA5B887A1f2Dc6EC3C0D10576628");
            Debug.Log("Web3 initialized successfully");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to initialize Web3: {ex.Message}");
        }
    }

    private static async Task<NFTMetadata> FetchMetadataFromURI(string tokenURI)
    {
        try
        {
            if (string.IsNullOrEmpty(tokenURI))
            {
                Debug.LogWarning("Token URI is empty or null");
                return null;
            }

            // Handle IPFS URIs
            string resolvedURI = tokenURI;
            if (tokenURI.StartsWith("ipfs://"))
            {
                resolvedURI = tokenURI.Replace("ipfs://", "https://ipfs.io/ipfs/");
            }

            using (UnityWebRequest request = UnityWebRequest.Get(resolvedURI))
            {
                var operation = request.SendWebRequest();

                // Wait for the request to complete
                while (!operation.isDone)
                {
                    await Task.Yield();
                }

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string jsonResponse = request.downloadHandler.text;

                    try
                    {
                        // Parse the JSON using Unity's built-in JSON parser
                        var metadataDict = Json.Deserialize(jsonResponse) as Dictionary<string, object>;

                        var metadata = new NFTMetadata();

                        if (metadataDict.ContainsKey("name"))
                            metadata.name = metadataDict["name"].ToString();

                        if (metadataDict.ContainsKey("description"))
                            metadata.description = metadataDict["description"].ToString();

                        if (metadataDict.ContainsKey("image"))
                        {
                            metadata.image = metadataDict["image"].ToString();
                            // Handle IPFS image URIs
                            if (metadata.image.StartsWith("ipfs://"))
                            {
                                metadata.image = metadata.image.Replace("ipfs://", "https://ipfs.io/ipfs/");
                            }
                        }

                        if (metadataDict.ContainsKey("attributes"))
                            metadata.attributes = metadataDict["attributes"] as Dictionary<string, object>;

                        return metadata;
                    }
                    catch (Exception parseEx)
                    {
                        Debug.LogError($"Error parsing metadata JSON: {parseEx.Message}");
                        return null;
                    }
                }
                else
                {
                    Debug.LogError($"Failed to fetch metadata from {resolvedURI}: {request.error}");
                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error fetching metadata from URI {tokenURI}: {ex.Message}");
            return null;
        }
    }

    public static async Task<SimpleNftData> GetTokenDetails(string tokenId)
    {
        try
        {
            var handler = web3.Eth.GetContractHandler(contractAddress);

            // Get owner
            var ownerFunction = new OwnerOfFunction()
            {
                TokenId = BigInteger.Parse(tokenId)
            };
            var owner = await handler.QueryAsync<OwnerOfFunction, string>(ownerFunction);

            // Get token URI
            var tokenURIFunction = new TokenURIFunction()
            {
                TokenId = BigInteger.Parse(tokenId)
            };
            var tokenURI = await handler.QueryAsync<TokenURIFunction, string>(tokenURIFunction);

            // Fetch metadata from token URI
            var metadata = await FetchMetadataFromURI(tokenURI);

            var tokenDetails = new SimpleNftData
            {
                tokenId = tokenId,
                description = metadata.description,
                imageUrl = metadata.image,
                name = metadata.name
            };

            // Set individual properties for easier access
            if (metadata != null)
            {
                tokenDetails.name = metadata.name;
                tokenDetails.imageUrl = metadata.image;
                tokenDetails.description = metadata.description;
            }

            return tokenDetails;
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("ERC721: invalid token ID") || ex.Message.Contains("owner query for nonexistent token"))
            {
                Debug.Log($"Token {tokenId} does not exist");
            }
            else
            {
                Debug.LogError($"Error getting token details for {tokenId}: {ex.Message}");
            }
            return null;
        }
    }

    public static async Task<List<BigInteger>> GetTokensOfOwner(string ownerAddr)
    {
        try
        {
            var handler = web3.Eth.GetContractHandler(contractAddress);

            var tokensFunction = new TokensOfOwnerFunction()
            {
                Owner = ownerAddr
            };

            var tokenIds = await handler.QueryAsync<TokensOfOwnerFunction, List<BigInteger>>(tokensFunction);

            Debug.Log($"Found {tokenIds.Count} tokens for owner {ownerAddr}");
            return tokenIds;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error getting tokens of owner {ownerAddr}: {ex.Message}");
            return new List<BigInteger>();
        }
    }

    [ContextMenu("Fetch All Token Details")]
    public async void FetchAllTokenDetails()
    {
        await FetchAllTokenDetailsAsync();
    }

    public async Task FetchAllTokenDetailsAsync()
    {
        try
        {
            Debug.Log($"Fetching tokens for owner: {ownerAddress}");

            var tokenIds = await GetTokensOfOwner(ownerAddress);

            if (tokenIds.Count == 0)
            {
                Debug.Log("No tokens found for this owner");
                return;
            }

            var tokenIdStrings = new List<string>();
            foreach (var tokenId in tokenIds)
            {
                tokenIdStrings.Add(tokenId.ToString());
            }

            Debug.Log($"Owner's tokens: [{string.Join(", ", tokenIdStrings)}]");

            // Fetch details for each token
            foreach (var tokenId in tokenIds)
            {
                await GetTokenDetails(tokenId.ToString());
                Debug.Log(""); // Add spacing between tokens
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in FetchAllTokenDetails: {ex.Message}");
        }
    }

    [ContextMenu("Fetch Single Token Details")]
    public async void FetchSingleTokenDetails()
    {
        // Example: fetch details for token ID 1
        await GetTokenDetails("1");
    }

    // Public method to fetch details for a specific token ID from external scripts
    public async Task<SimpleNftData> FetchTokenDetailsById(string tokenId)
    {
        return await GetTokenDetails(tokenId);
    }

    // Public method to fetch details for a specific owner from external scripts
    public static async Task<List<SimpleNftData>> FetchAllTokensForOwner(string ownerAddr)
    {
        var tokenDetails = new List<SimpleNftData>();

        try
        {
            var tokenIds = await GetTokensOfOwner(ownerAddr);

            foreach (var tokenId in tokenIds)
            {
                var details = await GetTokenDetails(tokenId.ToString());
                if (details != null)
                {
                    tokenDetails.Add(details);
                }

                // Optional: Add a small delay between requests to avoid overwhelming servers
                await Task.Delay(100);
            }

            Debug.Log($"Fetched complete details for {tokenDetails.Count} tokens");

            // Log summary of fetched tokens
            // foreach (var token in tokenDetails)
            // {
            //     Debug.Log($"Token {token.tokenId}: {token.name} - {token.imageUrl}");
            // }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error fetching all tokens for owner {ownerAddr}: {ex.Message}");
        }

        return tokenDetails;
    }

    // Method to update owner address at runtime
    public void SetOwnerAddress(string newOwnerAddress)
    {
        ownerAddress = newOwnerAddress;
    }

    // Method to update contract address at runtime
    public void SetContractAddress(string newContractAddress)
    {
        contractAddress = newContractAddress;
    }

    // Utility method to download and cache NFT images (optional)
    public static async Task<Texture2D> DownloadNFTImage(string imageUrl)
    {
        try
        {
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl))
            {
                var operation = request.SendWebRequest();

                while (!operation.isDone)
                {
                    await Task.Yield();
                }

                if (request.result == UnityWebRequest.Result.Success)
                {
                    return DownloadHandlerTexture.GetContent(request);
                }
                else
                {
                    Debug.LogError($"Failed to download image from {imageUrl}: {request.error}");
                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error downloading image from {imageUrl}: {ex.Message}");
            return null;
        }
    }
}