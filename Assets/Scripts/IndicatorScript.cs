using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorScript : MonoBehaviour
{
    public GameObject indicator;
    public int minimumDist;
     GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        indicator.SetActive(true);
    }

    private void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, player.transform.position);
        if (ray.collider.gameObject.CompareTag("Player"))
        {
            indicator.transform.position = new Vector2(ray.point.x, ray.point.y);
          // indicator.transform.TransformDirection(ray.point);
        }
       

        Vector2 direction = player.transform.position - transform.position;
       
    }
    void OnDrawGizmosSelected()
    {
       
        Gizmos.color = Color.red;
      //  Vector2 direction = player.transform.position - transform.position;
      //  Gizmos.DrawRay(transform.position, direction);
    }
}
