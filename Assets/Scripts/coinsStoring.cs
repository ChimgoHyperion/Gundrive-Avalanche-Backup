using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class coinsStoring : MonoBehaviour
{
    public static coinsStoring instance;
    public int CoinsStored;
    public Text coinsStoredText;
    public Text enemiesKilledText;
    public int HighScore;
    [SerializeField] Text highScoreText;
    [SerializeField] Text highscoreMainMenu;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
   
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        CoinsStored = CoinBalanceHolder.Instance.virtualCurrencyBalance;
        if (coinsStoredText != null)
        {
            coinsStoredText.text = ":" + CoinsStored.ToString();
        }
        if (enemiesKilledText != null)
        {
            enemiesKilledText.text = "Droids Killed :" + PlayerPrefs.GetInt("EnemiesKilled");
        }
        if (highScoreText != null)
        {
            highScoreText.text = "HighScore :" + HighScore.ToString();
        }

        HighScore = PlayerPrefs.GetInt("Highscore");

        if (highscoreMainMenu != null)
        {
            highscoreMainMenu.text = "HighScore :" + HighScore.ToString();
        }
       

    }
}
