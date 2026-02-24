using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    Rigidbody2D rb;
    public float jumpforce;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }
    private void FixedUpdate()
    {
        float  direction  = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            rb.velocity= Vector2.up * jumpforce;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
