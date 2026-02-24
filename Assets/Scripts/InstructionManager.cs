using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionManager : MonoBehaviour
{
    public Text  takeUpArms,killTheDroids;
    public float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Display());
    }
    public IEnumerator Display()
    {
        yield return new WaitForSeconds(waitTime);
        takeUpArms.gameObject.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        takeUpArms.gameObject.SetActive(false);
        yield return new WaitForSeconds(waitTime);
        killTheDroids.gameObject.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        killTheDroids.gameObject.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
