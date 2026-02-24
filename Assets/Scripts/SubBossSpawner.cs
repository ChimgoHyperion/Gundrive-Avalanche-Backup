using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubBossSpawner : MonoBehaviour
{
    public GameObject[] spawnPoints;
    GameObject currentPoint;
    int index;
    public GameObject[] enemies;
    public float minTimeBtwSpawns;
    public float maxTimeBtwSpawns;
    public bool canSpawn;
    public float spawnTime;
   
    public int enemiesInRoom;
   
   
    public float waitTime;
    public float startSpawnTime;
    public float timeBtwSpawns;

   

    // Start is called before the first frame update
    void Start()
    {
       // manager = FindObjectOfType<EnemyManager>();
        enemiesInRoom = GameObject.FindGameObjectsWithTag("Enemy").Length;
       
        // also account for enemy5 tag
        startSpawnTime = 10;//Random.Range(minSpawnTime, maxSpawnTime);
        Invoke("SpawnEnemy", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {

        enemiesInRoom = GameObject.FindGameObjectsWithTag("Enemy").Length;
      
        if (canSpawn)
        {
            spawnTime -= Time.deltaTime;
            if (spawnTime <= 0)
            {
              //  manager.IncreaseHealth();
             //   manager.IncreaseShootFreq();
                canSpawn = false;
                startSpawnTime++;

            }
        }
        StartCoroutine(SpawnAgain());
    }
    IEnumerator SpawnAgain()
    {
        // Random.Range(minSpawnTime, maxSpawnTime);
        if (enemiesInRoom == 0)
        {
            yield return new WaitForSeconds(waitTime);
            spawnTime = startSpawnTime;
            canSpawn = true;

        }

    }

    void SpawnEnemy()
    {
        index = Random.Range(0, spawnPoints.Length);
        currentPoint = spawnPoints[index];
        timeBtwSpawns = Random.Range(minTimeBtwSpawns, maxTimeBtwSpawns);

        if (canSpawn)
        {
            Instantiate(enemies[Random.Range(0, enemies.Length)], currentPoint.transform.position, Quaternion.identity);
            // enemiesInRoom++;
        }
        Invoke("SpawnEnemy", timeBtwSpawns);

    }
}
