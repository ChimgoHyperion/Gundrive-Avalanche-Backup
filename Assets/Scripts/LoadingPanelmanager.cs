using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanelmanager : MonoBehaviour
{
    public int hasOpened;
    public GameObject loadingPanel;
    // Start is called before the first frame update
    void Start()
    {
      
        if (PlayerPrefs.GetInt("hasOpened") == 0)
        {
            loadingPanel.SetActive(true);
           
        }

        if (PlayerPrefs.GetInt("hasOpened") == 1)
        {
            loadingPanel.SetActive(false);

        }


    }
    IEnumerator DestroyMe()
    {
        yield return new WaitForSeconds(4f);
        loadingPanel.SetActive(false);
        PlayerPrefs.SetInt("hasOpened", 1);
    }
    // Update is called once per frame
    void Update()
    {
        
        if(loadingPanel.activeSelf==true)
        {
            StartCoroutine(DestroyMe());
        }
        hasOpened = PlayerPrefs.GetInt("hasOpened");
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("hasOpened", 0);
    }
     [RuntimeInitializeOnLoadMethod]
     static void OnRuntimeMethodLoad()
     {
        PlayerPrefs.SetInt("hasOpened", 0);
         Debug.Log("After Scene is loaded and game is running");

     }


}
