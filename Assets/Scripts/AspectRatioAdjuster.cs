using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AspectRatioAdjuster : MonoBehaviour
{
    /*   public int fullWidthUnits = 14;*/
      private CinemachineVirtualCamera virCam;
    public float scenewidth = 10;
    // Start is called before the first frame update
    void Start()
    {
      //  float ratio = (float)Screen.height / (float)Screen.width;
        virCam = GetComponent<CinemachineVirtualCamera>();
       // virCam.m_Lens.OrthographicSize = (float)fullWidthUnits * ratio / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float unitsPerPixel = scenewidth / Screen.width;
        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;
        virCam.m_Lens.OrthographicSize = desiredHalfHeight;
    }
}
