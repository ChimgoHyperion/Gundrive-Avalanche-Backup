using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JoystickSizeMana : MonoBehaviour
{
    public Slider joystickSlider;
   public float slidervalue;
   public float newValue;
    // Start is called before the first frame update
  /*  private static JoystickSizeMana instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }*/
    void Start()
    {
        newValue = PlayerPrefs.GetFloat("SliderValue");

       // PlayerPrefs.SetFloat("SliderValue", 1.5f);
       slidervalue =  PlayerPrefs.GetFloat("SliderValue");

       joystickSlider.value = newValue;


    }
    public void saveValue()
    {
        float newJoystickvalue = joystickSlider.value;

        PlayerPrefs.SetFloat("SliderValue", newJoystickvalue);

        
        newValue = PlayerPrefs.GetFloat("SliderValue");
    }
    // Update is called once per frame
    void Update()
    {
       //  newValue = joystickSlider.value;

       // joystickSlider.value = newValue;
    }
}
