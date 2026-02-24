using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class miniBombGun : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject miniBomb;
    public float range = 15f;

    // weapon movement
    public Joystick joystick;
   // public GameObject muzzleflash;
    private float timebtwShots;
    public float starttimebtwShots;
    public float bulletsLeft;
    public Slider ammoBar;
    // audio
    public AudioClip shootingClip;

    public MovementandShooting movementandShooting;
   

    // Start is called before the first frame update
    void Start()
    {
        joystick = GameObject.FindGameObjectWithTag("WeaponStick").GetComponent<FixedJoystick>();
        ammoBar = GameObject.FindGameObjectWithTag("AmmoBar").GetComponent<Slider>();
        ammoBar.maxValue = bulletsLeft;
    }

    // Update is called once per frame
    void Update()
    {
        movementandShooting= GetComponentInParent<MovementandShooting>();
        //if (movementandShooting != null)
        //{
        //    return;
        //}


        switch (movementandShooting.controlType)
        {
            case MovementandShooting.ControlType.Joystick:
                if (Mathf.Abs(joystick.Horizontal) > 0.5 || Mathf.Abs(joystick.Vertical) > 0.5)
                {
                    if (bulletsLeft > 0)
                        Launch();
                }
                break;
            case MovementandShooting.ControlType.WASD:
                if (Input.GetMouseButton(0))
                {
                    if (bulletsLeft > 0)
                        Launch();
                }
                break;
        }



       
        ammoBar.value = bulletsLeft;
        if (bulletsLeft <= 0)
        {
            Destroy(gameObject);
            ammoBar.maxValue = 100;
        }
    }
    
    public void Launch()
    {
        GameObject bomb = ObjectPool.instance.GetPooledMiniBomb();
        if (timebtwShots <= 0)
        {

            if (bomb != null)
            {
                bomb.transform.position = spawnPoint.position;
                bomb.SetActive(true);
                bomb.GetComponent<Rigidbody2D>().velocity = transform.right * range;
              //  Instantiate(muzzleflash, spawnPoint.position, Quaternion.identity);
                AudioMana.instance.PlaySound(shootingClip);
            }
          
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
