using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireBallGun : MonoBehaviour
{
    public Transform shootingTip;


    // for shooting
    public GameObject bulletPrefab;

    private float timebtwShots;
    public float starttimebtwShots;
    public float Bulletspeed;
    // public Slider ammoBar;
    public int bulletsLeft;

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

    public MovementandShooting movementandShooting;
    // Start is called before the first frame update
    void Start()
    {
        shootingTip.rotation = transform.rotation;
        joystick = GameObject.FindGameObjectWithTag("WeaponStick").GetComponent<FixedJoystick>();
        ammoBar = GameObject.FindGameObjectWithTag("AmmoBar").GetComponent<Slider>();
        ammoBar.maxValue = bulletsLeft;

    }
    public void Shoot()
    {

        if (timebtwShots <= 0)
        {
            /*GameObject bulletInstance = Instantiate(bulletPrefab, shootingTip.position,shootingTip.rotation);

             bulletInstance.GetComponent<Rigidbody2D>().velocity = transform.right * Bulletspeed;*/

            GameObject bullet = ObjectPool.instance.GetPooledFireBall();
            if (bullet != null)
            {
                bullet.transform.position = shootingTip.position;
                bullet.SetActive(true);
                bullet.GetComponent<Rigidbody2D>().velocity = transform.right * Bulletspeed;
                AudioMana.instance.PlaySound(shootingClip);
            }


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
        movementandShooting = GetComponentInParent<MovementandShooting>();
        switch (movementandShooting.controlType)
        {
            case MovementandShooting.ControlType.Joystick:
                if (Mathf.Abs(joystick.Horizontal) > 0.5 || Mathf.Abs(joystick.Vertical) > 0.5)
                {
                    if (bulletsLeft > 0)
                    {
                        Shoot();
                    }

                }
                break;
            case MovementandShooting.ControlType.WASD:
                if (Input.GetMouseButton(0))
                {
                    if (bulletsLeft > 0)
                    {
                        Shoot();
                    }

                }
                break;
        }

        if (ammoBar != null)
            ammoBar.value = bulletsLeft;
        if (bulletsLeft <= 0)
        {
            Destroy(gameObject);
            ammoBar.maxValue = 100;
        }

    }


}
