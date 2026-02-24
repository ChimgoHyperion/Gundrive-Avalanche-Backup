using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class UnityAuthenticationManager : MonoBehaviour
{
    public static UnityAuthenticationManager Instance;

    public string AuthID;

   public IntroPlayfabMana playfabMana;
    public bool IDGotten = false;


    [SerializeField]  private const string PlayerIdKey = "SavedPlayerId";

    public string PlayerID;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    async void Start()
    {
        PlayerID = PlayerPrefs.GetString(PlayerIdKey, "");

        await UnityServices.InitializeAsync();

       

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            if (string.IsNullOrEmpty(PlayerID))
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Signed in anonymously. PlayerID: " + AuthenticationService.Instance.PlayerId);
                AuthID = AuthenticationService.Instance.PlayerId;
                IDGotten = true;

                playfabMana.AuthID = AuthenticationService.Instance.PlayerId;


                PlayerPrefs.SetString(PlayerIdKey, AuthenticationService.Instance.PlayerId);// save with playerprefs
                PlayerPrefs.Save();
            }
            else
            {
                // Reuse stored ID
                AuthID = PlayerID;
                IDGotten = true;

                playfabMana.AuthID = PlayerID;
            }

        }
      

        if (AuthenticationService.Instance.IsSignedIn)
        {
            IDGotten = true;
            if (string.IsNullOrEmpty(PlayerID))
            {
                playfabMana.AuthID = AuthenticationService.Instance.PlayerId;
            }
            else
            {
                playfabMana.AuthID = PlayerID;
            }
                
        }
    }
}
