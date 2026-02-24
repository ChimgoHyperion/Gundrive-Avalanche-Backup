using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateOnAxis : MonoBehaviour
{
    private GameObject ring;
    public Vector3 rotationAngle;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        ring = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        ring.transform.Rotate(rotationAngle * speed * Time.deltaTime, Space.Self);
    }
}
