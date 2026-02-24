using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

[Serializable]
public class SimpleNftDatas
{
    public string name;
    public string description;
    public string imageUrl;
    public string tokenId;
}

[Serializable]
public class AlchemyContract
{
    public string address;
    public string name;
    public string symbol;
    public string tokenType;
}

[Serializable]
public class AlchemyImage
{
    public string cachedUrl;
    public string thumbnailUrl;
    public string pngUrl;
    public string originalUrl;
}

[Serializable]
public class AlchemyRaw
{
    public AlchemyMetadata metadata;
}

[Serializable]
public class AlchemyMetadata
{
    public string name;
    public string description;
    public string image;
}

[Serializable]
public class AlchemyNftSimple
{
    public AlchemyContract contract;
    public string tokenId;
    public string tokenType;
    public string name;
    public string description;
    public AlchemyImage image;
    public AlchemyRaw raw;
}

[Serializable]
public class AlchemyV3ResponseSimple
{
    public AlchemyNftSimple[] ownedNfts;
    public string pageKey;
    public int totalCount;
}

public static class GDNftFetcher
{
    private static string alchemyKey = "YOUR_ALCHEMY_KEY_HERE"; // Set this to your Alchemy API key
    
    /// <summary>
    /// Set the Alchemy API key
    /// </summary>
    /// <param name="key">Your Alchemy API key</param>
    public static void SetAlchemyKey(string key)
    {
        alchemyKey = key;
    }
    
    /// <summary>
    /// Fetches GD NFTs for a wallet and returns simplified data (name, description, image)
    /// </summary>
    /// <param name="ownerAddress">Wallet address to fetch NFTs for</param>
    /// <returns>List of SimpleNftData containing name, description, and image URL</returns>
    public static async Task<List<SimpleNftDatas>> GetGDNftsAsync(string ownerAddress)
    {
        if (string.IsNullOrEmpty(ownerAddress) || string.IsNullOrEmpty(alchemyKey))
        {
            Debug.LogError("Owner address or Alchemy key is empty.");
            return new List<SimpleNftDatas>();
        }

        var collected = new List<AlchemyNftSimple>();
        string pageKey = null;
        string baseUrl = $"https://testnet.hashio.io/api/getNFTsForOwner";


        try
        {
            do
            {
                var sb = new StringBuilder(baseUrl);
                sb.Append($"?owner={Uri.EscapeDataString(ownerAddress)}");
                sb.Append("&withMetadata=true");
                if (!string.IsNullOrEmpty(pageKey))
                    sb.Append($"&pageKey={Uri.EscapeDataString(pageKey)}");

                using (UnityWebRequest uwr = UnityWebRequest.Get(sb.ToString()))
                {
                    uwr.timeout = 30;
                    var asyncOp = uwr.SendWebRequest();
                    
                    // Wait for request to complete
                    while (!asyncOp.isDone)
                        await Task.Yield();
                    
                    // Check for errors
                    if (uwr.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError($"Alchemy request failed: {uwr.error}");
                        break;
                    }

                    string json = uwr.downloadHandler?.text;
                    if (string.IsNullOrEmpty(json))
                        break;

                    AlchemyV3ResponseSimple resp = null;
                    try
                    {
                        resp = JsonUtility.FromJson<AlchemyV3ResponseSimple>(json);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Alchemy JSON parse failed: {ex.Message}\nRaw: {json}");
                        break;
                    }

                    if (resp?.ownedNfts != null && resp.ownedNfts.Length > 0)
                    {
                        collected.AddRange(resp.ownedNfts);
                    }

                    pageKey = resp?.pageKey;
                }

            } while (!string.IsNullOrEmpty(pageKey));
        }
        catch (Exception ex)
        {
            Debug.LogError($"GetGDNftsAsync error: {ex.Message}");
            return new List<SimpleNftDatas>();
        }

        // Filter for tokenType == "ERC721" and contract.symbol == "GD"
        var filtered = collected.FindAll(nft =>
            nft != null &&
            string.Equals(nft.tokenType, "ERC721", StringComparison.OrdinalIgnoreCase) &&
            nft.contract != null &&
            string.Equals(nft.contract.symbol, "GD", StringComparison.OrdinalIgnoreCase)
        );

        // Convert to SimpleNftData - only include NFTs with both name and description
        var result = new List<SimpleNftDatas>();
        foreach (var nft in filtered)
        {
            // Get name and description
            string nftName = GetNftName(nft);
            string nftDescription = GetNftDescription(nft);
            
            // Only add to result if both name and description are present and not empty
            if (!string.IsNullOrEmpty(nftName))
            {
                var simpleNft = new SimpleNftDatas();
                simpleNft.name = nftName;
                simpleNft.description = nftDescription;
                simpleNft.imageUrl = GetNftImageUrl(nft);
                simpleNft.tokenId = nft.tokenId ?? "";
                
                result.Add(simpleNft);
            }
            else
            {
                Debug.Log($"Skipping NFT #{nft.tokenId} - missing name or description. Name: '{nftName}', Description: '{nftDescription}'");
            }
        }

        Debug.Log($"Found {result.Count} GD NFTs for wallet {ownerAddress}");
        for (int i = 0; i < result.Count; i++)
        {
            Debug.Log(result[i].name);
        }
        return result;
    }
    
