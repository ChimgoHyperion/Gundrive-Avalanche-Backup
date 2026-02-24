using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class BulletsMulti : MonoBehaviour
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
    public float intensity;
    public float time;
    // public Animator gunAnimator;

    // weapon movement
  
    // audio
    public AudioClip shootingSound;

    public WeaponHolderMulti multi;
    public int startBullets;
    PhotonView view;
    public PhotonView playerview;
    public int myshooterId;
    public buttonSoundHolder soundHolder;
    // Start is called before the first frame update
    void Start()
    {
        shootingTip.rotation = transform.rotation;
        view = GetComponent<PhotonView>();
        myshooterId = playerview.ViewID;
    }
    public void Shoot()
    {
        if (view.IsMine)
        {

        }
       

        view.RPC(nameof(ShootMult), RpcTarget.AllBuffered);
    }
    [PunRPC] void ShootMult()
    {
        if (bulletsLeft > 0)
        {
            if (timebtwShots <= 0)
            {
                GameObject bulletInstance = PhotonNetwork.Instantiate(bulletPrefab.name, shootingTip.position, shootingTip.rotation);
                bulletInstance.GetComponent<firebulletprojectile>().shooterId = myshooterId;
                bulletInstance.GetComponent<Rigidbody2D>().velocity = transform.right * Bulletspeed;
                // play sound
                soundHolder.PlayfromSource(shootingSound);
                // shake camera
                ScreenShake.instance.shakeCamera(intensity, time);

                // play recoil animation
                //  gunAnimator.SetTrigger("Shoot");
               PhotonNetwork. Instantiate(muzzleflash.name, shootingTip.position, Quaternion.identity);
                bulletsLeft--;
                timebtwShots = starttimebtwShots;

            }
            else
            {
                timebtwShots -= Time.deltaTime;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        myshooterId = playerview.ViewID;

        if (bulletsLeft <= 0)
        {
            gameObject.SetActive(false);
          //  multi.hasWeapon = false;
            bulletsLeft = startBullets;
            view.RPC(nameof(Deactivate), RpcTarget.AllBuffered);
        }
        if (this.gameObject.activeSelf == false)
        {
            view.RPC(nameof(Deactivate2), RpcTarget.AllBuffered);
        }

    }
    [PunRPC]
    void Deactivate()
    {
        gameObject.SetActive(false);
      //  multi.hasWeapon = false;
        bulletsLeft = startBullets;
    }
    [PunRPC]
    void Deactivate2()
    {
        gameObject.SetActive(false);
        //  multi.hasWeapon = false;
        bulletsLeft = startBullets;
    }

}
