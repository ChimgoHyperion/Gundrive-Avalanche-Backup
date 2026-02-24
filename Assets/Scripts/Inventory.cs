using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    
    public bool isFilled;
    public Image[] slots;

    public bool[] consumableFull;
    public Image[] consumableSlots;

    public int bombsCollected;
    public GameObject blackHole;
    public Image blackHoleButton;

    public int hyperBombsCollected;
    public GameObject Hyperbomb;
    public Image HyperBombButton;
    public Transform spawnPoint;
    private void Update()
    {
       /* if (bombsCollected <= 0)
        {
            blackHoleButton.gameObject.SetActive(false);
        }
        else
        {
            blackHoleButton.gameObject.SetActive(true);
        }

        if (hyperBombsCollected <= 0)
        {
            HyperBombButton.gameObject.SetActive(false);
        }
        else
        {
            HyperBombButton.gameObject.SetActive(true);
        }*/
    }
    public void consumeBlackHole()
    {
        bombsCollected--;
        Instantiate(blackHole, spawnPoint.position, Quaternion.identity);
    }
    public void consumeHyperBomb()
    { 
        hyperBombsCollected--;
        Instantiate(Hyperbomb,spawnPoint.position, Quaternion.identity);
    }
}
