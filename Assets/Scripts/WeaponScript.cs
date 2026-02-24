using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponScript : MonoBehaviour
{

    public FixedJoystick rotationStick;
    
    public Transform shootingTip;
    
  
    // for shooting
    public GameObject bulletPrefab;
    
    private float timebtwShots;
    public float starttimebtwShots ;
    public float Bulletspeed;
    public Slider ammoBar;
    public int bulletsLeft = 100;
    
   // public GameObject muzzleflash;

   // public Animator gunAnimator;

    // Start is called before the first frame update
    void Start()
    {
        shootingTip.rotation = transform.rotation;
    }

    public void shootBullet()
    {
        
        if (timebtwShots <= 0)
        {
           Instantiate(bulletPrefab, shootingTip.position,shootingTip.rotation);
           // shake camera
           ScreenShake.instance.shakeCamera(5f, 0.1f);
           // play recoil animation
           /* gunAnimator.SetTrigger("Shoot");
           // GameObject BulletIns = Instantiate(bulletPrefab, shootingTip.position, shootingTip.rotation);
           // BulletIns.GetComponent<Rigidbody2D>().AddForce(BulletIns.transform.right * Bulletspeed); // tweakable
             bulletsLeft--;
          

            Instantiate(muzzleflash, shootingTip.position, Quaternion.identity);*/

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
        
        if (Mathf.Abs(rotationStick.Horizontal) > 0.5 || Mathf.Abs( rotationStick.Vertical) > 0.5 ) 
        {
            if(bulletsLeft>=0)
            shootBullet();
        }
        ammoBar.value = bulletsLeft;
      
    }
}
