using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused;
    
    public GameObject PausemenuUI;

  //  public GameObject loadingPanel;

  /*  public void Transition()
    {
        StartCoroutine(transition());
    }*/
  /*  IEnumerator transition()
    {
        loadingPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        loadingPanel.SetActive(false);
    }*/
    private void Start()
    {
       
    }
    // Update is called once per frame
    void Update()
    {
       
    }

    public void Pause()
    {
        PausemenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void PauseMultiplayer()// the timescale shouldnt be zero
    {
        PausemenuUI.SetActive(true);
    }
    public  void Resume()
    {
        PausemenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
       
    }
    public void quit()
    {
        PausemenuUI.SetActive(false);
      //  Transition();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Map Menu");
    }
    public void quitToMoreLevels()
    {
        PausemenuUI.SetActive(false);
       // Transition();
        Time.timeScale = 1f;
        SceneManager.LoadScene("More Levels Menu");
    }
    public void Restart()
    {
        PausemenuUI.SetActive(false);
      //  Transition();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex );
    }

    public void EndMultiPlayerMatch()
    {
        FusionNetworkManager.networkManagerInstance.LeaveSession();
    }
}
