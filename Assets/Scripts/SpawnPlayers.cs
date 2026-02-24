using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;

    public Transform[] spawnPoints;
   

    public Sprite[] playerSprites;

    
   // PhotonView view;


    // in case the other method doesnt work
    public GameObject[] playerPrefabs;
    private void BlackThornSpawn()
    {
        int randomNumber = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomNumber];
        GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
        PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);
    }
    // code for btp stops here
    private void Awake()
    {
      
        // gamecanvas is set to active true for some reason
    }
    // Start is called before the first frame update
  
    void Update()
    {
    }
   
    void Start()
    {
        int randomindex = Random.Range(0, spawnPoints.Length);
     
        BlackThornSpawn();
      //  spawnTestPlayer();
      
    }
    void spawnTestPlayer()
    {
        Transform spawnPoint = spawnPoints[0];
        GameObject playerToSpawn = playerPrefabs[0];
        PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);
    } 
    public void SpawnPlayer()
    {
        int randomindex = Random.Range(0, spawnPoints.Length);


        /*  ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
        //  playerProperties.Add("skin", 0);
          Playertospawn.GetComponent<PhotonView>().Owner.SetCustomProperties(playerProperties);*/
       
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[randomindex].position, Quaternion.identity);
         
       //    Playertospawn.GetComponent<SpriteRenderer>().sprite = playerSprites[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];

      /*  GameObject childPlayer = Playertospawn.transform.Find("PLAYER").gameObject;
        childPlayer.GetComponent<SpriteRenderer>().sprite = playerSprites[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];*/
         
        
    }
    [PunRPC] 
    public void changeSkin()
    {
        playerPrefab.GetComponent<SpriteRenderer>().sprite = playerSprites[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(5f);
        SpawnPlayer();
    }
  
  
}
