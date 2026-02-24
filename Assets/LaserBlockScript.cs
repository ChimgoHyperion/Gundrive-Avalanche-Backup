using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBlockScript : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform laserPosition;
    public Transform gunTip;
    public int damage;
    public GameObject hitEffect;
    public float range;
    public Transform firePoint;

    public LineRenderer LaserGameObject;
    public float WaitTime;
    // Start is called before the first frame update
    void Start()
    {
        //  lineRenderer.enabled = false;
        Alt();
    }
    void Alt()
    {
        StartCoroutine(Alternate());
    }
    IEnumerator Alternate()
    {
        yield return new WaitForSeconds(WaitTime);
        LaserGameObject.enabled = true;
        yield return new WaitForSeconds(WaitTime);
        LaserGameObject.enabled = false;
        Alt();

    }
    // Update is called once per frame
    void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(gunTip.position, transform.up);
        lineRenderer.SetPosition(0, laserPosition.position);
        RaycastHit2D hitEnemy = Physics2D.Raycast(firePoint.position, firePoint.up);

        if (hitEnemy)
        {
            EnemyScript enemy = hitEnemy.transform.GetComponent<EnemyScript>();

            TakeDamageandDisappear enemy5 = hitEnemy.transform.GetComponent<TakeDamageandDisappear>();

            BossHealthScript boss = hitEnemy.transform.GetComponent<BossHealthScript>();

            MovementandShooting player = hitEnemy.transform.GetComponent<MovementandShooting>();

            if (lineRenderer.enabled == true)
            {
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                  //  Instantiate(hitEffect, hitEnemy.point, Quaternion.identity);
                }
                if (enemy5 != null)
                {
                    enemy5.TakeDamage(damage);
                    Instantiate(hitEffect, hitEnemy.point, Quaternion.identity);
                }
                if (boss != null)
                {
                    boss.TakeDamage(damage);
                   // Instantiate(hitEffect, hitEnemy.point, Quaternion.identity);
                }

                if(player!= null)
                {
                    player.TakeDamage(damage);
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
                  //  Instantiate(hitEffect, hit.point, Quaternion.identity);
                }
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                  //  Instantiate(hitEffect, hit.point, Quaternion.identity);
                }
            }

        }
        else
        {
            lineRenderer.SetPosition(1, transform.up * 100);
        }
    }
}
