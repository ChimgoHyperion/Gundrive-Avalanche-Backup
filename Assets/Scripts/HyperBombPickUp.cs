using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperBombPickUp : MonoBehaviour
{
    private Inventory inventory;
   
    buttonSoundHolder soundHolder;
    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("GunContainer").GetComponent<Inventory>();
      
        soundHolder = FindObjectOfType<buttonSoundHolder>();
       

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
           
            if (inventory.hyperBombsCollected< 4)
            {
                inventory.hyperBombsCollected++;
                soundHolder.collection();
                Destroy(gameObject);
            }
           
        }
    }
    // Update is called once per frame
    void Update()
    {
          Destroy(gameObject, 10f);
    }
}
