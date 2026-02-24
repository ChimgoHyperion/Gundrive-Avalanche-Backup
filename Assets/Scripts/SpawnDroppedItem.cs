using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDroppedItem : MonoBehaviour
{
    public GameObject item;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void SpawnDroppedPowerUp()
    {
       Vector2 playerPos = new Vector2(player.position.x, player.position.y + 3);
        Instantiate(item, playerPos, Quaternion.identity);
    }
    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
       
    }
}
