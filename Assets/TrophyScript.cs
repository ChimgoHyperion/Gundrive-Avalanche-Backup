using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrophyScript : MonoBehaviour
{
    public Image TrophyImage, TrophyImage2;
    public Text TrophyText,TrophyText2;
    public int Highscore;

    public Color Bronze, Silver, Gold, Legendary;
    // Start is called before the first frame update
    void Start()
    {
        Highscore = PlayerPrefs.GetInt("Highscore");
    }

    // Update is called once per frame
    void Update()
    {
        if(Highscore>=0 && Highscore <= 500)
        {
            TrophyImage.color = Bronze;
            TrophyImage2.color = Bronze; // the 2 signifies the second image and text

            TrophyText.color = Bronze;
            TrophyText.text = "Rookie";

            TrophyText2.color = Bronze;
            TrophyText2.text = "Rookie";

        }

        if (Highscore > 500 && Highscore <= 1200)
        {
            TrophyImage.color = Silver;
            TrophyImage2.color = Silver;
            TrophyText.color = Silver;
            TrophyText.text = "Captain";

            TrophyText2.color = Silver;
            TrophyText2.text = "Captain";
        }


        if (Highscore > 1200 && Highscore <= 2400)
        {
            TrophyImage.color = Gold;
            TrophyImage2.color = Gold; 
            TrophyText.color = Gold;
            TrophyText.text = "Expert";

            TrophyText2.color = Gold;
            TrophyText2.text = "Expert";
        }


        if (Highscore > 2400)
        {
            TrophyImage.color = Legendary;
            TrophyImage2.color = Legendary;
            TrophyText.color = Legendary;
            TrophyText.text = "Legend";

            TrophyText2.color = Legendary;
            TrophyText2.text = "Legendary";
        }
    }

}
