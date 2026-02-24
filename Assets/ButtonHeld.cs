using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHeld : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{
    public Animator playeranimator;
    public PlayerHealthScript playerHealth;// reference to the player's health script
    public void OnPointerDown(PointerEventData eventData)
    {
        playeranimator.SetBool("IsBlocking", true);
        playerHealth.isBlocking = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        playeranimator.SetBool("IsBlocking", false);
        playerHealth.isBlocking = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
