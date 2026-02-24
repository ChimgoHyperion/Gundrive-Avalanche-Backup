using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBoxScript : MonoBehaviour
{
    buttonSoundHolder soundHolder;
  //  public MovementandShooting player;

    // Start is called before the first frame update
    void Start()
    {
      //  player = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementandShooting>();
        soundHolder = FindObjectOfType<buttonSoundHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 10f);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ActivateShield();
            collision.gameObject.GetComponent<MovementandShooting>().ActivateShield();
         //   collision.gameObject.GetComponent<MultiplayerMoveAndShoot>().ActivateShield(); need new script for multiplayerVersion
            soundHolder.collection();
            Destroy(gameObject);
          
        }
        if (collision.gameObject.tag == "World")
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        }
    }
    public void ActivateShield()
    {
     //   player.ActivateShield();
        soundHolder.Shield();
     //   Destroy(gameObject);
    }
}
