using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class backgroundSpriteManager : MonoBehaviour
{
    public backgroundSpriteDB backgroundDB;
    public Image backgroundsprite;
    private int selectedSprite;
    void Start()
    {
        selectedSprite = PlayerPrefs.GetInt("selectedSprite");
    }

    public void UpdateSprite(int selectedSprite)
    {
        backgroundSprite bgSprite = backgroundDB.GetBackgroundSprite(selectedSprite);
        backgroundsprite.sprite = bgSprite.backgroundImageSprite;
        PlayerPrefs.SetInt("selectedSprite", selectedSprite);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
