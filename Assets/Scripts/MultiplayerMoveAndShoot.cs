using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Fusion;



public class MultiplayerMoveAndShoot : NetworkBehaviour
{
    // for movement and rotation
    public float Speed;
    public float force;
    public FixedJoystick movementJoystick;
    Rigidbody2D rb;
    public float LerpTime;
    [Networked]
    public float Networkedhealth { get; set; } = 100;

    [Networked]
    public int NetworkedScore { get; set; } // used to rank

    [Networked]
    public string NetworkedNickName { get; set; }

    public Slider healthBar,healthBarAbvHead;
   
    public bool facingRight = true;


    // ground check
    [SerializeField] private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatisGround;

    public float boostAmount;
    [SerializeField] float maxBoostValue;
    public float boostFactor;
    public Slider boostBar;
    public float gravityScale;
    public float fallGravity;

    public GameObject Shield;
    public Transform[] teleportPoints;




    public Color currentColor;
    public GameObject dustEffect;
    public GameObject damageEffect;
    public GameObject deatheffect;
    public Material WhiteMaterial;
    public Material spritesDefault;

    private Animator anim;
    public AudioClip hitRock;
    public AudioSource source;

    public buttonSoundHolder soundHolder;
    public TrailRenderer trail;

    public float rigidBodySpeed;
    Vector2 direction;

    [Header("Multiplayer Variables")]
    public Animator CharacterAnimator;

    public enum PlayerTypes
    {
        SinglePlayer,
        MultiplayerPlayer
    }

    [Tooltip("Select the Player type for the character.")]
    public PlayerTypes playertype;

    public GameObject canvas, CameraMain, CinemachineGroup,GunandSkinContainer,UIControls;
    public GameObject lastAttacker;

    [SerializeField] bool isDead;
    public TextMeshProUGUI ScoreText;

    public bool HasDied = false;
    //  [SerializeField] MultiplayerSpawnPosition spawner;

    public ControlType controlType;
    public enum ControlType
    {
        WASD,Joystick
    }

