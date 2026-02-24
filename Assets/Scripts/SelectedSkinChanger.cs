using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedSkinChanger : MonoBehaviour
{
    public CharacterDataBase characterDB;
    public Image image;
    private int selectedOption;
    // Start is called before the first frame update
    void Start()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
        UpdateSkin(selectedOption);
    }
    private void UpdateSkin(int selectedOption)
    {
        Character character = characterDB.GetCharacter(selectedOption);
        image.sprite = character.characterSprite;
    }
}
