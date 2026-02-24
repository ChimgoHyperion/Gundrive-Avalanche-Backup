using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class ColourManager : MonoBehaviour
{
    public SpriteRenderer Sr;
    public GameObject playerColour;
    public Image SelectedColour;
    public Color[] colors;

    public int colourNumber;
    public static ColourManager instance;

   /* private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(Sr);
    }*/

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SelectColor1()
    {
        PlayerPrefs.SetInt("colourNumber", 0);
        SelectedColour.color = colors[0];
    }
    public void SelectColor2()
    {
        PlayerPrefs.SetInt("colourNumber", 1);
        SelectedColour.color = colors[1];
    }
    public void SelectColor3()
    {
        PlayerPrefs.SetInt("colourNumber", 2);
        SelectedColour.color = colors[2];
    }
    public void SelectColor4()
    {
        PlayerPrefs.SetInt("colourNumber", 3);
        SelectedColour.color = colors[3];
    }
    public void SelectColor5()
    {
        PlayerPrefs.SetInt("colourNumber", 4);
        SelectedColour.color = colors[4];
    }
    public void SelectColor6()
    {
        PlayerPrefs.SetInt("colourNumber", 5);
        SelectedColour.color = colors[5];
    }
    public void SelectColor7()
    {
        PlayerPrefs.SetInt("colourNumber", 6);
        SelectedColour.color = colors[6];
    }
    public void SelectColor8()
    {
        PlayerPrefs.SetInt("colourNumber", 7);
        SelectedColour.color = colors[7];
    }
    public void SelectColor9()
    {
        PlayerPrefs.SetInt("colourNumber", 8);
        SelectedColour.color = colors[8];
    }
    public void SelectColor10()
    {
        PlayerPrefs.SetInt("colourNumber", 9);
        SelectedColour.color = colors[9];
    }

    // Update is called once per frame
    void Update()
    {
       colourNumber = PlayerPrefs.GetInt("colourNumber");
        switch (colourNumber)
        {
            case 0:
                Sr.color = colors[0];
                break;
            case 1:
                Sr.color = colors[1];
                break;
            case 2:
                Sr.color = colors[2];
                break;
            case 3:
                Sr.color = colors[3];
                break;
            case 4:
                Sr.color = colors[4];
                break;
            case 5:
                Sr.color = colors[5];
                break;
            case 6:
                Sr.color = colors[6];
                break;
            case 7:
                Sr.color = colors[6];
                break;
            case 8:
                Sr.color = colors[6];
                break;
            case 9:
                Sr.color = colors[6];
                break;
           



        }
        SelectedColour.color = Sr.color;
    }
    public void EquipAndPlay()
    {
        PlayerPrefs.Save();
        //PrefabUtility.SaveAsPrefabAsset(playerColour, "Assets/selected colour.prefab");
        SceneManager.LoadScene("Map 1");
      
    }
}
