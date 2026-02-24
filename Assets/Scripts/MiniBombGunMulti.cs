using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;


public class MiniBombGunMulti : NetworkBehaviour
{

    public Transform shootingTip;
    public GameObject miniBomb;
    public float range = 15f;

    // weapon movement
    public Joystick joystick;
    // public GameObject muzzleflash;
    private float timebtwShots;
    public float starttimebtwShots;
    public float bulletsLeft;
    public float MaxBullets;
    public Slider ammoBar;
    // audio
    public AudioClip shootingClip;
    public Inventory inventory;
    public WeaponHolderMulti weaponHolder;

    public GameObject Attacker;// set by the player

    public MultiplayerMoveAndShoot movementandShooting;
    // Start is called before the first frame update
    void Start()
    {
      
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
                        RPC_Launch();
                }
                break;
            case MultiplayerMoveAndShoot.ControlType.WASD:
                if (Input.GetMouseButton(0))
                {
                    if (bulletsLeft > 0)
                        RPC_Launch();
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
    [Rpc]
    public void RPC_Launch()
    {
       
        if (timebtwShots <= 0)
        {
            NetworkObject bulletInstance = FusionNetworkManager.runnerInstance.Spawn(miniBomb, shootingTip.position, Quaternion.identity);// if
            // you use shooting tip.rotation, the projectile will disappear when the player switches rotation

            bulletInstance.GetComponent<MiniBombProjectileMulti>().Attacker = Attacker;
            bulletInstance.GetComponent<Rigidbody2D>().velocity = transform.right * range;

            AudioMana.instance.PlaySound(shootingClip);

            // shake camera
          //  ScreenShake.instance.shakeCamera(intensity, time);
            

            bulletsLeft--;
            timebtwShots = starttimebtwShots;
        }
        else
        {
            timebtwShots -= Time.deltaTime;
        }


        // GameObject bombInstance = Instantiate(miniBomb, spawnPoint.position, spawnPoint.rotation);
        //  bombInstance.GetComponent<Rigidbody2D>().AddForce(Vector2.right * range, ForceMode2D.Impulse);
        // bombInstance.GetComponent<Rigidbody2D>().velocity = transform.right * range;


    }
}
