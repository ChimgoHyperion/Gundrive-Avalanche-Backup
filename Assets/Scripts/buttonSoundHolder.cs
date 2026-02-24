using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class buttonSoundHolder : MonoBehaviour
{

    public AudioClip ShieldClip, TeleportClip, HealthClip, BoostClip, collectionClip, coinClip, buttonClick, enemydeathClip
        , PlayerDeathClip;
    public AudioSource source;
   
    public bool isrevealed = false;
    public GameObject leaderBoard;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }
   
    public void Shield()
    {
        source.PlayOneShot(ShieldClip);
       // but has to be edited for multiplayer probably a new Script
    }
    public void Health()
    {
        source.PlayOneShot(HealthClip);
    }
    public void teleport()
    {
        source.PlayOneShot(TeleportClip);
    }
    public void Boost()
    {
        source.PlayOneShot(BoostClip) ;
    }
    public void collection()
    {
        source.PlayOneShot(collectionClip);
    }
    public void CoinSound()
    {
        source.PlayOneShot(coinClip);
    }
    public void ButtonClick()
    {
        source.PlayOneShot(buttonClick);
    }
    public void EnemyDeath()
    {
        source.PlayOneShot(enemydeathClip);
    }
    public void PlayerDeath()
    {
        source.volume = 0.2f;
        source.PlayOneShot(PlayerDeathClip);
       
    }

    public void ShowLeaderBoard()
    {
        isrevealed = !isrevealed;
        if(isrevealed== true)
        {
            leaderBoard.SetActive(true);
        }
        else
        {
            leaderBoard.SetActive(false);
        }
    }
   
    public void PlayfromSource( AudioClip clip)
    {
       
     //   this.GetComponent<PhotonView>().RPC("PlaySound", RpcTarget.AllBuffered,clip);
    }
    public void ShieldMulti()
    {
        PlayfromSource(ShieldClip);
    }
    public void HealthMulti()
    {
        PlayfromSource(HealthClip);
    }
    public void teleportMulti()
    {
        PlayfromSource(TeleportClip);
    }
    public void BoostMulti()
    {
        PlayfromSource(BoostClip);
    }
    public void collectionMulti()
    {
        PlayfromSource(collectionClip);
    }
    public void CoinSoundMulti()
    {
        PlayfromSource(coinClip);
    }
    public void ButtonClickMulti()
    {
        PlayfromSource(buttonClick);
    }
    public void EnemyDeathMulti()
    {
        PlayfromSource(enemydeathClip);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
