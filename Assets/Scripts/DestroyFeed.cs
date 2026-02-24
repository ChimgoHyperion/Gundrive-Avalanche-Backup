using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFeed : MonoBehaviour
{
    public float DestroyTime = 5f;

    private void OnEnable()
    {
        Destroy(gameObject, DestroyTime);
    }
}
