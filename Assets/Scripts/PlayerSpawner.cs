using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public Transform[] spawnPoints;

    private void Start()
    {
        int random = Random.Range(0, spawnPoints.Length);
        Transform spawnpoint = spawnPoints[random];
        GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
        PhotonNetwork.Instantiate(playerToSpawn.name, spawnpoint.position, Quaternion.identity);
    }
}
