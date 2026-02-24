using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;


public class LaserGunMulti : NetworkBehaviour
{
    public LineRenderer lineRenderer;
    public Transform laserPosition;
    public Transform gunTip;
    public int damage;
    public GameObject hitEffect;
    public float range;
    public Transform firePoint;
    // weapon movement
    public Joystick joystick;
    public GameObject Obj;
   
    public bool facingRight = true;
    public Slider ammoBar;
    public float bulletsLeft;
    public int MaxBullets;
    public GameObject muzzleflash;
    // audio
    public AudioClip shootingClip;
    AudioSource source;

    public GameObject Attacker;// set by the player
    public Inventory inventory;
    public WeaponHolderMulti weaponHolder;

    public MultiplayerMoveAndShoot movementandShooting;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.enabled = false;
       
        ammoBar.maxValue = bulletsLeft;
        source = GetComponent<AudioSource>();

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
                        Instantiate(muzzleflash, firePoint.position, Quaternion.identity);
                        //  lineRenderer.enabled = true;
                        RPC_Shoot();
                        bulletsLeft--;
                        source.enabled = true;
                    }
                }
                else
                {
                    //  lineRenderer.enabled = false;
                    RPC_DeactivateLineRenderer();
                    source.enabled = false;

                }
                break;
            case MultiplayerMoveAndShoot.ControlType.WASD:

                if (Input.GetMouseButton(0))
                {

                    if (bulletsLeft > 0)
                    {
                        Instantiate(muzzleflash, firePoint.position, Quaternion.identity);
                        //  lineRenderer.enabled = true;
                        RPC_Shoot();
                        bulletsLeft--;
                        source.enabled = true;
                    }
                }
                else
                {
                    //  lineRenderer.enabled = false;
                    RPC_DeactivateLineRenderer();
                    source.enabled = false;

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
    void RPC_Shoot()
    {
        lineRenderer.enabled = true;

        RaycastHit2D hit = Physics2D.Raycast(gunTip.position, transform.right);
        lineRenderer.SetPosition(0, laserPosition.position);
        RaycastHit2D hitEnemy = Physics2D.Raycast(firePoint.position, firePoint.right);

        //if (hit)
        //{
        //    Debug.DrawLine(firePoint.position, hit.point, Color.green, 1f); // green line to hit
        //    var player = hit.collider.GetComponent<MultiplayerMoveAndShoot>();
        //    if (player != null)
        //    {
        //        player.TakeDamage_RPC(damage);
        //        player.RegisterAttacker(Attacker);
        //    }

        //}
        //else
        //{
        //    Debug.DrawLine(firePoint.position, firePoint.position + transform.right * range, Color.red, 1f); // red line if miss
        //}

        if (hitEnemy)
        {
            MultiplayerMoveAndShoot enemy = hitEnemy.transform.GetComponent<MultiplayerMoveAndShoot>();



            if (lineRenderer.enabled == true)
            {
                if (enemy != null)
                {
                    enemy.RegisterAttacker(Attacker);
                    enemy.TakeDamage_RPC(damage);
                   
                    Instantiate(hitEffect, hitEnemy.point, Quaternion.identity);
                }


            }

        }


        if (hit)
        {
            lineRenderer.SetPosition(1, hit.point);
            if (lineRenderer.enabled == true)
            {
                if (hit.collider.gameObject.CompareTag("World"))
                {
                    Instantiate(hitEffect, hit.point, Quaternion.identity);
                }
            }

        }
        else
        {
            lineRenderer.SetPosition(1, transform.right * 100);
        }

    }

    [Rpc]
    void RPC_DeactivateLineRenderer()
    {
        lineRenderer.enabled = false;
    }
}
