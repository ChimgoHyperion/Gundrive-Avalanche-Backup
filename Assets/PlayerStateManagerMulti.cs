using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;

public class PlayerStateManagerMulti : NetworkBehaviour
{

    public CharacterDataBase characterDB;
    public SpriteRenderer DisplayRenderer;

    public Sprite SelectedSprite;
    [Networked]
    [SerializeField] public int NetworkedselectedOption { get; set; }
    [Networked]
    [SerializeField] public string NetworkedNameSelection { get; set; }

    public TextMeshProUGUI NickNameText;

    public GameObject instantiatedScoreBoardItem,ScoreBoardContent,ScoreBoardItemPrefab,
        instantiatedWaitingRoomItem,WaitingRoomContent,WaitingRoomItemPrefab;

    public MultiplayerMoveAndShoot PlayerScript;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Initialise());
       
    }
    IEnumerator Initialise()
    {

        yield return new WaitForSeconds(1.5f);
        if (ScoreBoardContent != null)
        {
            instantiatedScoreBoardItem = Instantiate(ScoreBoardItemPrefab, Vector3.zero, Quaternion.identity, ScoreBoardContent.transform);

        }
        else
        {
            Debug.Log("Content not found");
        }

        if (WaitingRoomContent != null) { 
           instantiatedWaitingRoomItem= Instantiate(WaitingRoomItemPrefab,Vector3.zero,Quaternion.identity,WaitingRoomContent.transform);
        }
        else
        {
            Debug.Log("Waiting Content not found");
        }
    }

    private void UpdateStats(int selectedOption)
    {
        Character character = characterDB.GetCharacter(selectedOption);
        SelectedSprite= character.characterSprite;
    }
    private void SetLocalStats()
    {
        if (PlayerPrefs.HasKey("selectedSkin"))
        {
            int savedSkinSelection= PlayerPrefs.GetInt("selectedSkin");
            NetworkedselectedOption = savedSkinSelection; // This sets the Networked property, which syncs it to all clients
        }
        else
        {
            Debug.LogWarning("No nickname found in PlayerPrefs!");
        }

        if (PlayerPrefs.HasKey("PlayerNickname"))
        {
            string PlayerNickname = PlayerPrefs.GetString("PlayerNickname");
            NetworkedNameSelection = PlayerNickname; // This sets the Networked property, which syncs it to all clients


        }

    }
    // Update is called once per frame
    void Update()
    {
       
        // for skin selection

        if (Object.HasStateAuthority)
        {
            SetLocalStats();
           
        }
        UpdateStats(NetworkedselectedOption);

        DisplayRenderer.sprite = SelectedSprite;

        NickNameText.text = NetworkedNameSelection;

        // for ScoreBoard
        ScoreBoardContent = GameObject.Find("Scoreboard content");
        if (instantiatedScoreBoardItem != null)
        {
            instantiatedScoreBoardItem.GetComponent<ScoreBoardItem>().Score = PlayerScript.NetworkedScore;

            instantiatedScoreBoardItem.GetComponent<ScoreBoardItem>().playerUserName = NetworkedNameSelection;//or  moveandshoot.nickname

            instantiatedScoreBoardItem.GetComponent<ScoreBoardItem>().selectedSkinSprite = SelectedSprite;
        }

        WaitingRoomContent = GameObject.Find("Waiting room UI content");
        if (instantiatedWaitingRoomItem != null)
        {
            instantiatedWaitingRoomItem.GetComponent<ScoreBoardItem>().playerUserName = NetworkedNameSelection;//or  moveandshoot.nickname

            instantiatedWaitingRoomItem.GetComponent<ScoreBoardItem>().selectedSkinSprite = SelectedSprite;

            // ignore score
        }
    }
}
