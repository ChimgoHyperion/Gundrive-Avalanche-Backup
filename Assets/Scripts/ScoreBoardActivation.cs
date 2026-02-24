using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;


public class ScoreBoardActivation : MonoBehaviour
{

    public static ScoreBoardActivation Instance;

    [SerializeField] private CanvasGroup scoreboardGroup;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        scoreboardGroup.alpha=0;
    }

    public void ShowScoreboard()
    {
        scoreboardGroup.alpha = 1;
        
    }

    public void HideScoreboard()
    {
        scoreboardGroup.alpha = 0;
    }
}
