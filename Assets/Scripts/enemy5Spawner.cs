using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy5Spawner : MonoBehaviour
{
    public GameObject[] spawnPoints;
    GameObject currentPoint;
    int index;
  
    public GameObject enemy5;
    public float minTimeBtwSpawns;
    public float maxTimeBtwSpawns;
    public bool canSpawn;
   
    public float spawnTime;
   
   
    public int enemy5Num;
  
    public float waitTime;
    public float startSpawnTime;
    public float timeBtwSpawns;

    public EnemyManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<EnemyManager>();
      
        enemy5Num = GameObject.FindGameObjectsWithTag("Enemy5").Length;
        
        startSpawnTime = 10;//Random.Range(minSpawnTime, maxSpawnTime);
        Invoke("SpawnEnemy5", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        enemy5Num = GameObject.FindGameObjectsWithTag("Enemy5").Length;
        if (canSpawn)
        {
            spawnTime -= Time.deltaTime;
            if (spawnTime <= 0)
            {
                manager.IncreaseHealth();
                manager.IncreaseShootFreq();
                canSpawn = false;
                startSpawnTime++;

            }
        }
        StartCoroutine(SpawnAgain());
    }
    IEnumerator SpawnAgain()
    {
        // Random.Range(minSpawnTime, maxSpawnTime);
        if (enemy5Num == 0)
        {
            yield return new WaitForSeconds(waitTime);
            spawnTime = startSpawnTime;
            canSpawn = true;

        }


    }
    void SpawnEnemy5()
    {
        index = Random.Range(0, spawnPoints.Length);
        currentPoint = spawnPoints[index];
        timeBtwSpawns = Random.Range(minTimeBtwSpawns, maxTimeBtwSpawns);

        if (canSpawn)
        {
            Instantiate(enemy5, currentPoint.transform.position, Quaternion.identity);
            // enemiesInRoom++;
        }
        Invoke("SpawnEnemy5", timeBtwSpawns);

    }
}
