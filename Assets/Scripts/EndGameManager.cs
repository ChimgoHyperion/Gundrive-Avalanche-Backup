using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    public GameObject Deathpanel, SpinWheelPanel, XPRewardsPanel, ClearEnemyObj;

     MovementandShooting Player;
    EnemyManager enemyManager;
    public PlayfabManager playfabManager;
    public static EndGameManager instance;
    public enum EndGameStep
    {
        SpinForRewards, XPRewards, FinalDeathPanel
    }
    public EndGameStep currentStep= EndGameStep.SpinForRewards;

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        enemyManager.ReturnHealth();
        
       
    }
    public void ExtraLifeRespawn()
    {
        ClearEnemyObj.SetActive(true);
        // disable spin wheel panel
        SpinWheelPanel.GetComponent<CanvasGroup>().alpha = 0;
        SpinWheelPanel.GetComponent<CanvasGroup>().interactable = false;
        SpinWheelPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        // disable clearenemy obj
        ClearEnemyObj.SetActive(false);
        // make player to be able to move once again

        FindObjectOfType<MovementandShooting>().Resurrect();
    }
    public void EndGame()
    {
        if (LoggedInManager.Instance.isLoggedIn)
        {
            playfabManager.SendLeaderboard(PlayerPrefs.GetInt("Highscore"));
            ScoreManager.instance.SaveEnemiesKilled();
        }
      
        //ScoreManager.instance.SaveCoins(); // this method also has the code that send the score to the server leaderBoard
        StartCoroutine(wait());
    }
    IEnumerator wait()
    {


        // activate spin panel
       
        yield return new WaitForSeconds(0.4f);
        if (LoggedInManager.Instance.isLoggedIn)
        {
            SpinWheelPanel.GetComponent<CanvasGroup>().alpha = 1;
            SpinWheelPanel.GetComponent<CanvasGroup>().interactable = true;
            SpinWheelPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else
        {
            Deathpanel.SetActive(true);
        }
       
    }
    public void CompletedSpin()
    {
        //  CompleteStep(EndGameStep.SpinForRewards); // correct but paused for now

        CompleteStep(EndGameStep.XPRewards);
    }
    public void CompletedXPRewardScreen()
    {
        CompleteStep(EndGameStep.FinalDeathPanel);
    }
    public void CompleteStep(EndGameStep step) // call when an event is complete
    {
        // Transition to the next step based on your game design.
       
        if (step == EndGameStep.SpinForRewards)
            currentStep = EndGameStep.XPRewards;
        else if (step == EndGameStep.XPRewards)
            currentStep = EndGameStep.FinalDeathPanel;
       

        ExecuteStep(currentStep);
    }
    public void ExecuteStep(EndGameStep step)
    {
        switch (step)
        {
            case EndGameStep.SpinForRewards:
                // enable spinfor rewards
                SpinWheelPanel.GetComponent<CanvasGroup>().alpha = 1;
                SpinWheelPanel.GetComponent<CanvasGroup>().interactable = true;
                SpinWheelPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;


                break;
            case EndGameStep.XPRewards:
                //disable spinfor rewards, wait a while, then enable xprewards;
                SpinWheelPanel.GetComponent<CanvasGroup>().alpha = 0;
                SpinWheelPanel.GetComponent<CanvasGroup>().interactable=false;
                SpinWheelPanel.GetComponent<CanvasGroup>().blocksRaycasts=false;

                XPRewardsPanel.GetComponent<CanvasGroup>().alpha = 1;
                XPRewardsPanel.GetComponent<CanvasGroup>().interactable = true;
                XPRewardsPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
                XPRewardsPanel.GetComponent<XPandRewardsManager>().OnGameEnd();
              
                break;
            case EndGameStep.FinalDeathPanel:
                //disable xprewards and enable deathpanel
                XPRewardsPanel.GetComponent<CanvasGroup>().alpha = 0;
                XPRewardsPanel.GetComponent<CanvasGroup>().interactable = false;
                XPRewardsPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
                XPRewardsPanel.GetComponent<XPandRewardsManager>().OnGameEnd();

                // temporary
                SpinWheelPanel.GetComponent<CanvasGroup>().alpha = 0;
                SpinWheelPanel.GetComponent<CanvasGroup>().interactable = false;
                SpinWheelPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

                Deathpanel.SetActive(true);
                break;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<MovementandShooting>();
        enemyManager = FindObjectOfType<EnemyManager>();
        
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    

}
