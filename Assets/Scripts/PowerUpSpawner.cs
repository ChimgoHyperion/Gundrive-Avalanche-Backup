using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public Transform[] spawnpoints;
    public GameObject[] powerupBoxes;
    public int powerupsInRoom;
    public bool canSpawn;
    public GameObject ElectricGunBox;
    public GameObject BlackHoleGunBox;
    public GameObject DeathRayGunBox;
    public GameObject TriGunBox;
   // public float minTimeBtwSpawns;
   // public float maxTimeBtwSpawns;
    public float timebtwSpawns;


    public float maxBoxInRoom;

    // Start is called before the first frame update
    void Start()
    {
        canSpawn = true;
        Invoke("spawnBox", 0.5f);
        powerupsInRoom  = GameObject.FindGameObjectsWithTag("PowerUp").Length;
        //if (PlayerPrefs.GetInt("HasElectric") == 1)
        //{
        //    // spawn Electric Gun
        //    powerupBoxes[3] = ElectricGunBox;
        //}
        //if (PlayerPrefs.GetInt("HasBlackHole") == 1)
        //{
        //    powerupBoxes[4] = BlackHoleGunBox;
        //}
        //if (PlayerPrefs.GetInt("HasDeathRay") == 1)
        //{
        //    powerupBoxes[5] = DeathRayGunBox;
        //}
        //if (PlayerPrefs.GetInt("HasTriGun") == 1)
        //{
        //    powerupBoxes[6] = TriGunBox;
        //}
    }
    void spawnBox()
    {
        int index = Random.Range(0, spawnpoints.Length);
        Transform currentPoint = spawnpoints[index];
       // timebtwSpawns = Random.Range(minTimeBtwSpawns, minTimeBtwSpawns);
        int boxIndex = Random.Range(0, powerupBoxes.Length);
      
        if (canSpawn)
        {
            if (powerupsInRoom <=maxBoxInRoom)
            {
                Instantiate(powerupBoxes[boxIndex], currentPoint.transform.position, Quaternion.identity);
            }
          
        }
        Invoke("spawnBox", timebtwSpawns);
    }
    // Update is called once per frame
    IEnumerator SpawnMore()
    {
        if (powerupsInRoom >= 0 && powerupsInRoom < maxBoxInRoom)
        {
            yield return new WaitForSeconds(10);
            canSpawn = true;
        }
        else
        {
            canSpawn = false;
        }
       

    }
   
    void Update()
    {
        powerupsInRoom = GameObject.FindGameObjectsWithTag("PowerUp").Length;

        StartCoroutine(SpawnMore());

       
       
    }
}
