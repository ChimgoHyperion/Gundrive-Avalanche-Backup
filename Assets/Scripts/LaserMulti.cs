using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LaserMulti : MonoBehaviourPunCallbacks,IPunObservable
{
    public GameObject laser;
    public LineRenderer lineRenderer;
    public Transform laserPosition;
    public Transform gunTip;
    public int damage;
    public GameObject hitEffect;
    public float range;
    public Transform firePoint;
    // weapon movement
   
    public GameObject Object;
    Vector2 Rotation;
    private float rotation2;
    private float rotation3;
    public bool facingRight = true;
    
    public float bulletsleft;
    public GameObject muzzleflash;
    // audio
    public AudioClip shootingClip;
    AudioSource source;
    // multi
    public WeaponHolderMulti multi;
    PhotonView view;
    public PhotonView playerview;
    private int myshooterId;
    public int startBullets;
    public PhotonTransformView transformView;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        view.RPC(nameof(DisableRenderer), RpcTarget.AllBuffered);

        source = GetComponent<AudioSource>();
        source.enabled = false;
        

    }

    [PunRPC]
    void Happen()
    {
        RaycastHit2D hit = Physics2D.Raycast(gunTip.position, transform.right);
        lineRenderer.SetPosition(0, laserPosition.position);
        RaycastHit2D hitEnemy = Physics2D.Raycast(firePoint.position, firePoint.right);

        if (hitEnemy)
        {
            MultiplayerMoveAndShoot player = hitEnemy.transform.GetComponent<MultiplayerMoveAndShoot>();
            if(laser.activeSelf== true)
            if (player != null)
            {
               // player.setshooter(playerview.ViewID); // we dont use the set setshooter to zero in laser because theres no
                                                      // way we can use a laser against ourselves
                player.TakeDamage_RPC(damage); // we arent using slowly damage . for some reason take damage works
                PhotonNetwork.Instantiate(hitEffect.name, hitEnemy.point, Quaternion.identity); // multiplayer script
            }
            if (lineRenderer.enabled == true)
            {
               
               

            }

        }


        if (hit)
        {
            lineRenderer.SetPosition(1, hit.point);
            if(laser.activeSelf== true)
            if (hit.collider.gameObject.CompareTag("World"))
            {
                PhotonNetwork.Instantiate(hitEffect.name, hit.point, Quaternion.identity);
            }
            if (lineRenderer.enabled == true)
            {
               
            }

        }
        else
        {
            lineRenderer.SetPosition(1, transform.right * 100);
        }
        

        if (GetComponent<GeneralGun>().willShoot == false)
        {
            view.RPC(nameof(DisableRenderer), RpcTarget.AllBuffered);
            source.enabled = false;
        }
        else
        {

            if (bulletsleft > 0)
            {
                //  Instantiate(muzzleflash, firePoint.position, Quaternion.identity);
                view.RPC(nameof(EnableRenderer), RpcTarget.AllBuffered);
                bulletsleft--;
                source.enabled = true;
            }
        }

        if (bulletsleft <= 0)
        {
            gameObject.SetActive(false);
//            multi.hasWeapon = false;
            bulletsleft = startBullets;
            view.RPC(nameof(Deactivate), RpcTarget.AllBuffered);
        }
        if (this.gameObject.activeSelf == false)
        {
            view.RPC(nameof(Deactivate2), RpcTarget.AllBuffered);
        }
    }

    // Update is called once per frame
    void Update()
    {
        view.RPC(nameof(Happen), RpcTarget.All);
       
    }

    [PunRPC]
    void Deactivate()
    {
        gameObject.SetActive(false);
     //   multi.hasWeapon = false;
        bulletsleft = startBullets;
    }
    [PunRPC]
    void Deactivate2()
    {
        gameObject.SetActive(false);
        //  multi.hasWeapon = false;
        bulletsleft = startBullets;
    }
   
    public void Shoot( bool willshoot)
    {
        view.RPC(nameof(ShootMulti), RpcTarget.AllBuffered, true);
     
    }
    [PunRPC] 
    public void ShootMulti(bool willshoot)
    {

        if (willshoot == true)
        {
            if (bulletsleft > 0)
            {
                // Instantiate(muzzleflash, firePoint.position, Quaternion.identity); too heavy
                view.RPC(nameof(EnableRenderer), RpcTarget.AllBuffered);
                bulletsleft--;
                source.enabled = true;
            }
        }
        if (willshoot == false)
        {
            view.RPC(nameof(DisableRenderer), RpcTarget.AllBuffered);
            source.enabled = false;
        }
    }
    [PunRPC]
    void EnableRenderer()
    {
      //  lineRenderer.enabled = true;
        laser.SetActive(true);
    }
    [PunRPC]
    void DisableRenderer()
    {
      //  lineRenderer.enabled = false;
        laser.SetActive(false);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       
    }
}