    public CanvasGroup joyStickGroup;
    // Start is called before the first frame update
    void Start()
    {
        healthBar.value = Networkedhealth;
        boostBar.maxValue = maxBoostValue;
       
        rb = GetComponent<Rigidbody2D>();
        boostAmount = maxBoostValue;
      //  RandomPosition();
       
       
        anim = GetComponent<Animator>();
        // trail = GetComponent<TrailRenderer>(); for jetpack sake
        StartCoroutine(startTrail());

        //  spawner = FindObjectOfType<MultiplayerSpawnPosition>();


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
    public void GameStart()
    {

        StartCoroutine(InitialSpawn());
    }
    IEnumerator InitialSpawn()
    {
        yield return new WaitForSeconds(1f);

        if (HasStateAuthority)
        {
            canvas.SetActive(true);
            CameraMain.SetActive(true);
            CinemachineGroup.SetActive(true);

            //  canvas.SetActive(true);// not until game has started

        }
    }
    private void Update()
    {

        healthBar.value = Networkedhealth;
        healthBarAbvHead.value= Networkedhealth; 
        boostBar.value = boostAmount;
        ScoreText.text = "Score:" + NetworkedScore.ToString();
    }

    private void SetLocalStats()
    {
       
        if (PlayerPrefs.HasKey("PlayerNickname"))
        {
            string PlayerNickname = PlayerPrefs.GetString("PlayerNickname");
            NetworkedNickName = PlayerNickname; // This sets the Networked property, which syncs it to all clients


        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }




    void FixedUpdate()
    {
        if (playertype == PlayerTypes.MultiplayerPlayer)
        {
            
            
        }
        if (HasStateAuthority == false)
        {

            return;
        }
        if (HasStateAuthority)
        {

            CameraMain.SetActive(true);
            CinemachineGroup.SetActive(true);
            SetLocalStats();



        }
        healthBar.value = Networkedhealth;
       


        // grounding

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatisGround);

        if (isGrounded)
        {
            if (boostAmount < maxBoostValue)
                boostAmount += Time.deltaTime * boostFactor;
            trail.emitting = false; // use RPC for multiplayer

            SlowlyRecover();
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

       // rb.MovePosition((Vector2)transform.position + direction * Speed * Time.deltaTime);


        if (rb.bodyType != RigidbodyType2D.Static)
        {
            rb.MovePosition((Vector2)transform.position + direction * Speed * Time.deltaTime);

        }


        boostBar.value = boostAmount;

    }
    public void SwitchControls()
    {
        if (controlType == ControlType.Joystick)
        {
            controlType = ControlType.WASD;
        }
        else
        if (controlType == ControlType.WASD)
        {
            controlType = ControlType.Joystick;
        }
    }
    public void SlowLyDamage(int damageFactor)
    {
        Networkedhealth -= damageFactor;
    }

    public void RegisterAttacker(GameObject attacker)
    {
        lastAttacker = attacker;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void TakeDamage_RPC(int damage)
    {
        if (HasDied)
        {
            return;
        }
        if (Shield.activeSelf == true)
        {
            Networkedhealth -= 0;
        }
        else if (!Shield.activeSelf)
        {
            Networkedhealth -= damage;
            
        }
        else
        {
            // anim.SetBool("isStretched", false);
        }
        healthBar.value = Networkedhealth;
        
        if (Networkedhealth <= 0)
        {
            if (isDead)
            {
                return;
            }
            RPC_Effects();
            respawn();
        }

    }
   
    [Rpc]
    public void RPC_Effects()
    {
        // effects
        soundHolder.PlayerDeath();
        Instantiate(deatheffect, transform.position, Quaternion.identity);
        //  ScreenShake.instance.shakeCamera(1f, 0.1f);
    }

    public void IncreaseHealth()
    {
        if (Networkedhealth <= 80)
        {
            Networkedhealth += 20;
        }
        if (Networkedhealth > 80 && Networkedhealth <= 90)
        {
            Networkedhealth += 10;
        }
        if (Networkedhealth > 90)
        {
            Networkedhealth = 100;
        }

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




  
    IEnumerator startTrail()
    {
        trail.enabled = false;
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

        if (collision.gameObject.tag == "Health")
        {
            Networkedhealth = 100;
            Destroy(collision.gameObject);
            soundHolder.Health();
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BottomDeath")
        {
           
            respawn();
            RPC_Effects();
            Instantiate(dustEffect, transform.position, Quaternion.identity);
            source.PlayOneShot(hitRock);
        }
    }


    // for respawning player after knockout
    void respawn()
    {
        
        if (lastAttacker != null && lastAttacker!= this.gameObject)
        {
            
            lastAttacker.GetComponent<MultiplayerMoveAndShoot>().NetworkedScore += 1;
        }
       
        StartCoroutine(waitBeforeRespawn());
    }
    IEnumerator waitBeforeRespawn()
    {

        HasDied = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;// freeze player


        isDead = true;
        RPC_Activations(false,0);
       
        if (HasStateAuthority)
        {
            ScoreBoardActivation.Instance.ShowScoreboard();
        }
       
      
        yield return new WaitForSeconds(3f);

       
        
        if (HasStateAuthority)
        {
           
            Networkedhealth = 100;
            boostAmount = maxBoostValue;
        }
       
        this.transform.position = new Vector2(0, 0);

        yield return new WaitForSeconds(1f);
        isDead = false;

        HasDied = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;// freeze player
        RPC_Activations(true,1);
        if (HasStateAuthority)
        {
            ScoreBoardActivation.Instance.HideScoreboard();
        }
        lastAttacker = null;// to avoid the player coming back and rewarding the last attacker again

    }

    [Rpc]
    public void RPC_Activations(bool ActiveState,int alphaState)
    {


        GunandSkinContainer.SetActive(ActiveState); // make the player invisible
      //  UIControls.SetActive(ActiveState);// cant control player

        joyStickGroup.interactable=(ActiveState);
        joyStickGroup.blocksRaycasts=ActiveState;
        joyStickGroup.alpha = alphaState;
    }
    public bool isDisplayingScoreboard = false;
    public void DisplayScoreboard()// connected to the button in the canvas
    {
        if (!isDisplayingScoreboard)
        {
            ScoreBoardActivation.Instance.ShowScoreboard();
            isDisplayingScoreboard=true;
        }else
        if (isDisplayingScoreboard)
        {
           ScoreBoardActivation.Instance.HideScoreboard();
            isDisplayingScoreboard = false;
        }
    }
    public void SlowlyRecover()
    {
        // when health is not depreciating slowly increase health

        if (Networkedhealth < 100)
        {
            Networkedhealth += Time.deltaTime * 0.5f;
        }
       
    }
    public void GameEnds()
    {
        ScoreBoardActivation.Instance.ShowScoreboard();// show scoreboard
        joyStickGroup.interactable = false;// disable controls
        joyStickGroup.blocksRaycasts = false;
        joyStickGroup.alpha = 0;
        UIControls.SetActive(false);
    }
}

