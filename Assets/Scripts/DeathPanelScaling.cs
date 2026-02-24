using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPanelScaling : MonoBehaviour
{
    Vector2 newScale;
    // Start is called before the first frame update
    void Start()
    {
        newScale = new Vector2(0.7f, 0.7f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector2.Lerp(transform.localScale, newScale, 0.3f);
    }
}
