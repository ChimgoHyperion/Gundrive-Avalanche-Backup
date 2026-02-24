using PlayFab;
using PlayFab.AuthenticationModels;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class XPandRewardsManager : MonoBehaviour
{
    // Start is called before the first frame update

    // Idea is simple , upon game over add accumulated score in match to the total xp gotten and upgrade if necessary. for each xp level, a 
    // reward is attached. 
   
    public float TotalXP;
    [SerializeField] UnityEngine.UI.Slider XPslider;
    [SerializeField] public float currentXP, maxXP, currentLevel;

    [SerializeField] TextMeshProUGUI TotalXPText, currentXPOvermaxXPText, CurrentLevelText;

    public float lerpSpeed = 2f; // Adjust for faster/slower fill

    private Coroutine lerpRoutine;
    
    void Start()
    {
        GetPlayerStats();
    }

    // Update is called once per frame
    void Update()
    {
      
        // visual representations
        currentXPOvermaxXPText.text = Mathf.RoundToInt(currentXP).ToString() + "/" + maxXP.ToString();
        TotalXPText.text = "+" + TotalXP.ToString() + "XP";
        CurrentLevelText.text ="LV."+ currentLevel.ToString();
    }

    public void OnGameEnd()
    {

        TotalXP = ScoreManager.instance.Score;
        HandleXPChange(TotalXP);
    }
    public void HandleXPChange(float newXP)
    {
        // animate it
        currentXP += newXP;
        UpdateXPBar(currentXP, maxXP);// animate slider


        
     //   PlayerPrefs.SetFloat("CurrentXP", currentXP); // save level
        if (currentXP >= maxXP)
        {
            LevelUp();// LevelUp(newXP);
        }
    }
    void LevelUp()
    {
        currentLevel++;
      //  PlayerPrefs.SetFloat("CurrentLevel", currentLevel);
        maxXP += 1500;
      //  PlayerPrefs.SetFloat("CurrentMaxXP", maxXP);
        currentXP = 0; // 0 means no carry over // currentXP - TargetXP; while this formula accounts for carry over

        UpdatePlayerStats(currentXP, maxXP, currentLevel);// update to backend
    }

    public void UpdatePlayerStats(float currentXP, float maxXP, float currentLevel)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate { StatisticName = "CurrentXP", Value = Mathf.RoundToInt(currentXP * 100) },
                new StatisticUpdate { StatisticName = "MaxXP", Value = Mathf.RoundToInt(maxXP * 100) },
                new StatisticUpdate { StatisticName = "CurrentLevel", Value = Mathf.RoundToInt(currentLevel * 100) }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request,
            result => Debug.Log("Player statistics updated successfully!"),
            error => Debug.LogError("Error updating stats: " + error.GenerateErrorReport()));
    }
    public void GetPlayerStats()
    {
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(),
            result =>
            {
                 currentXP = 0f;
                 maxXP = 0f;
                 currentLevel = 0f;

                foreach (var stat in result.Statistics)
                {
                    switch (stat.StatisticName)
                    {
                        case "CurrentXP":
                            currentXP = stat.Value / 100f;
                            break;
                        case "MaxXP":
                            maxXP = stat.Value / 100f;
                            break;
                        case "CurrentLevel":
                            currentLevel = stat.Value / 100f;
                            break;
                    }
                }

                Debug.Log($"Stats Loaded - Level: {currentLevel}, XP: {currentXP}/{maxXP}");
                XPslider.maxValue = maxXP;

            },
            error => Debug.LogError("Error getting stats: " + error.GenerateErrorReport()));
    }

  

    // Call this when XP changes for Slider
    public void UpdateXPBar(float currentXP, float maxXP)
    {
        XPslider.maxValue = maxXP;

        if (lerpRoutine != null)
            StopCoroutine(lerpRoutine);

        lerpRoutine = StartCoroutine(LerpXP(currentXP));
    }

    private IEnumerator LerpXP(float targetXP)
    {
        float startXP = XPslider.value;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * lerpSpeed;
            XPslider.value = Mathf.Lerp(startXP, targetXP, t);
            yield return null;
        }

        XPslider.value = targetXP; // Ensure final value is exact
    }

    void GrantReward()
    {
        //if (Highscore > 500 && Highscore <= 1200)
        //{
        //    NewRankIndicator.SetActive(true);
        //    StartCoroutine(Disable());
        //}

        //if (Highscore > 1200 && Highscore <= 2400)
        //{
        //    NewRankIndicator.SetActive(true);
        //    StartCoroutine(Disable());
        //}

        //if (Highscore > 2400)
        //{
        //    NewRankIndicator.SetActive(true);
        //    StartCoroutine(Disable());
        //}
    }
}
