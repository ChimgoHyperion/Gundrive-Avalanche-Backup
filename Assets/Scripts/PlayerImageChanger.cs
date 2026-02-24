using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerImageChanger : MonoBehaviour
{

    public CharacterDataBase characterDB;
  
    private Image image;
    private int selectedOption;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        selectedOption = PlayerPrefs.GetInt("selectedSkin");
        UpdateSkin(selectedOption);
    }
    private void UpdateSkin(int selectedOption)
    {
        Character character = characterDB.GetCharacter(selectedOption);
        image.sprite = character.characterSprite;
      
    }
    // Update is called once per frame
    void Update()
    {

    }
}
