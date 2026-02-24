using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialHealth : MonoBehaviour
{
    private Inventory inventory;
    public Image ConsumableButton;
    TutorialManager tutManager;
    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("GunContainer").GetComponent<Inventory>();
      //  ConsumableButton.transform.localScale = new Vector3(1, 1, 1);
        tutManager = FindObjectOfType<TutorialManager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            tutManager.hasCollectedHealth = true;
            for (int i = 0; i < inventory.consumableSlots.Length; i++)
            {
                if (inventory.consumableFull[i] == false)
                {
                    inventory.consumableFull[i] = true;
                    // Instantiate(ConsumableButton, inventory.consumableSlots[i].rectTransform, false);
                    // ConsumableButton.rectTransform.anchoredPosition = inventory.consumableSlots[i].rectTransform.anchoredPosition;
                    ConsumableButton.gameObject.SetActive(true);


                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
       // Destroy(gameObject, 10f);
    }
}
