using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleScript : MonoBehaviour
{
    public float speed;
    Rigidbody2D rb;
    public GameObject explosionEffect;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("World"))
        {
            Destroy(gameObject);
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

       // transform.Translate(Vector2.right * speed * Time.deltaTime);

    }
}
