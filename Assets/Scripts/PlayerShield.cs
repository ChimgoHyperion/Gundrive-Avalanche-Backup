using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerShield : MonoBehaviour
{
    public float time=10;
    PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
       
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        view.RPC(nameof(CountDown), RpcTarget.All);
    }
    [PunRPC]
    void CountDown()
    {

        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = Time.deltaTime;
            gameObject.SetActive(false);
        }
    }
    public void StartCountDown()
    {
        view.RPC(nameof(StartCountdownMulti), RpcTarget.All);
    }
    [PunRPC]
    public void StartCountdownMulti()
    {
        time = 10;
    }
}
