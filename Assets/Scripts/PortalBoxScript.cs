using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBoxScript : MonoBehaviour
{
    buttonSoundHolder soundHolder;
  //  public MovementandShooting player;

    // Start is called before the first frame update
    void Start()
    {
     //   player = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementandShooting>();
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
            TelePortPlayer();
            collision.gameObject.GetComponent<MovementandShooting>().TeleportPlayer();
            soundHolder.collection();
            Destroy(gameObject);

        }
        if (collision.gameObject.tag == "World")
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        }
    }
    public void TelePortPlayer()
    {
       // player.TeleportPlayer();
        soundHolder.teleport();
     //   Destroy(gameObject);
    }
}
