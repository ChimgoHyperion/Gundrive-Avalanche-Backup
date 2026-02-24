using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
    private CinemachineVirtualCamera cineVirCamera;
    private float shakeTimer;
    public static ScreenShake instance { get; private set; }
   


    private void Awake()
    {
    }
    public void shakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinePerlin = cineVirCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinePerlin.m_AmplitudeGain = intensity;
       
        shakeTimer = time;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        cineVirCamera = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinePerlin = cineVirCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinePerlin.m_AmplitudeGain = 0f;
                
            }
        }
    }
}
