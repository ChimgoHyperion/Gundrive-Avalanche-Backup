using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthScript : MonoBehaviour
{
    // for the player's health
    public float Health;
    public Slider HealthBar;
    public bool isDead=false;
    public BaymaxButtonManager managerForNow; // used to check is player is being attacked and to reload scene
    public BaymaxMovement playerMovement;
    public bool isBlocking;
    // block integrity parameters
     public float shieldStrength;
     public float maxShieldStrength;
     public Button BlockButton;

    // for animations
    public Animator PlayerAnimator;
    public SpriteRenderer thisRenderer;
    public Color OriginalColor,hurtColor;
    public float waitTime;

    // Rage Bar
    public int RageAmount;
    public Slider RageBar;
    
    // Start is called before the first frame update
    void Start()
    {
        OriginalColor = thisRenderer.color;
        shieldStrength = maxShieldStrength;
        RageBar.value = RageAmount;
    }

    // Update is called once per frame
    void Update()
    {
        // Health checks can be done in TakeDamage method but constraints should be checked
        if (Health <= 0)
        {
            // play death animation
            isDead = true;
            checkDeath();
            // end game (you lose)
        }
        HealthBar.value = Health;
        RageBar.value = RageAmount;
    }
    void checkDeath()
    {
        if (isDead==true)
        {
            // activate death panel
            PlayerAnimator.SetTrigger("Death");
            managerForNow.ActivateDeathPanel();// win for the enemy
            isDead = false;
        }
    }
    public void TakeDamage(int Damage)
    {
        // if he's not blocking, take damage
        if (isBlocking == false)
        {
            // player cant attack
            managerForNow.canAttack = false;
            // Play hurt animation (stun the player)
            PlayerAnimator.SetTrigger("Hurt");
            // Knock Back Player
            playerMovement.KnockBack();
            // stop the player for a while then continue
            StartCoroutine(SpriteRedden());
            // damage player
            Health -= Damage;
            // increase ragebar value
            RageAmount += 15;
        }
        //if hes blocking, weaken shield/reduce shield strength
        if (isBlocking == true)
        {
            shieldStrength -= Damage;
        }
        // if shield strength is less or equal to zero, reset shield strength, and damage the player
        if (shieldStrength <= 0)
        {
            PlayerAnimator.SetBool("IsBlocking", false);// stopBlocking animation
            isBlocking = false;
            StartCoroutine(ShieldRestoration());
            PlayerAnimator.SetTrigger("Hurt");
          //  Health -= Damage; // damage player
        }
       
    }
    IEnumerator ShieldRestoration()
    {
        BlockButton.interactable = false;
        BlockButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(9f);
        
        BlockButton.gameObject.SetActive(true);
        BlockButton.interactable = true;
        shieldStrength = maxShieldStrength;
    }
    IEnumerator SpriteRedden()
    {
        thisRenderer.color = hurtColor;
        yield return new WaitForSeconds(waitTime);
        thisRenderer.color = OriginalColor;
        managerForNow.canAttack = true; // restore attack capabillity
        // this may be modified to include:1. the player may not be attacked when hes been knocked to the ground(instead of Hurt animation)
    }
}
