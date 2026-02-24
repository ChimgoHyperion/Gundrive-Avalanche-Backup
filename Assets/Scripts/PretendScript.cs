using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PretendScript : MonoBehaviour
{
    // for movement and rotation
    public float Speed;
    public float force;
    public FixedJoystick movementJoystick;
    Rigidbody2D rb;
    public float LerpTime;
    public int health;
    public Slider healthBar;
    // public Text healthText;
    public bool facingRight = true;


    // ground check
    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatisGround;

    public float boostAmount;
    [SerializeField] float maxBoostValue;
    public float boostFactor;
    public Slider boostBar;
    public float gravityScale;
    public float fallGravity;
    

   
   
    Vector2 direction;

   
   

    private void Awake()
    {
        /*if (view.IsMine)
         {
             SpawnPlayers.instance.localPlayer = this.gameObject;
         }*/
    }
    void Start()
    {
        healthBar.value = health;
        boostBar.maxValue = maxBoostValue;
        //  difficultyManager = FindObjectOfType<DifficultyManager>();
       
        rb = GetComponent<Rigidbody2D>();
        boostAmount = maxBoostValue;
        // int pointNumber = Random.Range(0, teleportPoints.Length);
        //  transform.position = teleportPoints[pointNumber].position;
       
       
    }

    public void IncreaseBoost()
    {
        boostAmount = maxBoostValue;
    }
   

    
    private void Update()
    {
        // if (view.IsMine) { }
        healthBar.value = health;
    }
    IEnumerator enableEndGame()
    {
        yield return new WaitForSeconds(2);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
    // Update is called once per frame


    void FixedUpdate()
    {
        CheckInput();
       
    }


    private void CheckInput()
    {
        rb.MovePosition((Vector2)transform.position + direction * Speed * Time.deltaTime);
        //  view.transform.position = new Vector2(movementJoystick.Horizontal, movementJoystick.Vertical);
        //  view.gameObject.GetComponent<Rigidbody2D>().MovePosition((Vector2)transform.position + direction * Speed * Time.deltaTime);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatisGround);

        if (isGrounded)
        {
            if (boostAmount < maxBoostValue)
                boostAmount += Time.deltaTime * boostFactor;
           
        }
        if (boostAmount == 0)
        {
            boostAmount = Time.deltaTime;
        }
        if (isGrounded == false)
        {
            //  direction.y -= boostFactor * Time.deltaTime;
            if (boostAmount >= 0)
                boostAmount -= Time.deltaTime * boostFactor;
           
        }

        if (boostAmount >= 0)
        {
            direction = new Vector2(movementJoystick.Horizontal, movementJoystick.Vertical);
            rb.gravityScale = gravityScale;

        }
        else
        {
            direction = new Vector2(movementJoystick.Horizontal, 0);
            rb.gravityScale = fallGravity;
        }

        boostBar.value = boostAmount;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        /* if(collision.gameObject.tag== "World")
          {
              Instantiate(dustEffect, transform.position, Quaternion.identity);
              source.PlayOneShot(hitRock);
          }
          if (collision.gameObject.tag == "Enemy")
          {
              Instantiate(dustEffect, transform.position, Quaternion.identity);
              source.PlayOneShot(hitRock);
          }
          if (collision.gameObject.tag == "Enemy5")
          {
              Instantiate(dustEffect, transform.position, Quaternion.identity);
              source.PlayOneShot(hitRock);
          }*/

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Boost")
        {
            IncreaseBoost();
           // view.RPC(nameof(IncreaseBoost), RpcTarget.AllBuffered);

        }
    }


}
