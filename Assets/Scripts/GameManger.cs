using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
   // public GameObject selectedSkin;
    //public GameObject selectedColour;
    public GameObject Player;
    private Sprite playerSprite;
    private Color playerColor;
    public Vector3 newScale;
    public List<Sprite> skins = new List<Sprite>();
    public int skinNumber;
    // Start is called before the first frame update
    void Start()
    {
        //  playerSprite = selectedSkin.GetComponent<SpriteRenderer>().sprite;
        //playerSprite = SkinManager.instnce.Sr.sprite;

        Player.transform.localScale = newScale;

      //  playerColor = ColourManager.instance.Sr.color;

        Player.GetComponent<SpriteRenderer>().sprite = playerSprite;

      //  Player.GetComponent<SpriteRenderer>().color = playerColor;
        
    }

    public void ReloadCurrentScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex); // needs work
    }
    public void ShowAds()
    {
        // show ad 
        // increase coins buy 100 or so (on ad completed)

    }
    // Update is called once per frame
    void Update()
    {

        Player.GetComponent<SpriteRenderer>().sprite = playerSprite;

       // Player.GetComponent<SpriteRenderer>().color = playerColor;
        skinNumber = PlayerPrefs.GetInt("skinNumber");
        switch (skinNumber)
        {
            case 0:
                playerSprite= skins[0];
                break;
            case 1:
              playerSprite = skins[1];
                break;
            case 2:
                playerSprite = skins[2];
                break;
            case 3:
                playerSprite = skins[3];
                break;
            case 4:
                playerSprite = skins[4];
                break;
            case 5:
                playerSprite = skins[5];
                break;
            case 6:
                playerSprite = skins[6];
                break;




        }
        
    }
}
