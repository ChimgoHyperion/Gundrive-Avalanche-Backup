using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralGun : MonoBehaviour
{
    GunShootingScript gunshooting;
    FireBallGun fireball;
    FireAndIceMulti fireandIce;
    LaserEnemyScript laser;
    MiniBombGunMulti minibomb;

   public int chosenScript;
    public int maxAmmo;
    public float currentBulletsLeft;

    public bool willShoot;
    // Start is called before the first frame update
    void Start()
    {
        if(TryGetComponent<MiniBombGunMulti>(out MiniBombGunMulti minibomb))
        {
            chosenScript = 5;
         //  minibomb.LaunchMulti();
            
        }
        if (TryGetComponent<LaserEnemyScript>(out LaserEnemyScript laser))
        {
            chosenScript = 4;
        }
    }
    
   public void checkGun()
   {
        /*   switch (chosenScript)
           {
               case 5:
                   minibomb.Launch();
                   break;
               case 4:
               //   laser.Shoot();
                   break;
           }*/

        if (TryGetComponent<MiniBombGunMulti>(out MiniBombGunMulti miniBomb))
        {
            chosenScript = 5;
           // miniBomb.LaunchMulti();
            currentBulletsLeft = miniBomb.bulletsLeft;

        }

        if(TryGetComponent<LaserMulti>(out LaserMulti laserMulti))
        {
            chosenScript = 4;
            laserMulti.Shoot(willShoot);
            currentBulletsLeft = laserMulti.bulletsleft;
        }

        if (TryGetComponent<FireAndIceMulti>(out FireAndIceMulti fireAndIce))
        {
            chosenScript = 3;
            fireAndIce.Shoot(willShoot);
            currentBulletsLeft = fireAndIce.bulletsleft;
        }
        if (TryGetComponent<FireBallGunMulti>(out FireBallGunMulti fireBall))
        {
            chosenScript = 2;
            //fireBall.Shoot();
            //currentBulletsLeft = fireBall.bulletsLeft;
        }
        if (TryGetComponent<BulletsMulti>(out BulletsMulti bulletsMulti) )
        {
            chosenScript = 1;
            bulletsMulti.Shoot();
            currentBulletsLeft = bulletsMulti.bulletsLeft;
        }
   }
  
    // Update is called once per frame
    void Update()
    {
    }
}
