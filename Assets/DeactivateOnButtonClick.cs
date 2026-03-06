using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeactivateOnButtonClick : MonoBehaviour
{
    [SerializeField] GameObject objectToDeactivate;

    public void TweakActiveStatewithBtn(bool state)
    {
        objectToDeactivate.SetActive(state);
    }

    public void ReloadScene()// called by ok button in error UI
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);// reloading the scene.
       
    }
}
