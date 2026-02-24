using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerAIScript : MonoBehaviour
{
    //Animations and States
    public Animator EnemyAnimator;
    public enum AIstate {Jump,Dash ,UseMagic} // these can be used when the enemy is still far from the player
    private AIstate currentState;
    [SerializeField]
    public bool IsWalking;//IsBlocking}
    public int Rand;
    
    // Timer
    public float stateChangeInterval;// Time in seconds to switch states. (Max value)
    public float stateChangeTimer;

    public float WalkmaxTime;
    public float Walktimer;

    public float doubleJumpMaxTime;
    public float doubleJumpTimer;
    // Movement variables
    Rigidbody2D rb;
    public float Dashforce,JumpForce,DoubleJumpForce;
    public float waitTimeForDoubleJump;
    public bool facingRight = false;
    public float KnockBackForce;

    // Seeking the Player
    public Transform Player;
    public float walkSpeed;
    public float waitTime;
    public float minimumDistance;
    

    // using magic variables
    public float AttackForce;
    public GameObject MagicAttack;
    public Transform AttackSpawnPoint;
    public bool canAttack;
    public EnemyHealthScript enemyHealth;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stateChangeTimer = stateChangeInterval;
        Walktimer = WalkmaxTime;
        doubleJumpTimer = doubleJumpMaxTime;
        SwitchState();// for animations that need animator.setbool
       SwitchBoolStates();// for animations that need animator.setbool
        
    }

    // Update is called once per frame
    void Update()
    {
        stateChangeTimer -= Time.deltaTime;
        if (stateChangeTimer <= 0)
        {
            SwitchState();
            
            stateChangeTimer = stateChangeInterval;
        }

        doubleJumpTimer -= Time.deltaTime;
        if (doubleJumpTimer <= 0)
        {
            JumpTwice(); // specifically for the double jump
            doubleJumpTimer = doubleJumpMaxTime;
        }
        Walktimer -= Time.deltaTime;
        if (Walktimer <= 0)
        {
            SwitchBoolStates();

            Walktimer = WalkmaxTime;
        }

        if (IsWalking)
        {
            if((Player.position.x<transform.position.x && facingRight) || (Player.position.x>transform.position.x && !facingRight))
            {
                Flip();
            }
            MoveTowardsPlayer();
        }
        if(Vector2.Distance(transform.position, Player.position) < minimumDistance)
        {
            // enter attack state
            /* if (canAttack)
             {
                 EnemyAnimator.SetTrigger("FirstAttack");
             }*/
            HandlePlayerProximity();
        }
        

    }
    //for triggers
    void SwitchState()
    {
        // randomize state
        currentState = (AIstate)Random.Range(0, System.Enum.GetValues(typeof(AIstate)).Length);
        // animator.playrandom

        EnemyAnimator.SetTrigger(currentState.ToString());
        // this has to take into account magic separately 
        HandleCurrentState();

       

    }
    // for triggers
    void HandleCurrentState()
    {
        switch (currentState)
        {
            case AIstate.Dash:
                if (!facingRight)
                {
                    rb.AddForce(Vector2.right * Dashforce, ForceMode2D.Impulse); // test with negative parameters
                }
                if (facingRight)
                {
                    rb.AddForce(Vector2.left * Dashforce, ForceMode2D.Impulse); // test with negative parameters
                }
                break;
            case AIstate.Jump:
                rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
                break;
            case AIstate.UseMagic:
                UseMagic();
                break;
        }
    }
    // for randomizing jump or doublejump
    void JumpTwice()
    {
        int rand = Random.Range(0, 2);
        switch (rand)
        {
            case 0:
                // do nothing
               
                break;
            case 1:
                StartCoroutine(DoubleJump());
                break;

        }
    }
    IEnumerator DoubleJump()
    {
        EnemyAnimator.SetTrigger("Jump");
        rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(waitTimeForDoubleJump);
        EnemyAnimator.SetTrigger("DoubleJump");
        rb.AddForce(Vector2.up * DoubleJumpForce, ForceMode2D.Impulse);

    }


    // for bools
    void SwitchBoolStates()
    {
        // randomize BoolState
        Rand = Random.Range(0, 2);
       // handle the output of Rand
        HandleCurrentBoolState();
    }
   
    // for animator.setbool animations
    void HandleCurrentBoolState()
    {
        switch (Rand)
        {
           // case BoolStates.IsBlocking:
                 // activate shield
                 // wait for a while then set bool back to false
             //   break;
            case 1:
                
                // walk, wait for a while then set bool back to false
                IsWalking = true;
                EnemyAnimator.SetBool("IsWalking", true);
                StartCoroutine(Walking());
                break;
           
        }
    }
    // for flipping
    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    // for moving towards the player
    void MoveTowardsPlayer()
    {
        Vector3 targetDirection = (Player.position - transform.position).normalized;
        transform.position += targetDirection * walkSpeed * Time.deltaTime;

       
    }

    IEnumerator Walking()
    {
        yield return new WaitForSeconds(waitTime);
        EnemyAnimator.SetBool("IsWalking", false);
        IsWalking = false;
    }
    // using Magic
    public void UseMagic()
    {
        if (enemyHealth.RageAmount >= 100)
        {
             StartCoroutine(WaitBeforeMagic());
        }
       
    }
    IEnumerator WaitBeforeMagic()
    {
        yield return new WaitForSeconds(1f);
        GameObject magic = Instantiate(MagicAttack, AttackSpawnPoint.position, Quaternion.identity);
        magic.GetComponent<Rigidbody2D>().velocity = transform.right * AttackForce;
    }
    // for damage KnockBack
    public void KnockBack()
    {
        if (facingRight)
        {
            // if facing right,knock back left
            rb.AddForce(Vector2.left * KnockBackForce, ForceMode2D.Impulse);
        }
        if (!facingRight)
        {
            // if facing left, knock back right
            rb.AddForce(Vector2.right * KnockBackForce, ForceMode2D.Impulse);
        }
    }
    // for approaching the player
    void HandlePlayerProximity()
    {
        int rand = Random.Range(0, 2);
        switch (rand)
        {
            case 0:
                // attack Player
                if (canAttack)
                {
                    EnemyAnimator.SetTrigger("FirstAttack");
                }
                break;
            case 1:
                // Block 
                StartCoroutine(waitandUnblock());
                break;
        }
    }
    IEnumerator waitandUnblock()
    {
        enemyHealth.isBlocking = true;
        EnemyAnimator.SetBool("IsBlocking", true);
        yield return new WaitForSeconds(2f);
        enemyHealth.isBlocking = false;
        EnemyAnimator.SetBool("IsBlocking", false);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
       
        Gizmos.DrawWireSphere(Player.position,minimumDistance);

    }
}
