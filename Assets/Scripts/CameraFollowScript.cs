using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    // Update is called once per frame
    private void Start()
    {
        transform.position = transform.position;
    }

    void Update()
    {
        transform.position = player.position + offset;
    }
}
