using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateGamobjectInSeconds : MonoBehaviour
{
    [SerializeField] float timespan;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Invoke("Deactivate", timespan);
    }
    void Deactivate()
    {
        this.gameObject.SetActive(false);
    }

}
