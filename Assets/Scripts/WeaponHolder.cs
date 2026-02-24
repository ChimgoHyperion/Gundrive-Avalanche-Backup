using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHolder : MonoBehaviour
{
    int totalWeapons = 0;
  //  public GameObject[] guns;
     GameObject weaponHolder;
   
    private bool buttonClicked = false;
    DestroyPowerUP destroyPowerUp;
   
    // gun control 
    public Joystick WeaponStick;
    bool gun1Selected;
    bool gun2Selected;

    Inventory inventory;
   
    buttonSoundHolder soundHolder;

    public GameObject currentGun;
    // Start is called before the first frame update
    void Start()
    {
        weaponHolder = this.gameObject;
        destroyPowerUp = FindObjectOfType<DestroyPowerUP>();

        inventory = GetComponent<Inventory>();
        soundHolder = GameObject.FindObjectOfType<buttonSoundHolder>();
    }
  
 
   

    // Update is called once per frame
    void Update()
    {
        totalWeapons = weaponHolder.transform.childCount;
       
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        if (collision.gameObject.TryGetComponent(out PickUp pickUpScript))
        {
           
            if (inventory.isFilled == false)
            {
                currentGun = pickUpScript.Weapon;
                Destroy(collision.gameObject);
            }
           
        }
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (inventory.isFilled == false)
            {

                inventory.isFilled = true;
                soundHolder.collection();
               
                Instantiate(currentGun, this.gameObject.transform, false);
                Destroy(currentGun.gameObject);
               

                //inventory.slots[i].GetComponent<Image>().enabled = true;
                
                break;
            }
        }
       


        if (collision.gameObject.tag == "World")
        {
          //  GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
      
    }


}
