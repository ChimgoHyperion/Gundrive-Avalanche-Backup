using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickSizeChange : MonoBehaviour
{
    float currentValue;
    RectTransform rTransform;
    // Start is called before the first frame update
    void Start()
    {
        currentValue = PlayerPrefs.GetFloat("SliderValue");
        rTransform = GetComponent<RectTransform>();
        rTransform.localScale = new Vector3(currentValue, currentValue, currentValue);

        
        if (currentValue == 0 || currentValue <=1)
        {
            rTransform.localScale = new Vector3(1, 1, 1);
        }
        LoadPosition();
    }
    // Update is called once per frame
    void Update()
    {
        rTransform.localScale = new Vector3(currentValue, currentValue, currentValue);
        if (currentValue == 0 || currentValue <= 1)
        {
            rTransform.localScale = new Vector3(1, 1, 1);
        }
    }
    private void LoadPosition()
    {


        if (this.gameObject.name == "movement Joysitck")
        {
            if (PlayerPrefs.HasKey("UIPositionXM") && PlayerPrefs.HasKey("UIPositionYM"))
            {
                float posX = PlayerPrefs.GetFloat("UIPositionXM");
                float posY = PlayerPrefs.GetFloat("UIPositionYM");
                rTransform.anchoredPosition = new Vector2(posX, posY);
            }

        }
        else if (this.gameObject.name == "Weapon Joystick")
        {
            if (PlayerPrefs.HasKey("UIPositionXW") && PlayerPrefs.HasKey("UIPositionYW"))
            {
                float posX = PlayerPrefs.GetFloat("UIPositionXW");
                float posY = PlayerPrefs.GetFloat("UIPositionYW");
                rTransform.anchoredPosition = new Vector2(posX, posY);
            }

        }
       
    }
}
