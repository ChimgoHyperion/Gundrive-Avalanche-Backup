using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsSpawner : MonoBehaviour
{
    public Transform[] spawnpoints;
    public GameObject[] powerupBoxes;
    public int powerupsInRoom;
    public bool canSpawn;
   
   // public float minTimeBtwSpawns;
    //public float maxTimeBtwSpawns;
    public float timebtwSpawns;
    public float maxBoxInRoom;
    // Start is called before the first frame update
    void Start()
    {
        canSpawn = true;
        Invoke("BoxSpawnProb", 0.5f);
        powerupsInRoom = GameObject.FindGameObjectsWithTag("ConsPow").Length;
    }

    // Update is called once per frame
   
    void BoxSpawnProb()
    {
      //  timebtwSpawns = Random.Range(minTimeBtwSpawns, minTimeBtwSpawns);
        int index = Random.Range(0, spawnpoints.Length);
        Transform currentPoint = spawnpoints[index];
        int boxIndex = Random.Range(0, powerupBoxes.Length);
        

        if (canSpawn)
        {
            if (powerupsInRoom <= maxBoxInRoom)
            {
                Instantiate(powerupBoxes[boxIndex], currentPoint.transform.position, Quaternion.identity);
            }
        }
       
        Invoke("BoxSpawnProb", timebtwSpawns);
    }
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
        powerupsInRoom = GameObject.FindGameObjectsWithTag("ConsPow").Length;

        StartCoroutine(SpawnMore());
       
    }
}
