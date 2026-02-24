using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterDataBase : ScriptableObject
{
    public Character[] character;

    public Character GetCharacter (int index)
    {
        return character[index];
    }
    public int CharacterCount
    {
        get
        {
            return character.Length;
        }
    }
}
