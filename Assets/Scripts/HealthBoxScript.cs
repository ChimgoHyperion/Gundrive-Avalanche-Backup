using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoxScript : MonoBehaviour
{
    buttonSoundHolder soundHolder;
    public MovementandShooting player;
   
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementandShooting>();
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
            increaseHealth();
            soundHolder.collection();
            Destroy(gameObject);
            /*  float time = 3f;
              time -= Time.deltaTime;
              if (time <= 0)
              {
                  Destroy(gameObject);
              }*/
        }
        if (collision.gameObject.tag == "World")
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        }
    }
    public void increaseHealth()
    {
        player.IncreaseHealth();
        soundHolder.Health();
        Destroy(gameObject);


    }
}
