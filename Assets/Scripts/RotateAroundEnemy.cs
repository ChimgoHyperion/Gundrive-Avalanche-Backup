using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundEnemy : MonoBehaviour
{
    public Transform target;
    public float speed;
    public Vector3 direction = Vector3.up;
 


    // Start is called before the first frame update
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
        /* Vector3 relativePosition = (target.position + new Vector3(0, 0, 2)) - transform.position;
         Quaternion rotation = Quaternion.LookRotation(relativePosition);

         Quaternion current = transform.localRotation;
         transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime);
         transform.Translate(0, 0, 3 * Time.deltaTime);*/
        transform.RotateAround(target.transform.position, direction, speed * Time.deltaTime);


    }
}
