using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform laserPosition;
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
    
   
    public AudioClip shootingClip;
    AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.enabled = true;
       
      
        source = GetComponent<AudioSource>();

    }


    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right);
        lineRenderer.SetPosition(0, laserPosition.position);
        RaycastHit2D hitEnemy = Physics2D.Raycast(firePoint.position, firePoint.right);

        if (hitEnemy)
        {
         
            MovementandShooting takeDamage = hitEnemy.transform.GetComponent<MovementandShooting>();
           
            if (lineRenderer.enabled == true)
            {
                if (takeDamage != null)
                {
                    takeDamage.TakeDamage(damage);
                    Instantiate(hitEffect, hitEnemy.point, Quaternion.identity);
                }
               

            }

        }


        if (hit)
        {
            lineRenderer.SetPosition(1, hit.point);
            if (lineRenderer.enabled == true)
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


      /*  if (Mathf.Abs(joystick.Horizontal) > 0.5 || Mathf.Abs(joystick.Vertical) > 0.5)
        {
            if (bulletsleft > 0)
            {
                lineRenderer.enabled = true;
                bulletsleft--;
                source.enabled = true;
            }

        }
        else
        {
            lineRenderer.enabled = false;
            source.enabled = false;

        }*/
      
       /* if (bulletsleft <= 0)
        {

            Destroy(gameObject);
           
        }*/
    }
  /*  private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }*/
}
