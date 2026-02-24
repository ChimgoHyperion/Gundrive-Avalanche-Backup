using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;


public class ElectricGunMulti : NetworkBehaviour
{
    public ParticleSystem electicEffect;
    public float bulletsleft,MaxBullets;

    // weapon movement
    public Joystick joystick;

  
    public bool facingRight = true;
   
    public Slider ammoBar;
    public Inventory inventory;
    public WeaponHolderMulti weaponHolder;
    public Transform ShootingPoint;

    // Start is called before the first frame update
    void Start()
    {
        
        electicEffect.Stop();
        ammoBar.maxValue = bulletsleft;
       
    }

    // Update is called once per frame
    void Update()
    {

        if (Mathf.Abs(joystick.Horizontal) > 0.5 || Mathf.Abs(joystick.Vertical) > 0.5)
        {
            if (bulletsleft > 0)
            {

                RPC_Shoot();
                bulletsleft--;
            }

        }
        else
        {
            RPC_Stop();
        }
        ammoBar.value = bulletsleft;
        if (bulletsleft <= 0)
        {
            inventory.isFilled = false;
            bulletsleft = MaxBullets;
            ammoBar.maxValue = 100;
            //  gameObject.SetActive(false);// should be networked
            weaponHolder.RPC_DropGun(); // carries the essential RPC
        }
    }

    [Header("Detector")]
    public int damage;
    public GameObject Attacker;
    public float radius;
    public LayerMask layertoHit;

    [Rpc]
    public void RPC_Shoot()
    {
        electicEffect.Play();
        GetComponent<AudioSource>().enabled = true;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(ShootingPoint.position, radius, layertoHit);

        foreach (Collider2D obj in colliders)
        {

            MultiplayerMoveAndShoot enem = obj.GetComponent<MultiplayerMoveAndShoot>();

            if (enem.gameObject == Attacker)
            {
                return;// dont damage ourselves 
            }

            if (enem != null)
            {
                enem.TakeDamage_RPC(damage);
                enem.RegisterAttacker(Attacker);
            }



        }
    }

    [Rpc]
    public void RPC_Stop()
    {
        electicEffect.Stop();
        GetComponent<AudioSource>().enabled = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(ShootingPoint.position, radius);
    }
}
