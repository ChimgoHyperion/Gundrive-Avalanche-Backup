using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int Highscore =0; // will be used in stats scene
 
    public Text ScoreText;
    public int Score = 0;
  
    int enemiesKilled;
    int totalEnemiesKilled;

    // death panel stats
    public Text deathPanelScore;
    public Text deathpanelHighscore;
    public Text deathpanelcoinsCollected;
    public Text deathpaneltotalCoins;
    int currentCoins;

    // HighScore and Rank Increase indicators
    public GameObject HighscoreIndicator;
    public GameObject NewRankIndicator;


    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
      
        Highscore = PlayerPrefs.GetInt("Highscore", 0);
        ScoreText.text = "Score :" + Score.ToString();
      
        totalEnemiesKilled = PlayerPrefs.GetInt("EnemiesKilled");

       
    }
    // Update is called once per frame
    void Update()
    {

        deathpanelHighscore.text = "Highscore:" + Highscore.ToString();
        deathPanelScore.text = ScoreText.text;
        deathpaneltotalCoins.text = "Total:" + PlayerPrefs.GetInt("Coins");
    }
    public void AddPoints()
    {
        Score += 10 ;
        enemiesKilled++;
      
      
        ScoreText.text = "Score :" + Score.ToString();
        if (Highscore < Score)
        {
            // update highscore
            PlayerPrefs.SetInt("Highscore", Score);
          
        }

    }
  
    public void SaveEnemiesKilled()
    {
        PlayerPrefs.SetInt("EnemiesKilled", totalEnemiesKilled + enemiesKilled);


        //  PlayerPrefs.SetInt("Coins", coinsStored + CoinsCollected);
        PlayerPrefs.Save();
    }

  

  
}
