using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popups;
    public int popIndex = 0;
    public Joystick movementStick;
    public Joystick weaponStick;

    public GameObject bulletBox;
    public GameObject HealthBox;
    public GameObject enemySpawner;

    public bool hasCollectedWeapon = false;

    public bool hasCollectedHealth = false;

    public bool hasPressedhealthbtn = false;
    public bool hasPressedCambtn = false;
    // Start is called before the first frame update
    void Start()
    {

    }
  
    public void healthpressed()
    {
        hasPressedhealthbtn = true;
    }
    public void camPressed()
    {
        hasPressedCambtn = true;
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < popups.Length; i++)
        {
            if (i == popIndex)
            {
                popups[i].SetActive(true);

            }
            else { popups[i].SetActive(false); }
        }
        if (popIndex == 0)
        {
            if (Mathf.Abs(movementStick.Horizontal) > 0.5 || Mathf.Abs(movementStick.Vertical) > 0.5)
            {
                bulletBox.SetActive(true);
               
                 StartCoroutine(wait(2f));
                popIndex++;
              
              
            }
        }
         if (popIndex == 1)
         {

            if (hasCollectedWeapon == true)
            {
                
                popIndex++;
            }

         }
         if (popIndex == 2)
        {

            if (Mathf.Abs(weaponStick.Horizontal) > 0.5 || Mathf.Abs(weaponStick.Vertical) > 0.5)
            {
                HealthBox.SetActive(true);
                //
                popIndex++;
               // StartCoroutine(wait(2f));
               
            }
        }
        if (popIndex == 3)
        {
            if (hasCollectedHealth == true)
            {
                popIndex++;
               
            }
        }
        if (popIndex == 4)
        {
            if (hasPressedhealthbtn)
            {
                popIndex++;
            }
        }
        if (popIndex == 5)
        {
            if (hasPressedCambtn)
            {
                popIndex++;
            }
        }
        else if (popIndex == 6)
        {
            StartCoroutine(GotoMenu());
        }
    }
      IEnumerator GotoMenu()
      {
          yield return new WaitForSeconds(3f);
          SceneManager.LoadScene("Main Menu");
      }
    IEnumerator wait( float waittime)
    {
        yield return new WaitForSeconds(waittime);
        
    }

}
