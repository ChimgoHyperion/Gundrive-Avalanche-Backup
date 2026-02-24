using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Fusion;

public class MovementandShooting : MonoBehaviour
{
    // for movement and rotation
    public float Speed;
    public float force;
    public FixedJoystick movementJoystick;
    Rigidbody2D rb;
    public  float LerpTime;
    public int health ;
    public Slider healthBar;
   // public Text healthText;
    public bool facingRight = true;


    // ground check
  [SerializeField]  private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatisGround;

    public float boostAmount;
    [SerializeField] float maxBoostValue;
    public float boostFactorIncrement,boostFactorDecreament;
    public Slider boostBar;
    public float gravityScale;
    public float fallGravity;

    public GameObject Shield;
    public Transform[] teleportPoints;
    


    EndGameManager EndgameManager;
    public GameObject weapon;
    WeaponScript weaponScript;


   
    public GameObject dustEffect;
    public GameObject damageEffect;
    public GameObject deatheffect;
    public Material WhiteMaterial;
    public Material spritesDefault;

    private Animator anim;
    public AudioClip hitRock;
    public AudioSource source;

    buttonSoundHolder soundHolder;
    public TrailRenderer trail;

    public float rigidBodySpeed;
    public Vector2 direction;

    [Header("Multiplayer Variables")]
    public Animator CharacterAnimator;

    public GameObject UIControls;
    public SpriteRenderer skinRenderer;

