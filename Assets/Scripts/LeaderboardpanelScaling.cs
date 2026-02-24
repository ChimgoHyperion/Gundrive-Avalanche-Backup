using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardpanelScaling : MonoBehaviour
{
    Vector2 newScale;
    // Start is called before the first frame update
    void Start()
    {
        newScale = new Vector2(8.8f, 6.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector2.Lerp(transform.localScale, newScale, 0.3f);
    }
}
