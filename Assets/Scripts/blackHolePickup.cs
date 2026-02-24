using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class blackHolePickup : MonoBehaviour
{
   
  
    public float deathTime;
    public Button button;
   
  



    void Start()
    {
       
       
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "World")
        {
            MakeStatic();
        }

        if (collision.gameObject.tag == "Player")
        {
            MakeStatic();
        }

    }
   
    void MakeStatic()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static; // must this Be RPCed???
    }



    // Update is called once per frame
    void Update()
    {
        StartCoroutine(WaitDestroy());
       
    }
    IEnumerator WaitDestroy()
    {
        yield return new WaitForSeconds(deathTime);
       
    }
   
    void Destroy()
    {
        Destroy(gameObject);
    }




    /*  private Inventory inventory;
      //  public Image BlackHoleButton;
      //  public Image BombContainer;
      buttonSoundHolder soundHolder;
      // Start is called before the first frame update
      void Start()
      {
          inventory = GameObject.FindGameObjectWithTag("GunContainer").GetComponent<Inventory>();
          //   ConsumableButton.transform.localScale = new Vector3(1, 1, 1);
          soundHolder = FindObjectOfType<buttonSoundHolder>();
          //  BlackHoleButton = GameObject.Find("BlackHoleButton").GetComponent<Image>();

      }
      private void OnCollisionEnter2D(Collision2D collision)
      {
          if (collision.gameObject.tag == "Player")
          {
              /*  if(BlackHoleButton.gameObject.activeSelf== false)
                {
                    BlackHoleButton.gameObject.SetActive(true);
                }*/
    /*  if (inventory.bombsCollected < 4)
      {
          inventory.bombsCollected++;
          soundHolder.collection();
          Destroy(gameObject);
      }*/
    /*  for (int i = 0; i < inventory.bombsCollected; i++)
      {

          if (inventory.bombSlotfull[i] == false)
          {
              inventory.bombSlotfull[i] = true;
              soundHolder.collection();

              // ConsumableButton.rectTransform.anchoredPosition = inventory.consumableSlots[i].rectTransform.anchoredPosition;

              Destroy(gameObject);
              break;
          }
      } **
}
}
// Update is called once per frame
void Update()
{
 Destroy(gameObject, 10f);
} */
}
