using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnerScript : MonoBehaviour
{
    public GameObject Boss;
    public Transform spawnPosition;
    public GameObject EnemySpawner;
    public GameObject PlatformsContainer;
    public bool canSpawn;
    public int SpawnTime;
    float initialcountDown = 50;
    public GameObject[] TextEffects;
    public int rand;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(wait());
    }
    public void spawnboss()
    {
        Instantiate(Boss, spawnPosition.position, Quaternion.identity);
      //  EnemySpawner.SetActive(false);
      //  PlatformsContainer.SetActive(false);
        // set platform sconttainer to false
        // ser enemyy spawner to false
        // canspawn == false
    }
    public void startWaitTime()
    {
        StartCoroutine(TimeBeforeNextSpawn(SpawnTime));
    }

    IEnumerator TimeBeforeNextSpawn(int waitTime)
    {
     //   EnemySpawner.SetActive(true);
      //  PlatformsContainer.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        spawnboss(); 
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(initialcountDown);
        spawnboss();
    }
    public void ShowEffects()
    {
        rand = Random.Range(0, 4);
        switch (rand)
        {
            case 0:
                TextEffects[0].SetActive(true);
                TextEffects[0].GetComponent<DisableText>().SelectColor();

                /*   StartCoroutine(ActivateandDeactivate());
                   TextEffects[0].SetActive(false); */
                break;
            case 1:
                TextEffects[1].SetActive(true);
                TextEffects[1].GetComponent<DisableText>().SelectColor();
                break;
            case 2:
                Debug.Log("Nothing");
                break;
            case 3:
                TextEffects[3].SetActive(true);
                TextEffects[3].GetComponent<DisableText>().SelectColor();
                break;
            case 4:

                TextEffects[2].SetActive(true);
                TextEffects[2].GetComponent<DisableText>().SelectColor();
                break;


        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
