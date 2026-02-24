using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Photon.Pun;
using Photon.Realtime;

public class AudioMana : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    private  AudioSource effectSource;
    public static AudioMana instance;

    public const string MusicKey = "MusicVolume";
    public const string SFXKey = "SFXVolume";


    private void Start()
    {
        effectSource = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else if( instance!= this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        LoadVolume();
    }
    public void PlaySound(AudioClip clip)
    {
        effectSource.clip = clip;
        effectSource.PlayOneShot(clip);
    }
    public void PlaySoundMulti(AudioClip clip)
    {
        this.GetComponent<PhotonView>().RPC("Play_Sound", RpcTarget.AllBuffered, clip);
    }


    [PunRPC]
    void Play_Sound(AudioClip clip)
    {
        effectSource.clip = clip;
        effectSource.PlayOneShot(clip);
    }
   




    public void StopSound()
    {
        effectSource.Stop();
    }
    void LoadVolume()// volume saved in volumeSettings.cs
    {
        float musicVolume = PlayerPrefs.GetFloat(MusicKey, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFXKey, 1f);
        mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        mixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
    }




}
