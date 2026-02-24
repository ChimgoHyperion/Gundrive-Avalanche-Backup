using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanelChange : MonoBehaviour
{
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    private int currentPanel;

    private void Awake()
    {
        Selectpanel(0);
    }
    private void Selectpanel(int index)
    {
        previousButton.interactable = (index != 0);
        nextButton.interactable = (index != transform.childCount - 1);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == index);
        }

    }
    public void ChangePanel(int Change)
    {
        currentPanel += Change;
        Selectpanel(currentPanel);
    }




}
