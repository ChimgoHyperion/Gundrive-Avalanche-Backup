using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireSprayScript : MonoBehaviour
{

    public ParticleSystem flamethrower, yellow;
    public float bulletsleft;

    // weapon movement
    public Joystick joystick;
    public GameObject Object;
    Vector2 Rotation;
    private float rotation2;
    private float rotation3;
    public bool facingRight = true;
    
    public Slider ammoBar;
    public GameObject detectCollision;
    //  public AudioClip shootingClip;
    AudioSource source;
    // Start is called before the first frame update

    public MovementandShooting movementandShooting;
    void Start()
    {
        joystick = GameObject.FindGameObjectWithTag("WeaponStick").GetComponent<FixedJoystick>();
        flamethrower.Stop();
        
        yellow.Stop();
        ammoBar = GameObject.FindGameObjectWithTag("AmmoBar").GetComponent<Slider>();
        ammoBar.maxValue = bulletsleft;
        source = GetComponent<AudioSource>();

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
                    if (bulletsleft > 0)
                    {
                        detectCollision.SetActive(true);
                        //  AudioMana.instance.PlaySound(shootingClip); 
                        // source.Play();
                        flamethrower.Play();

                        yellow.Play();
                        bulletsleft--;
                    }

                }
                else
                {
                    detectCollision.SetActive(false);
                    //  source.Stop();
                    //  AudioMana.instance.StopSound();
                    flamethrower.Stop();

                    yellow.Stop();
                }
                break;
            case MovementandShooting.ControlType.WASD:

                if (Input.GetMouseButton(0))
                {
                    if (bulletsleft > 0)
                    {
                        detectCollision.SetActive(true);
                        //  AudioMana.instance.PlaySound(shootingClip); 
                        // source.Play();
                        flamethrower.Play();

                        yellow.Play();
                        bulletsleft--;
                    }

                }
                else
                {
                    detectCollision.SetActive(false);
                    //  source.Stop();
                    //  AudioMana.instance.StopSound();
                    flamethrower.Stop();

                    yellow.Stop();
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
