using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinChanger : MonoBehaviour
{
    public CharacterDataBase characterDB;
    public SpriteRenderer artWorkSprite;
    private int selectedOption;
    // Start is called before the first frame update
    void Start()
    {
        selectedOption = PlayerPrefs.GetInt("selectedSkin");
        UpdateSkin(selectedOption);
    }
    private void UpdateSkin(int selectedOption)
    {
        Character character = characterDB.GetCharacter(selectedOption);
        artWorkSprite.sprite = character.characterSprite;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
