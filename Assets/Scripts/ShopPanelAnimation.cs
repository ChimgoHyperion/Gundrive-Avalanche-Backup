using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanelAnimation : MonoBehaviour
{
    [SerializeField] Vector3 finalPosition;
    [SerializeField] Vector3 initialPosition;
    RectTransform initialRect;
    [SerializeField] RectTransform finalRect;
   

   


    private void Awake()
    {
       // initialPosition = transform.position;
       initialRect= this.GetComponent<RectTransform>();

        
    }

   



    // Update is called once per frame
    void Update()
    {
        initialRect.position = Vector3.Lerp(initialRect.position, finalRect.position, 0.1f);
      //  transform.position = Vector3.Lerp(transform.position, finalPosition, 0.1f);
    }

    private void OnDisable()
    {
        //transform.position = initialPosition;
        this.GetComponent<RectTransform>().position = initialRect.position;
      
    }
}
