using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumablePickUp : MonoBehaviour
{

    private Inventory inventory;
    public Image ConsumableButton;
    buttonSoundHolder soundHolder;
    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("GunContainer").GetComponent<Inventory>();
        ConsumableButton.transform.localScale = new Vector3(1, 1, 1);
        soundHolder = FindObjectOfType<buttonSoundHolder>();

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="Player")
        {
          
            for (int i = 0; i < inventory.consumableSlots.Length; i++)
            {
                if (inventory.consumableFull[i] == false)
                {
                    inventory.consumableFull[i] = true;
                    soundHolder.collection();
                    Instantiate(ConsumableButton, inventory.consumableSlots[i].rectTransform,false);
                   // ConsumableButton.rectTransform.anchoredPosition = inventory.consumableSlots[i].rectTransform.anchoredPosition;
                   
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 10f);
    }
}
