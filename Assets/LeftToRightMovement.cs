using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftToRightMovement : MonoBehaviour
{
    public Transform left;
    public Transform right;


    float leftLimit;
    float rightLimit;
    public float speed;
    public int direction = 1;
    public Vector2 LeftRightmovement;


    // Start is called before the first frame update
    void Start()
    {
        leftLimit = left.transform.position.x;
        rightLimit = right.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.x < leftLimit)
        {
            direction = -1;
        }
        else if (transform.position.x > rightLimit)
        {
            direction = 1;
        }
        LeftRightmovement = Vector2.left * direction * speed * Time.deltaTime;

        transform.Translate(LeftRightmovement);


    }


}
