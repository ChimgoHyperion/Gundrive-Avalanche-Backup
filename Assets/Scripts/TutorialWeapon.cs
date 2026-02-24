using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialWeapon : MonoBehaviour
{
    private Inventory inventory;
    public GameObject Weapon;
    private Transform Guncontainer;
    public Sprite newImage;
    public float deathTime;
    public Button button;
    public TutorialManager tutManager;




    void Start()
    {

        Guncontainer = GameObject.FindGameObjectWithTag("GunContainer").transform;
        inventory = Guncontainer.GetComponent<Inventory>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            tutManager.hasCollectedWeapon = true;
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                if (inventory.isFilled == false)
                {

                    inventory.isFilled = true;

                    Instantiate(Weapon, Guncontainer.transform, false);

                    // Instantiate(button, inventory.slots[i].rectTransform, false);

                  //  Weapon.transform.localPosition = new Vector3(1.5f, -0.15f, 0f);


                    inventory.slots[i].GetComponent<Image>().sprite = newImage;

                    inventory.slots[i].GetComponent<Image>().enabled = true;



                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
      //  Destroy(gameObject, deathTime);
    }
}
