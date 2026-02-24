using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialPlayer : MonoBehaviour
{
    // for movement and rotation
    public float Speed;
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
    private float maxBoostValue = 30;
    public float boostFactor;
    public Slider boostBar;

    public GameObject Shield;
    public Transform[] teleportPoints;



    EndGameManager EndGamemanager;
    public GameObject weapon;
    WeaponScript weaponScript;


    public Color currentColor;
    public GameObject dustEffect;
    public GameObject damageEffect;
    public GameObject deatheffect;

    private Animator anim;

    public void SlowLyDamage(int damageFactor)
    {
        health -= damageFactor;
    }


    public void TakeDamage(int damage)
    {
        if (Shield.activeSelf == true)
        {
            health -= 0;
        }
        else if (!Shield.activeSelf)
        {
            health -= damage;
            Instantiate(damageEffect, transform.position, Quaternion.identity);
            int num = Random.Range(0, 1);
            switch (num)
            {
                case 0:
                    anim.SetTrigger("Squashing");
                    break;
                case 1:

                    anim.SetTrigger("Stretching");
                    break;
            }

            StartCoroutine(shineWhite());

        }
        else
        {
            // anim.SetBool("isStretched", false);
        }
        if (health <= 0)
        {
            Instantiate(deatheffect, transform.position, Quaternion.identity);
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            EndGamemanager.EndGame();
            gameObject.SetActive(false);
            // sprite.enabled = false;


        }


    }
    IEnumerator shineWhite()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<SpriteRenderer>().color = currentColor;

    }
    public void IncreaseHealth()
    {
        if (health <= 80)
        {
            health += 20;
        }
        if (health > 80 && health <= 90)
        {
            health += 10;
        }
        if (health > 90)
        {
            health = 100;
        }

    }
    public void IncreaseBoost()
    {
        boostAmount = maxBoostValue;
    }
    public void ActivateShield()
    {
        Shield.SetActive(true);
    }
    public void TeleportPlayer()
    {
        int pointNumber = Random.Range(0, teleportPoints.Length);
        transform.position = teleportPoints[pointNumber].position;
    }



    // Start is called before the first frame update
    void Start()
    {
        healthBar.value = health;
        EndGamemanager = FindObjectOfType<EndGameManager>();
        weaponScript = FindObjectOfType<WeaponScript>();
        rb = GetComponent<Rigidbody2D>();
        boostAmount = maxBoostValue;
      //  int pointNumber = Random.Range(0, teleportPoints.Length);
        //transform.position = teleportPoints[pointNumber].position;
        currentColor = gameObject.GetComponent<SpriteRenderer>().color;

        anim = GetComponent<Animator>();
    }
    private void Update()
    {


        if (health > 0)
        {

        }
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
        // movement

        Vector2 direction = new Vector2(movementJoystick.Horizontal, movementJoystick.Vertical);
        direction.Normalize();
        rb.MovePosition((Vector2)transform.position + direction * Speed * Time.deltaTime);

        // grounding
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatisGround);

        if (isGrounded == false)
        {
            boostAmount -= Time.deltaTime * boostFactor;
            if (boostAmount <= 0)
            {
                Vector2 newdir = new Vector2(movementJoystick.Horizontal, 0);
                //  rb.MovePosition((Vector2)transform.position + newdir * 0 * Time.deltaTime);

                rb.gravityScale = 100;
                boostAmount = Time.deltaTime;

            }
        }
        else if (boostAmount < maxBoostValue)
        {
            boostAmount += Time.deltaTime * boostFactor;
            if (boostAmount >= 0)
            {
                rb.MovePosition((Vector2)transform.position + direction * Speed * Time.deltaTime);
                rb.gravityScale = 45f;
            }
        }
        boostBar.value = boostAmount;



    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "World")
        {
            Instantiate(dustEffect, transform.position, Quaternion.identity);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            Instantiate(dustEffect, transform.position, Quaternion.identity);
        }
        if (collision.gameObject.tag == "Enemy5")
        {
            Instantiate(dustEffect, transform.position, Quaternion.identity);
        }
    }







}
