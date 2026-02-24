using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BaymaxButtonManager : MonoBehaviour
{
    public Animator Playeranimator;
    public BaymaxMovement movement;
    public PlayerHealthScript playerHealth;
    // for block button
    public bool isButtonHeld = false;
    public int currentWeapon=1;

    public GameObject Sword,Axe;
    // to check if the player is being damaged
    public bool canAttack = true;
   
    public float comboTimeWindow = 0.5f;
    [SerializeField]
    private float lastAttackTime;
    [SerializeField]
    private int comboStep = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Attack()
    {
        // play first attack animation 
        // Playeranimator.SetTrigger("FirstAttack");
        //  ScreenShake.instance.shakeCamera(1f, 0.1f);

        // damage enemy
        if (canAttack)
        {
            switch (currentWeapon)
            {
                case 1:
                    Playeranimator.SetTrigger("FirstAttack");
                    break;
                case 2:
                    // for when wielding the axe (combos)
                    AttackWithAxe();
                    //  Playeranimator.SetTrigger("AxeFirst");
                    break;
            }
        }

       
    }
    public void HeavyAttack()
    {
        Playeranimator.SetTrigger("SwordHeavy");
    }
    public void SwitchWeapon()
    {
        if (currentWeapon == 1)
        {
            currentWeapon = 2;
            Sword.SetActive(false);
            Axe.SetActive(true);
        }
       else if (currentWeapon == 2)
        {
            currentWeapon = 1;
            Axe.SetActive(false);
            Sword.SetActive(true);
        }
    }
    void AttackWithAxe()
    {
        if (Time.time - lastAttackTime > comboTimeWindow)
        {
            // Reset combo if too much time has passed since the last attack
            comboStep = 0;
        }
        lastAttackTime = Time.time;
        switch (comboStep)
        {
            case 0:
                // trigger first axe attack animation
                Playeranimator.SetTrigger("AxeFirst");
                comboStep++;
                break;
            case 1:
                // trigger second axe attack animation
                Playeranimator.SetTrigger("AxeSecond");
                comboStep++;
                break;
        }

    }
    public void Block()
    {
        // play Blocking animation - handled in the block button
       
        // handled in the Block button itself

        // prevent Player from taking damage(activate shield)
    }
    public void UseMagic()
    {
        if (playerHealth.RageAmount >= 100)
        {
            // play Magic activation animation
            Playeranimator.SetTrigger("UseMagic");
            StartCoroutine(WaitBeforeMagic());
            
        }
       
    }
    IEnumerator WaitBeforeMagic()
    {
        yield return new WaitForSeconds(1f);
        // play magic effect animation or particle system for now
        movement.UseMagic();
        playerHealth.RageAmount = 0;
    }

    public void Dash()
    {
        Playeranimator.SetTrigger("Dash");
        movement.Dash();
    }
    // for block button held


    // for now till the game is transferred to a new project
    public GameObject Deathpanel,WinPanel;
    public GameObject Enemy;
    
    
    public void ActivateDeathPanel()
    {
        Deathpanel.SetActive(true);
        Enemy.GetComponent<ComputerAIScript>().enabled = false;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ActivateWinPanel()
    {
        WinPanel.SetActive(true);
        Enemy.GetComponent<ComputerAIScript>().enabled = false;
    }
}
