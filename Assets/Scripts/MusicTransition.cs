using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicTransition : MonoBehaviour
{
    private static MusicTransition instance;


    private void Awake()
    {
        if(instance== null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start ()
    {

       
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
