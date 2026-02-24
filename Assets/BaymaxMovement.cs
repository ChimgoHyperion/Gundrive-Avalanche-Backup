using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaymaxMovement : MonoBehaviour
{
    // for movement and rotation
    public float MoveSpeed;
    public FixedJoystick movementJoystick;
    Rigidbody2D rb;
    Vector2 direction;
    public float LerpTime;
    public float DashForce;
    public float KnockBackForce;
    Vector2 dashDirection;

    //  public int jumpsLeft;
    //  public int maxJumps;
    public bool candoubleJump;
    public float jumpforce;
    public float doublejumpforce;
  
   public float acceleration, decceleration,velPower;

    public int lastGroundedTime,lastJumpTime;

    public bool isJumping, jumpInputReleased;

    // wall sliding
    [Header("Wall Sliding system")]
    public Transform wallCheck;
    public bool iswalltouch, isSliding;
    public float wallSlidingSpeed;
    public float HorizontalInput;
    public float BoxX, BoxY;

    public float wallJumpDuration;
    public Vector2 wallJumpForce;
    public bool wallJumping;
   

    // ground check
    [SerializeField] private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatisGround;

    // Animation
    public Animator PlayerAnimator;

    // Magic attack
    public GameObject magicAttack;
    public Transform AttackSpawnPoint;
    public float force;

    // Player flipping
    public GameObject Object;
    Vector2 Rotation;
    private float rotation2;
    public bool facingRight = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
       
        // in the start of the game, jumpsleft equals maxjump
       // jumpsLeft = maxJumps;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        // Checking if Player is on the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatisGround);

        if (isGrounded)
        {
            // jumpsLeft = maxJumps;
            candoubleJump = true;
           
        }

        if (isGrounded == false)
        {

        }
        // Translational movement
        direction.x = movementJoystick.Horizontal;

        

        if (movementJoystick.Horizontal != 0 && isGrounded)
        {
            // play Walking animation
            PlayerAnimator.SetBool("IsWalking", true);
        }
        if (movementJoystick.Horizontal == 0 && movementJoystick.Vertical == 0)
        {
            // Switch back to idle animation
            PlayerAnimator.SetBool("IsWalking", false);
        }

        // Player direction flipping
        UpdateGame();
        

        #region Run

        float targetSpeed = movementJoystick.Horizontal * MoveSpeed;
        float speedDif = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        rb.AddForce(movement * Vector2.right);
        #endregion

        // checking if player is on the wall
        iswalltouch = Physics2D.OverlapBox(wallCheck.position, new Vector2(BoxX, BoxY), 0, whatisGround);
        HorizontalInput = movementJoystick.Horizontal;
         // for wall sliding
        if (iswalltouch && !isGrounded && HorizontalInput != 0)
        {
            isSliding = true;
        }
        else
        {
            isSliding = false;
        }
        if (isSliding)
        {
            // play Wall sliding animation
            PlayerAnimator.SetTrigger("Slide");
            // Apply force so player can jump away and back to the wall
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        #region old jump code
        // for wall jumping
        /*if (wallJumping)
         {
             rb.velocity = new Vector2(-HorizontalInput * wallJumpForce.x, wallJumpForce.y);
         }
         else
         {
             rb.AddForce(movement * Vector2.right);
         }*/
        // for jumping
        //  if //(jumpsLeft <= 0)
        //  {
        //  rb.gravityScale = 20;
        // }
        // else
        //  {
        //      rb.gravityScale = 2f;
        //}
        #endregion
    }
    // for jumping
    public void JumpWhenButtonPressed()
    {
        #region old double jump code
        // if ( jumpsLeft>0) //&& rb.gravityScale==2f)
        // {
        // rb.velocity = new Vector2(rb.velocity.x, 0f);
        // StartCoroutine(Jump());
        //   }

        // attempted wall jumping when player is wall sliding

        /* if (isSliding)
          {
              wallJumping = true;
              Invoke("StopWallJump", wallJumpDuration);
          }*/
        #endregion
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            PlayerAnimator.SetTrigger("Jump");
           
            rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
        } else if (candoubleJump)
        {
           
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            PlayerAnimator.SetTrigger("DoubleJump");
            rb.AddForce(Vector2.up * doublejumpforce, ForceMode2D.Impulse);
            candoubleJump = false;
            
        }
        
    }
     
    IEnumerator Jump()
    {
        #region old jump code
        //Play Jumping animation
        PlayerAnimator.SetTrigger("Jump");
        yield return new WaitForSeconds(0.01f);
        //apply upward force
        // rb.AddForce(Vector2.up * Jumpforce, ForceMode2D.Impulse);
        // jumpsLeft -= 1;


        // smoothing jumps
        /* lastGroundedTime = 0;
         lastJumpTime = 0;
         isJumping = true;
         jumpInputReleased = false;*/

        // ScreenShake.instance.shakeCamera(1f, 0.1f);




        //revert to idle animation
        //  yield return new WaitForSeconds(1f);
        //  rb.gravityScale = 20;

        //  yield return new WaitForSeconds(1f);
        //  rb.gravityScale = 2f;
        #endregion
    }

    void StopWallJump()
    {
        wallJumping = false;
    }
    // for player flipping
    void UpdateGame()
    {
        Rotation = new Vector2(movementJoystick.Horizontal, movementJoystick.Vertical);
        rotation2 = Rotation.x;

       
        if (rotation2 < 0 && facingRight)
        {
            Flip();
           
        }
        else if (rotation2 > 0 && !facingRight)
        {
            Flip();
           
        }
    }


    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0); // check
                                     /* Vector3 Scaler = transform.localScale;
                                      Scaler.x *= -1;*/
                                     // transform.localScale *= -1;
    }

    // For Dashing

    public void Dash()
    {
        if (facingRight)
        {
            rb.AddForce(Vector2.right * DashForce, ForceMode2D.Impulse);
        }
        if (!facingRight)
        {
            rb.AddForce(Vector2.left * DashForce, ForceMode2D.Impulse);
        }

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
    // for Magic Blast
    public void UseMagic()
    {
       GameObject magic= Instantiate(magicAttack, AttackSpawnPoint.position, Quaternion.identity);
        magic.GetComponent<Rigidbody2D>().velocity = transform.right * force;
    }

    // for Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(wallCheck.position, new Vector3(BoxX,BoxY, 0f));
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);

    }
}
