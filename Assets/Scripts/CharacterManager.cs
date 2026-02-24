using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    public CharacterDataBase characterDB;
    public SpriteRenderer artWorkSprite;
    
    public int selectedOption ;

    // Start is called before the first frame update
    void Start()
    {
        selectedOption = PlayerPrefs.GetInt("selectedSkin");
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateCharacter(int selectedOption)
    {
        Character character = characterDB.GetCharacter(selectedOption);
        artWorkSprite.sprite = character.characterSprite;
        
        PlayerPrefs.SetInt("selectedSkin", selectedOption);
    }
}
