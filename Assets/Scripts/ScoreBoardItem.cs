using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using TMPro;

public class ScoreBoardItem : MonoBehaviour
{
    [Header("The variables that will indicate value change in player UI ")]
    public float Score;// updated by healthscript through statemanager
    public TextMeshProUGUI ScoreText;// updated in this script
                
   
    public TextMeshProUGUI playerUserNameText;

    public string playerUserName;

    public Image SelectedSkinImage;

    public Sprite selectedSkinSprite;
  
    private void Start()
    {
        GetComponent<RectTransform>().SetAsFirstSibling();
    }
    private void Update()
    {
        
        playerUserNameText.text = playerUserName;

        SelectedSkinImage.sprite = selectedSkinSprite;

        if(ScoreText!= null)
        ScoreText.text = Score.ToString();
       
    }
   
}
