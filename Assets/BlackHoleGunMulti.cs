using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class BlackHoleGunMulti : NetworkBehaviour
{
    public Transform shootingTip;


    // for shooting
    public GameObject bulletPrefab;

    private float timebtwShots;
    public float starttimebtwShots;
    public float Bulletspeed;
    // public Slider ammoBar;
    public int bulletsLeft,MaxBullets;

    public float offset;

    public GameObject muzzleflash;

    // public Animator gunAnimator;
    public float intensity;
    public float time;
    // weapon movement
    public Joystick joystick;
    public Slider ammoBar;

    // audio
    public AudioClip shootingClip;

    public Inventory inventory;
    public WeaponHolderMulti weaponHolder;

    public GameObject Attacker; // set to the player wielding this gun

    public MultiplayerMoveAndShoot movementandShooting;
    // Start is called before the first frame update
    void Start()
    {
        shootingTip.rotation = transform.rotation;
       

    }

    [Rpc]
    public void RPC_Shoot()
    {

        if (timebtwShots <= 0)
        {
            //  GameObject bulletInstance = Instantiate(bulletPrefab, shootingTip.position, Quaternion.identity);

            // bulletInstance.GetComponent<Rigidbody2D>().velocity = transform.right * Bulletspeed;
            NetworkObject bulletInstance = FusionNetworkManager.runnerInstance.Spawn(bulletPrefab, shootingTip.position, shootingTip.rotation);


            bulletInstance.GetComponent<BlackHoleDamageMulti>().Attacker = Attacker;


            AudioMana.instance.PlaySound(shootingClip);


            // shake camera
            ScreenShake.instance.shakeCamera(intensity, time);
            // play recoil animation
            /* gunAnimator.SetTrigger("Shoot");*/
            Instantiate(muzzleflash, shootingTip.position, Quaternion.identity);
            bulletsLeft--;
            timebtwShots = starttimebtwShots;

        }
        else
        {
            timebtwShots -= Time.deltaTime;
        }
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
                    }

                }
                break;
            case MultiplayerMoveAndShoot.ControlType.WASD:
                if (Input.GetMouseButton(0))
                {
                    if (bulletsLeft > 0)
                    {
                        RPC_Shoot();
                    }

                }
                break;
        }



       
        ammoBar.value = bulletsLeft;
        if (bulletsLeft <= 0)
        {
            StartCoroutine(WaitBeforeRefill());
        }

    }
    IEnumerator WaitBeforeRefill()
    {
        yield return new WaitForSeconds(1f);
        bulletsLeft = MaxBullets;
        ammoBar.maxValue = 100;
    }
}
