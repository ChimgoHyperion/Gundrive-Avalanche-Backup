using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public float parallaxEffectMultiplier;
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GameObject.Find("CM vcam1").transform;
        lastCameraPosition = cameraTransform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        
        transform.position += deltaMovement * parallaxEffectMultiplier;
        lastCameraPosition = cameraTransform.position;

    }
}
