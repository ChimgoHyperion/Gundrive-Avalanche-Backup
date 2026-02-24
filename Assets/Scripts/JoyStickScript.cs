using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoyStickScript : MonoBehaviour
{
    public Transform player;
    float speed;
    public float LerpTime;
    private bool touchStart = false;
    private Vector2 pointA;
    private Vector2 pointB;

    public Transform innerCircle;
    public Transform outerCircle;

    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pointA = outerCircle.transform.position;

            innerCircle.transform.position = pointA ;
            outerCircle.transform.position = pointA ;
            

        }
        if (Input.GetMouseButton(0))
        {
            touchStart = true;
            pointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
        }
        else
        {
            touchStart = false;
        }

    }
    private void FixedUpdate()
    {
        if(touchStart== true)
        {
            Vector2 offset = pointB -pointA;
            Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);
            Movecharacter(direction );
            rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode2D.Impulse);

            innerCircle.transform.position = new Vector2(pointA.x + direction.x, pointA.y + direction.y) ;
        }
        
    }
    void Movecharacter(Vector2 direction)
    {
        speed = Mathf.Lerp(11,15, LerpTime);
        player.Translate(direction * speed * Time.deltaTime);
    }
}
