using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserEnemyScript : MonoBehaviour
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
    public GameObject Object;
    Vector2 Rotation;
    private float rotation2;
    private float rotation3;
    public bool facingRight = true;
    public Slider ammoBar;
    public float bulletsleft;
    public GameObject muzzleflash;
    // audio
    public AudioClip shootingClip;
    AudioSource source;

    public MovementandShooting movementandShooting;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.enabled = false;
        joystick = GameObject.FindGameObjectWithTag("WeaponStick").GetComponent<FixedJoystick>();
        ammoBar = GameObject.FindGameObjectWithTag("AmmoBar").GetComponent<Slider>();
        ammoBar.maxValue = bulletsleft;
        source = GetComponent<AudioSource>();
        
    }
     

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(gunTip.position, transform.right);
        lineRenderer.SetPosition(0, laserPosition.position);
        RaycastHit2D hitEnemy = Physics2D.Raycast(firePoint.position, firePoint.right);

        if(hitEnemy)
        {
            EnemyScript takeDamage = hitEnemy.transform.GetComponent<EnemyScript>();

            TakeDamageandDisappear enemy5 = hitEnemy.transform.GetComponent<TakeDamageandDisappear>();

            BossHealthScript boss = hitEnemy.transform.GetComponent<BossHealthScript>();

            if(lineRenderer.enabled== true)
            {
                if (takeDamage != null)
                {
                    takeDamage.TakeDamage(damage);
                    Instantiate(hitEffect, hitEnemy.point, Quaternion.identity);
                }
                if(enemy5!= null)
                {
                    enemy5.TakeDamage(damage);
                    Instantiate(hitEffect, hitEnemy.point, Quaternion.identity);
                }
                if (boss != null)
                {
                    boss.TakeDamage(damage);
                    Instantiate(hitEffect, hitEnemy.point, Quaternion.identity);
                }

            }
           
        }
       
       
        if (hit)
        {
            lineRenderer.SetPosition(1, hit.point);
            if(lineRenderer.enabled== true)
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

        movementandShooting = GetComponentInParent<MovementandShooting>();
        switch (movementandShooting.controlType)
        {
            case MovementandShooting.ControlType.Joystick:
                if (Mathf.Abs(joystick.Horizontal) > 0.5 || Mathf.Abs(joystick.Vertical) > 0.5)
                {

                    if (bulletsleft > 0)
                    {
                        Instantiate(muzzleflash, firePoint.position, Quaternion.identity);
                        lineRenderer.enabled = true;
                        bulletsleft--;
                        source.enabled = true;
                    }
                }
                else
                {
                    lineRenderer.enabled = false;
                    source.enabled = false;

                }
                break;
            case MovementandShooting.ControlType.WASD:
                if (Input.GetMouseButton(0))
                {

                    if (bulletsleft > 0)
                    {
                        Instantiate(muzzleflash, firePoint.position, Quaternion.identity);
                        lineRenderer.enabled = true;
                        bulletsleft--;
                        source.enabled = true;
                    }
                }
                else
                {
                    lineRenderer.enabled = false;
                    source.enabled = false;

                }
                break;
        }

        if (ammoBar != null)
            ammoBar.value = bulletsleft;
        if (bulletsleft <= 0)
        {
        
            Destroy(gameObject);
            ammoBar.maxValue = 100;
        }
    }
    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
  
}
