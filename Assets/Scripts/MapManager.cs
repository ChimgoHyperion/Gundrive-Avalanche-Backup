using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    // used from mapselection

    [SerializeField] int SelectedMapNumber;
    public Sprite[] SelectedMapDisplaySprites;
    [SerializeField] Image CurrentSelectedMapDisplay;

   public GameObject LoadingObj;

    public const string SelectedMap = "SelectedMap";

    public TextMeshProUGUI MapNameText,MapDescriptionText;

  [SerializeField]  public Color[] textcolors;

    public void ChooseMap(int MapNumber)
    {
        SelectedMapNumber = MapNumber;
        PlayerPrefs.SetInt(SelectedMap, SelectedMapNumber);
        CurrentSelectedMapDisplay.sprite = SelectedMapDisplaySprites[SelectedMapNumber];

        switch (SelectedMapNumber)
        {
            case 0:
                
                MapNameText.text = "Greenville";
                MapNameText.color = textcolors[0];
                MapDescriptionText.text = "Droid first contact";
                break;
            case 1:
                MapNameText.text = "Rockies";
                MapNameText.color = textcolors[1];
                MapDescriptionText.text = "Rocky hills and mountains";
                break;
            case 2:
                MapNameText.text = "Zero Horizon";
                MapNameText.color = textcolors[2];
                MapDescriptionText.text = "Violet side of the Moon ";
                break;
            case 3:
                MapNameText.text = "DeepFrost";
                MapNameText.color = textcolors[3];
                MapDescriptionText.text = "Absolute Zero!";
                break;
            case 4:
                MapNameText.text = "Smoke Box";
                MapNameText.color = textcolors[4];
                MapDescriptionText.text = "Heavy Industry";
                break;
            case 5:
                MapNameText.text = "Igbo Ukwu";
                MapNameText.color = textcolors[5];
                MapDescriptionText.text = "Back to Ancient Africa";
                break;
            case 6:
                MapNameText.text = "Ancient Egypt";
                MapNameText.color = textcolors[6];
                MapDescriptionText.text = "Deep inside King Tut's tomb";
                break;

        }
    }

    public void ApproveMapSelection()
    {
        //  VersusScreen.SetActive(true);
      //  MapUIScreen.SetActive(false);
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0f); // previously 2f. till versus screen is fixed
        LoadingObj.SetActive(true);
        yield return new WaitForSeconds(4f); // short delay introduced regardless of AsyncOperation
                                             //  SceneManager.LoadScene("Offline Scene Final",LoadSceneMode.Single);

        switch (SelectedMapNumber)
        {
            case 0:
                SceneManager.LoadScene("Map 1", LoadSceneMode.Single);
                
                break;
             case 1:
                SceneManager.LoadScene("Map 2", LoadSceneMode.Single);
                break;
            case 2:
                SceneManager.LoadScene("Map 3", LoadSceneMode.Single);
                break;
            case 3:
                SceneManager.LoadScene("Map 4", LoadSceneMode.Single);
                break;
            case 4:
                SceneManager.LoadScene("Map 5", LoadSceneMode.Single);
                break;
            case 5:
                SceneManager.LoadScene("Map 6", LoadSceneMode.Single);
                break;
            case 6:
                SceneManager.LoadScene("Map 7", LoadSceneMode.Single);
                break;

        }
       
    }
}
