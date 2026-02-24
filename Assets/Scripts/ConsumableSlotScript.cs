using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableSlotScript : MonoBehaviour
{
    private Inventory inventory;
    public int i;
    public RectTransform rect;
    

    public void DropItem()
    {
        foreach (Transform child in transform)
        {
            // child.GetComponent<SpawnDroppedItem>().SpawnDroppedPowerUp();
            Destroy(child.gameObject);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("GunContainer").GetComponent<Inventory>();
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<RectTransform>().position = rect.position;
        }
        if (transform.childCount <= 0)
        {
            inventory.consumableFull[i] = false;
        }


    }
}
