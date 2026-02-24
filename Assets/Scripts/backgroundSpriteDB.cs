using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class backgroundSpriteDB : ScriptableObject
{
    public backgroundSprite[] backgroundSprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public backgroundSprite GetBackgroundSprite(int index)
    {
        return backgroundSprite[index];
    }
  
}
