using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthScript : MonoBehaviour
{
    // for the player's health
    public float Health;
    public Slider HealthBar;
    public bool isDead = false;
    public BaymaxButtonManager managerForNow;
    public ComputerAIScript enemyAIScript;

    // for animations
    public Animator PlayerAnimator; //in this case the enemy's animator
    public SpriteRenderer thisRenderer;
    public Color OriginalColor, hurtColor;
    public float waitTime;

    // Rage Bar
    public int RageAmount;
    public Slider RageBar;

    // block integrity parameters
    public float shieldStrength;
    public float maxShieldStrength;
    public Button BlockButton;
    public bool isBlocking;
    // Start is called before the first frame update
    void Start()
    {
        OriginalColor = thisRenderer.color;
        RageBar.value = RageAmount;
        shieldStrength = maxShieldStrength;
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
        if (isDead == true)
        {
            // activate death panel
            PlayerAnimator.SetTrigger("Death");
            managerForNow.ActivateWinPanel(); // win for the player
            isDead = false;
        }
    }
    public void TakeDamage(int Damage)
    {
        if (isBlocking == false)
        {
            // enemy cant attack
            enemyAIScript.canAttack = false;
            // Play hurt animation (stun the enemy)
            PlayerAnimator.SetTrigger("Hurt");
            // knock back the enemy
            enemyAIScript.KnockBack();
            // stop the enemy for a while then continue
            StartCoroutine(SpriteRedden());
            // damage the enemy
            Health -= Damage;
            // increase rage bar
            RageAmount += 15;
        }
       

        //if hes blocking, weaken shield/reduce shield strength
        if (isBlocking == true)
        {
            shieldStrength -= Damage;
        }
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
       
        yield return new WaitForSeconds(4f); // slightly less for the  enemy
       
        shieldStrength = maxShieldStrength;
    }
    IEnumerator SpriteRedden()
    {
        thisRenderer.color = hurtColor;
        yield return new WaitForSeconds(waitTime);
        thisRenderer.color = OriginalColor;
        enemyAIScript.canAttack = true;
    }
}
