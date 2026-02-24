using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

   public const string MIXER_MUSIC = "MusicVolume";
   public  const string MIXER_SFX = "SFXVolume";
    private void Awake()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

    }
    private void OnDisable()
    {
        PlayerPrefs.SetFloat(AudioMana.MusicKey, musicSlider.value);
        PlayerPrefs.SetFloat(AudioMana.SFXKey, sfxSlider.value);
    }
    void SetMusicVolume(float value)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value)*20);
    }
    void SetSFXVolume(float value)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
    }
    // Start is called before the first frame update
    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat(AudioMana.MusicKey, 1f);
        sfxSlider.value = PlayerPrefs.GetFloat(AudioMana.SFXKey, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