    public bool HasDied=false;
    public SlotScript SlotScript;

   
    public ControlType controlType;
    public enum ControlType
    {
      WASD,Joystick
    }
    // Start is called before the first frame update
    void Start()
    {
        healthBar.value = health;
        boostBar.maxValue = maxBoostValue;
        EndgameManager = FindObjectOfType<EndGameManager>();
        weaponScript = FindObjectOfType<WeaponScript>();
        rb = GetComponent<Rigidbody2D>();
        boostAmount = maxBoostValue;
        RandomPosition();
      
        soundHolder = FindObjectOfType<buttonSoundHolder>();
        anim = GetComponent<Animator>();
        // trail = GetComponent<TrailRenderer>(); for jetpack sake
        StartCoroutine(startTrail());


        // check the platform and change to the app
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            controlType = ControlType.WASD;
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            controlType = ControlType.Joystick;
        }
        else if (Application.isEditor)
        {
            controlType = ControlType.WASD;
        }

    }
    private void Update()
    {
        
        healthBar.value = health;
        boostBar.value = boostAmount;
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
  

   

    void FixedUpdate()
    {

        healthBar.value = health;



        // grounding

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatisGround);

        if (isGrounded)
        {
            if (boostAmount < maxBoostValue)
                boostAmount += Time.deltaTime * boostFactorIncrement;
            trail.emitting = false; // use RPC for multiplayer
        }
        if (boostAmount == 0)
        {
            boostAmount = Time.deltaTime;
        }
        if (isGrounded == false)
        {
            //  direction.y -= boostFactor * Time.deltaTime;
            if (boostAmount >= 0)
                boostAmount -= Time.deltaTime * boostFactorDecreament;
            trail.emitting = true;
        }

        if (boostAmount >= 0)
        {
            switch (controlType)
            {
                case ControlType.Joystick:
                    direction = new Vector2(movementJoystick.Horizontal, movementJoystick.Vertical);
                    break;
                case ControlType.WASD:
                    direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
                    break;
            }

            rb.gravityScale = gravityScale;

        }
        else
        {
            

            switch (controlType)
            {
                case ControlType.Joystick:
                    direction = new Vector2(movementJoystick.Horizontal, 0);
                    break;
                case ControlType.WASD:
                    direction = new Vector2(0, Input.GetAxisRaw("Vertical")).normalized;
                    break;
            }

            rb.gravityScale = fallGravity;
        }



        if (rb.bodyType != RigidbodyType2D.Static)
        {
            rb.MovePosition((Vector2)transform.position + direction * Speed * Time.deltaTime);

        }






        boostBar.value = boostAmount;

    }

    public void SwitchControls()
    {
        if(controlType == ControlType.Joystick)
        {
            controlType = ControlType.WASD;
        }else
        if (controlType == ControlType.WASD)
        {
            controlType=ControlType.Joystick;
        }
    }
    public void SlowLyDamage(int damageFactor)
    {
        health -= damageFactor;
    }


    public void TakeDamage (int damage)
    {
        StartCoroutine(shineWhite());
        if (HasDied)
        {
            return;
        }
        if (Shield.activeSelf== true)
        {
            health -= 0;
        }else if(!Shield.activeSelf )
        {
            health -= damage;
            Instantiate(damageEffect, transform.position, Quaternion.identity);
           int num =  Random.Range(0, 1);
            //switch (num)
            //{
            //    case 0:
            //        anim.SetTrigger("Squashing");
            //        break;
            //    case 1:

            //        anim.SetTrigger("Stretching");
            //        break;
            //}
           
          

        }
        else
        {
           // anim.SetBool("isStretched", false);
        }
        healthBar.value = health;
       
        if (health <= 0)
        {
            die();

        }

    }
    public void die()
    {
        // if is multiplayer, respawn
        SlotScript.DropItem();
        HasDied = true;
        soundHolder.PlayerDeath();
        Instantiate(deatheffect, transform.position, Quaternion.identity);
        skinRenderer.enabled = false;
        UIControls.SetActive(false);
       
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;// freeze player
        EndgameManager.EndGame();
        // drop gun
       
      //  gameObject.SetActive(false);
    }

    public void Resurrect()
    {
        RandomPosition();// change position
        HasDied = false;
        skinRenderer.enabled = true;
        health = 100;
        UIControls.SetActive(true);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
    IEnumerator shineWhite()
    {
      //  gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        gameObject.GetComponent<SpriteRenderer>().material = WhiteMaterial;
        yield return new WaitForSeconds(0.05f);
     //   gameObject.GetComponent<SpriteRenderer>().color = currentColor;
        gameObject.GetComponent<SpriteRenderer>().material = spritesDefault;

    }
    public void IncreaseHealth()
    {

        health = 100;
       
    }
    public void IncreaseBoost()
    {
        boostAmount = maxBoostValue;
    }
    public void ActivateShield()
    {
        Shield.SetActive(true);
        Shield.GetComponent<ShieldScript>().StartCountDown();
       
    }
    public void TeleportPlayer()
    {
        int pointNumber = Random.Range(0, teleportPoints.Length);
        transform.position = teleportPoints[pointNumber].position;
    }
    public GameObject BlackHoleBtn;
    public GameObject BlackHole;
    public Transform SpawnPoint;

    public void SpawnBhole()
    {
        Instantiate(BlackHole, SpawnPoint.position, Quaternion.identity);
        BlackHoleBtn.SetActive(false);
        SpawnPoint.gameObject.SetActive(false);
    }
   
   

   
    public void RandomPosition()
    {
        int pointNumber = Random.Range(0, teleportPoints.Length);
        transform.position = teleportPoints[pointNumber].position;
    }
    IEnumerator startTrail()
    {
        trail.enabled= false;
        yield return new WaitForSeconds(4f);
        trail.enabled = true;

       
    }

   
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.tag == "BlackHole")
        {
            collision.gameObject.SetActive(false);
            // view.RPC(nameof(ActivateLaser), RpcTarget.All);
            BlackHoleBtn.SetActive(true);
            SpawnPoint.gameObject.SetActive(true);
            Destroy(collision.gameObject);

        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BottomDeath")
        {
            die();
            Instantiate(dustEffect, transform.position, Quaternion.identity);
            source.PlayOneShot(hitRock);
        }
    }







}
