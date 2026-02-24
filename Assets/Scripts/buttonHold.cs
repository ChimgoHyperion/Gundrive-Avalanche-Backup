using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class buttonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public zoomScript zoomScript;

    public void OnPointerDown(PointerEventData eventData)
    {
        zoomScript.zoomIn = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        zoomScript.zoomIn = false;
    }

    public void shootBullet()
    {

    }
}
