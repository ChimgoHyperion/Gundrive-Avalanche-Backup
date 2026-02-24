using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyPowerUP : MonoBehaviour
{
    public Button powerupButton;
    WeaponHolder weaponHolder;
    private Inventory inventory;

    public void destroyPowerUp()
    {
        powerupButton.GetComponent<Image>().enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        weaponHolder = GameObject.FindObjectOfType<WeaponHolder>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }
    //public void RemoveGun1()
    //{
    //    inventory.slots[0].GetComponent<Image>().enabled = false;
    //    inventory.isFull[0] = false;
    //    Destroy(weaponHolder.guns[0]);
    //}
    //public void RemoveGun2()
    //{
    //    inventory.slots[1].GetComponent<Image>().enabled = false;
    //    inventory.isFull[1] = false;
    //    Destroy(weaponHolder.guns[1]);
    //}
    //public void RemoveGun3()
    //{
    //    inventory.slots[2].GetComponent<Image>().enabled = false;
    //    inventory.isFull[2] = false;
    //    Destroy(weaponHolder.guns[2]);
    //}
    //public void RemoveGun4()
    //{
    //    inventory.slots[3].GetComponent<Image>().enabled = false;
    //    inventory.isFull[3] = false;
    //    Destroy(weaponHolder.guns[3]);
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
