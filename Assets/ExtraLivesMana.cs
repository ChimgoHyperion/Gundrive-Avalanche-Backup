using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraLivesMana : MonoBehaviour
{
    public GameObject deathPanel,leaderBoardPanel;
    public GameObject Player,EnemyDeath;
    public int ExtraLivesNumber;
    public Button RessurectBtn;
    public Text ExtraLivesNumText;
    public int HasBeenGifted;
    // Start is called before the first frame update
    void Start()
    {
        ExtraLivesNumber = PlayerPrefs.GetInt("ExtraLivesNumber");
        ExtraLivesNumText.text = ExtraLivesNumber.ToString();
        HasBeenGifted = PlayerPrefs.GetInt("HasBeenGifted");
        ExtraLivesGifter();
    }

    // Update is called once per frame
    void Update()
    {
        ExtraLivesNumber = PlayerPrefs.GetInt("ExtraLivesNumber");
        ExtraLivesNumText.text = ExtraLivesNumber.ToString();
        if (ExtraLivesNumber < 1)
        {
            RessurectBtn.interactable = false;
        }
        else
        {
            RessurectBtn.interactable = true;
        }
    }

    public void Ressurect()
    {
        int newLiveNumber = ExtraLivesNumber - 1;
        PlayerPrefs.SetInt("ExtraLivesNumber", newLiveNumber);
        Player.GetComponent<MovementandShooting>().ActivateShield();
        EnemyDeath.SetActive(true);
        StartCoroutine(WaitBeforeRestore());

        deathPanel.SetActive(false);
        leaderBoardPanel.SetActive(false);
        Player.SetActive(true);
        Player.GetComponent<MovementandShooting>().RandomPosition();
        Player.GetComponent<MovementandShooting>().health = 100;
        Player.GetComponent<MovementandShooting>().boostAmount = 30;


      

    }

    IEnumerator WaitBeforeRestore()
    {
        yield return new WaitForSeconds(4f);
        EnemyDeath.SetActive(false);
       
    }

    // this ensures that once the player has been gifted 3 extra lives once , it cannot happen again
    // during the gamePlay or restarting the game . we are doing this using PlayerPrefs asin Tutorial manager
    public void ExtraLivesGifter()
    {
        if (HasBeenGifted == 0)
        {
            PlayerPrefs.SetInt("ExtraLivesNumber", 3);

            PlayerPrefs.SetInt("HasBeenGifted", 1);
        }
    }
}
