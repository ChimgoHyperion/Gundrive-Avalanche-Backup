using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TimerScript : MonoBehaviour ,IPointerClickHandler
{
    [SerializeField] private Image uiFill;
    [SerializeField] private TextMeshProUGUI uiText;

    public int Duration;

    public int remainingDuration;

    private bool Pause;

    public CanvasGroup ScoreBoardGroup;
    public GameObject EndGameButton;// labelled as continue when match has ended
   
    // Start is called before the first frame update
    void Start()
    {
        Being(Duration);
    }
    public void OnPointerClick(PointerEventData eventdata)
    {
        Pause = !Pause;
    }

    private void Being(int second)
    {
        remainingDuration = second;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        
        while (remainingDuration >= 0)
        {
            if (!Pause)
            {
                uiText.text = $"{ remainingDuration / 60:00}: {remainingDuration % 60:00}";
                uiFill.fillAmount = Mathf.InverseLerp(0, Duration, remainingDuration);
                remainingDuration--;
                yield return new WaitForSeconds(1f);
            }
            yield return null;
        }
        OnEnd();
    }

    private void OnEnd()
    {
        print("End");
        ScoreBoardGroup.alpha = 1;
        EndGameButton.SetActive(true);
        GameManagerMulti.Instance.EndGameSessionByTime_RPC();
     //   ScoreBoard.SetActive(true);
     //   board.DisplayWinnerByKills();
       
    }
    
    void ClearGameIfMaster()
    {
        
       
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
