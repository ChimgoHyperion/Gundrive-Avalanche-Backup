using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class WeaponHolderMulti : NetworkBehaviour
{
   
    int totalWeapons = 0;
   
   

    private bool buttonClicked = false;
    DestroyPowerUP destroyPowerUp;
   
    // gun control 
    public Joystick WeaponStick;
    

    Inventory inventory;
   
    [SerializeField]  buttonSoundHolder soundHolder;
    [SerializeField] Joystick WeaponJoystick;
    [SerializeField] Slider ammoBar;


    public GameObject[] Guns;
    public int currentGunIndex;

    [Networked]
    public int NetworkedcurrentWeaponSelection { get; set; }
    public bool canPickBullets;
    public LayerMask weaponLayer;
    public float radius;
    // Start is called before the first frame update
    private void Awake()
    {
       
    }
    void Start()
    {
      
        destroyPowerUp = FindObjectOfType<DestroyPowerUP>();

        inventory = GetComponent<Inventory>();
        soundHolder = GameObject.FindObjectOfType<buttonSoundHolder>();

        
    }


   

    // Update is called once per frame
    void Update()
    {
        if (Object.HasStateAuthority)
        {
            WeaponSelection();

        }
        Guns[NetworkedcurrentWeaponSelection].SetActive(true);


        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, weaponLayer);

        if (colliders.Length > 0)
        {
            canPickBullets = true;
            foreach (GameObject obj in Guns)
            {
                // Skip execution if an element in the array is null
                // if (obj == null) continue;


                if (obj.TryGetComponent<RiffleGunShootingScriptMulti>(out RiffleGunShootingScriptMulti riffleScript))
                {
                    // Access variables or call public methods
                    riffleScript.bulletsLeft = riffleScript.MaxBullets;
                }

                if (obj.TryGetComponent<MiniBombGunMulti>(out MiniBombGunMulti protonbombscript))
                {
                    // Access variables or call public methods
                    protonbombscript.bulletsLeft = protonbombscript.MaxBullets;
                }

            }
            foreach(Collider2D col in colliders)
            {
                Destroy(col.gameObject);
                soundHolder.Health();
            }
            
           
        }
        else
        {
            // No weapon in range, reset the reference
            canPickBullets = false;
        }
    }
    
    void WeaponSelection()
    {

        if (PlayerPrefs.HasKey("SelectedGun"))
        {
            int savedGunSelection = PlayerPrefs.GetInt("SelectedGun");
            NetworkedcurrentWeaponSelection = savedGunSelection;


        }
        else
        {
            NetworkedcurrentWeaponSelection = 0;

        }
    }

   
    [Rpc]
    public void RPC_DropGun()// called from the btn
    {
        foreach (GameObject gun in Guns)
        {
            gun.SetActive(false);
        }
        inventory.isFilled = false;
      
        ammoBar.value = 0;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, radius);
        Gizmos.color = Color.blue;
    }


}
