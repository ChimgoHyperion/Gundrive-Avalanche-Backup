using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonAction : MonoBehaviour
{
    public MovementandShooting player;
    public GameObject Guncontainer;
    buttonSoundHolder soundHolder;
    
 
  
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementandShooting>();
        Guncontainer = GameObject.FindGameObjectWithTag("GunContainer");
        soundHolder = FindObjectOfType<buttonSoundHolder>();
    }
   
    public void increaseHealth()
    {
        player.IncreaseHealth();
        soundHolder.Health();
        Destroy(gameObject);
        
        
    }   
    public void increaseBoost()
    {
        player.IncreaseBoost();
        soundHolder.Boost();
        Destroy(gameObject);
       
    }
    public void ActivateShield()
    {
        player.ActivateShield();
        soundHolder.Shield();
        Destroy(gameObject);
    }
    public void TelePortPlayer()
    {
        player.TeleportPlayer();
        soundHolder.teleport();
        Destroy(gameObject);
    }
}
