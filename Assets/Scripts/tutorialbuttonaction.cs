using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialbuttonaction : MonoBehaviour
{
    TutorialPlayer player;
     TutorialManager tutManager;

    public void Start()
    {
        tutManager = GameObject.FindGameObjectWithTag("Tutorialmanager").GetComponent<TutorialManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<TutorialPlayer>();
    }
    public void increaseHealth()
    {
        player.IncreaseHealth();
        tutManager.hasPressedhealthbtn = true;
        Destroy(gameObject);


    }
}