    private static string GetNftName(AlchemyNftSimple nft)
    {
        // Try raw metadata first
        if (nft.raw?.metadata?.name != null && !string.IsNullOrEmpty(nft.raw.metadata.name))
            return nft.raw.metadata.name;
            
        // Fallback to direct name
        if (!string.IsNullOrEmpty(nft.name))
            return nft.name;
            
        // Return null if no valid name found
        return null;
    }
    
    private static string GetNftDescription(AlchemyNftSimple nft)
    {
        // Try raw metadata first
        if (nft.raw?.metadata?.description != null && !string.IsNullOrEmpty(nft.raw.metadata.description))
            return nft.raw.metadata.description;
            
        // Fallback to direct description
        if (!string.IsNullOrEmpty(nft.description))
            return nft.description;
            
        // Return null if no valid description found
        return null;
    }
    
    private static string GetNftImageUrl(AlchemyNftSimple nft)
    {
        // Try cached URL first (best quality)
        if (nft.image?.cachedUrl != null && !string.IsNullOrEmpty(nft.image.cachedUrl))
            return nft.image.cachedUrl;
            
        // Try PNG URL
        if (nft.image?.pngUrl != null && !string.IsNullOrEmpty(nft.image.pngUrl))
            return nft.image.pngUrl;
            
        // Try original URL
        if (nft.image?.originalUrl != null && !string.IsNullOrEmpty(nft.image.originalUrl))
            return nft.image.originalUrl;
            
        // Try raw metadata image
        if (nft.raw?.metadata?.image != null && !string.IsNullOrEmpty(nft.raw.metadata.image))
            return nft.raw.metadata.image;
            
        // Try thumbnail as last resort
        if (nft.image?.thumbnailUrl != null && !string.IsNullOrEmpty(nft.image.thumbnailUrl))
            return nft.image.thumbnailUrl;
            
        // No image found
        return null;
    }
    
    /// <summary>
    /// Get NFT data as JSON string for debugging
    /// </summary>
    /// <param name="ownerAddress">Wallet address</param>
    /// <returns>JSON string of NFT data</returns>
    public static async Task<string> GetGDNftsAsJsonAsync(string ownerAddress)
    {
        var nfts = await GetGDNftsAsync(ownerAddress);
        
        if (nfts == null || nfts.Count == 0)
            return "[]";

        var sb = new StringBuilder();
        sb.Append('[');
        for (int i = 0; i < nfts.Count; i++)
        {
            string itemJson = JsonUtility.ToJson(nfts[i]);
            sb.Append(itemJson);
            if (i < nfts.Count - 1) sb.Append(',');
        }
        sb.Append(']');
        
        return sb.ToString();
    }
}