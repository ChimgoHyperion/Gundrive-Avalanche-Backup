using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;

public class ConsSpawnerMulti : MonoBehaviour
{

    public Transform[] spawnpoints;
    public NetworkObject[] powerupBoxes;
    public NetworkObject bulletBox;
    public int powerupsSpawned;
    public bool canSpawn = true;

    // public float minTimeBtwSpawns;
    //public float maxTimeBtwSpawns;
    public float timebtwSpawns;
    public float maxBoxInRoom;
    public int NetworkedIndex { get; set; } 
    public int NetworkedBoxIndex { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        Invoke("BoxSpawnProb", 0.5f);

    }

    // Update is called once per frame

    void BoxSpawnProb()
    {

        NetworkedIndex = Random.Range(0, spawnpoints.Length);
        Transform currentPoint = spawnpoints[NetworkedIndex];
        NetworkedBoxIndex = Random.Range(0, powerupBoxes.Length);


        if (canSpawn)
        {
            if (powerupsSpawned <= maxBoxInRoom)
            {
                FusionNetworkManager.runnerInstance.Spawn(bulletBox,transform.position, Quaternion.identity);
                powerupsSpawned++;
            }
        }
        if (powerupsSpawned < maxBoxInRoom)
        {
            canSpawn = true;
            Invoke("BoxSpawnProb", timebtwSpawns);
        }
        if (powerupsSpawned >= maxBoxInRoom)
        {
            canSpawn = false;
            powerupsSpawned = 0;
            StartCoroutine(SpawnMore());
        }


    }
    IEnumerator SpawnMore()
    {
        yield return new WaitForSeconds(100f);
        canSpawn = true;
        Invoke("BoxSpawnProb", 0.5f);


    }

    void Update()
    {


    }

}
