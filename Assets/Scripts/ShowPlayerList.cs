using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPlayerList : MonoBehaviour
{
    public GameObject PlayerListCanvas;
    public bool isrevealing = false;

    public void RevealPlayer()
    {
        isrevealing = !isrevealing;
        if(isrevealing== true)
        {
            PlayerListCanvas.SetActive(true);

        } else if ( isrevealing== false)
        {
            PlayerListCanvas.SetActive(false);
        }
    }
}
