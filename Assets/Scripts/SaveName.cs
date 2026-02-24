using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveName : MonoBehaviour
{
    public InputField nameField;
    public Text nameDisplay;

    // text mesh pro veersion
    public TMP_Text nameDisplayTmp;
    public TMP_InputField nameFieldTmp;

    // Start is called before the first frame update
    void Start()
    {
        nameDisplay.text = PlayerPrefs.GetString("PlayerNickname");
      //  nameDisplayTmp.text = PlayerPrefs.GetString("PlayerNickname");
    }
    public void SaveTextAsName()
    {
        if (nameField.text.Length <= 15)
        {
            PlayerPrefs.SetString("PlayerNickname", nameField.text);
        }
      /*  if (nameFieldTmp.text.Length <= 15)
        {
            PlayerPrefs.SetString("PlayerNickname", nameFieldTmp.text);
        }*/
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
