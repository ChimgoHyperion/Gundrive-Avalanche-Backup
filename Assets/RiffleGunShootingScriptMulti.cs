using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;


public class RiffleGunShootingScriptMulti : NetworkBehaviour
{
    public Transform shootingTip;
    // for shooting
    public GameObject bulletPrefab;

    private float timebtwShots;
    public float starttimebtwShots;
    public float Bulletspeed;
    public Slider ammoBar;
    public int MaxBullets;
    public int bulletsLeft;

    public float offset;

    public GameObject muzzleflash;
    public float intensity;
    public float time;



    // weapon movement
    public Animator Anim;
    // audio
    public AudioClip shootingSound;
    public Joystick joystick;
    public Inventory inventory;
    public WeaponHolderMulti weaponHolder;

    [Header("Hitscan properties")]
    [SerializeField] private float range = 20f;
    [SerializeField] private int damage = 25;
    [SerializeField] private LayerMask hitMask;

    public GameObject Attacker;// set to player wielding this gun

    public MultiplayerMoveAndShoot movementandShooting;
    // Start is called before the first frame update
    void Start()
    {
        shootingTip.rotation = transform.rotation;
        

        

    }
    // Update is called once per frame
    void Update()
    {
        
        
        if (!HasStateAuthority)
        {
            return;
        }
        if (ammoBar != null)
        {
            ammoBar.maxValue = MaxBullets;
        }

        movementandShooting = GetComponentInParent<MultiplayerMoveAndShoot>();
        switch (movementandShooting.controlType)
        {
            case MultiplayerMoveAndShoot.ControlType.Joystick:
                if (Mathf.Abs(joystick.Horizontal) > 0.5 || Mathf.Abs(joystick.Vertical) > 0.5)
                {
                    if (bulletsLeft > 0)
                    {
                        RPC_Shoot();
                        //  Handheld.Vibrate();
                    }

                }
                break;
            case MultiplayerMoveAndShoot.ControlType.WASD:
                if (Input.GetMouseButton(0))
                {
                    if (bulletsLeft > 0)
                    {
                        RPC_Shoot();
                        //  Handheld.Vibrate();
                    }

                }
                break;
        }
       



        ammoBar.value = bulletsLeft;

        // finished bullets
        if (bulletsLeft <= 0)
        {
            //  inventory.isFilled = false;
            //   weaponHolder.RPC_DropGun(); // carries the essential RPC

            // wait a while

            StartCoroutine(WaitBeforeRefill());
         
          
        }



    }
    IEnumerator WaitBeforeRefill()
    {
        yield return new WaitForSeconds(1f);
        bulletsLeft = MaxBullets;
        ammoBar.maxValue = 100;
    }
    [Rpc]
    public void RPC_Shoot()
    {

        if (bulletsLeft > 0)
        {
            if (timebtwShots <= 0)
            {
                GameObject bulletInstance = Instantiate(bulletPrefab, shootingTip.position, shootingTip.rotation); // doing this will run physics on
                // only the state authority's device. only the visuals is networked


                //  NetworkObject bulletInstance =  FusionNetworkManager.runnerInstance.Spawn(bulletPrefab, shootingTip.position, shootingTip.rotation);
                // if we spawn the network object in every shot, it causes network lag
                var hit = Physics2D.Raycast(shootingTip.position, transform.right, range, hitMask);

                if (hit)
                {
                    Debug.DrawLine(shootingTip.position, hit.point, Color.green, 1f); // green line to hit
                    var player = hit.collider.GetComponent<MultiplayerMoveAndShoot>();
                    if (player != null)
                    {
                        player.RegisterAttacker(Attacker);
                        player.TakeDamage_RPC(damage);
                       
                    }
                      
                }
                else
                {
                    Debug.DrawLine(shootingTip.position, shootingTip.position +transform.right * range, Color.red, 1f); // red line if miss
                }

                bulletInstance.GetComponent<Rigidbody2D>().velocity = transform.right * Bulletspeed;
                // play sound
                AudioMana.instance.PlaySound(shootingSound);
                // shake camera
                ScreenShake.instance.shakeCamera(intensity, time);

                // play recoil animation
                Anim.SetTrigger("Shoot");

                Instantiate(muzzleflash, shootingTip.position, Quaternion.identity);
                bulletsLeft--;
                timebtwShots = starttimebtwShots;

            }
            else
            {
                timebtwShots -= Time.deltaTime;
            }
        }
        
    }
   
}
