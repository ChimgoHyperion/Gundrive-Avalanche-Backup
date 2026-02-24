using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class FireAndIceMulti : MonoBehaviour
{

    ParticleSystem flamethrower;
    public GameObject particleOBJ;
    public Transform firePos;
    public float bulletsleft;
    
    // weapon movement
  
    Vector2 Rotation;
    private float rotation2;
    private float rotation3;
    public bool facingRight = true;

    
    public GameObject detectCollision;
    //  public AudioClip shootingClip;
    AudioSource source;
    // multi
    PhotonView view;
    public PhotonView playerview;
    private int myshooterId;
    public WeaponHolderMulti multi;
    public int startBullets;
    public FlameFreeze Firedetector;
    public flameIgnite iceDetector;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        GameObject flamethrowerobj =PhotonNetwork.Instantiate(particleOBJ.name,firePos.position,Quaternion.identity);
        flamethrowerobj.transform.SetParent(firePos);
       // flamethrowerobj.transform.localPosition = firePos.localPosition;
        flamethrower = flamethrowerobj.GetComponent<ParticleSystem>();
        flamethrower.Stop();
        source = GetComponent<AudioSource>();
        if (Firedetector != null)
        {
           
        }
        if(iceDetector!= null)
        {
          
        }
      //  iceDetector.shooterId = playerview.ViewID; // a different script should be made for ice
        Firedetector.shooterId = playerview.ViewID;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
           
            //  iceDetector.shooterId = playerview.ViewID;
            Firedetector.shooterId = playerview.ViewID;

            if (GetComponent<GeneralGun>().willShoot == false)
            {
                detectCollision.SetActive(false);

                flamethrower.Stop();
            }
            else
            {
                if (bulletsleft > 0)
                {
                    detectCollision.SetActive(true);
                    flamethrower.Play();
                    bulletsleft--;
                }
            }

            if (bulletsleft <= 0)
            {
                gameObject.SetActive(false);
              //  multi.hasWeapon = false;
                bulletsleft = startBullets;
                view.RPC(nameof(Deactivate), RpcTarget.AllBuffered);
            }
            if (this.gameObject.activeSelf == false)
            {
                view.RPC(nameof(Deactivate2), RpcTarget.AllBuffered);
            }
        }
     
    }

    [PunRPC]
    void Deactivate()
    {
        gameObject.SetActive(false);
//        multi.hasWeapon = false;
        bulletsleft = startBullets;
    }
    [PunRPC]
    void Deactivate2()
    {
        gameObject.SetActive(false);
        //  multi.hasWeapon = false;
        bulletsleft = startBullets;
    }

    public void Shoot(bool willShoot)
    {
        view.RPC(nameof(ShootMulti), RpcTarget.AllBuffered, true);
    }
    [PunRPC] 
    public void ShootMulti(bool willShoot)
    {

        if (willShoot == true)
        {
            if (bulletsleft > 0)
            {
                detectCollision.SetActive(true);
                flamethrower.Play();
                bulletsleft--;
            }
        }
        if (willShoot == false)
        {

            detectCollision.SetActive(false);
            flamethrower.Stop();
        }
        else
        {

        }
    }
    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

}
