using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelldoneScaling : MonoBehaviour
{
    Vector2 newScale;
    Vector2 oldScale;
    public float origscalefactor;
    bool shouldScaleUp;
    bool shouldScaleDown;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale= new Vector2(origscalefactor,origscalefactor);
        newScale = new Vector2(1.13519f, 1.970088f);
        oldScale = new Vector2(origscalefactor, origscalefactor);
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldScaleUp== true)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, newScale, 0.3f);
        }
        if (shouldScaleDown==true)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, oldScale, 0.3f);
        }
    }
    public void ScaleUp()
    {
        shouldScaleUp = true;
        shouldScaleDown = false;
    }
    public void Scaledown()
    {
        shouldScaleUp = false;
        shouldScaleDown = true;
       
    }
}
