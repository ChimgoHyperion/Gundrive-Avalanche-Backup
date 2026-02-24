using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableText : MonoBehaviour
{
    public Color[] colors;
    public SpriteRenderer myRend;
    public float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ActivateandDeactivate());
        

        SelectColor();
    }
    public void SelectColor()
    {
        int rand = Random.Range(0, 3);
        switch (rand)
        {
            case 0:
                myRend.color = colors[0];
                 break;
            case 1:
                myRend.color = colors[1];
                break;
            case 2:
                myRend.color = colors[2];
                break;
            case 3:
                myRend.color = colors[3];
                break;

        }
    }
    // Update is called once per frame
    void Update()
    {
        StartCoroutine(ActivateandDeactivate());
       
    }
    IEnumerator ActivateandDeactivate()
    {
        yield return new WaitForSeconds(waitTime);
        this.gameObject.SetActive(false);
    }

}
