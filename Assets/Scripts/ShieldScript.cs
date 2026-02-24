using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        time = 10;
    }
  
    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = Time.deltaTime;
            gameObject.SetActive(false);
        }
    }
    public void StartCountDown()
    {
        time = 10;
    }
}
