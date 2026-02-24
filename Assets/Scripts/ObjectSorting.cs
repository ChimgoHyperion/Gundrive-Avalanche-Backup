using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectSorting : MonoBehaviour
{
    public List<GameObject> objectsToSort;

    GameObject[] scoreBoardItems;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        scoreBoardItems = scoreBoardItems.OrderBy(go => go.transform.position.y).ToArray();
        foreach(var go in scoreBoardItems.OrderBy(go => go.transform.position.y))
        {

        }
        System.Array.Sort(scoreBoardItems);

        foreach( ScoreBoardItem item in transform)
        {
          //  scoreBoardItems.OrderBy(item.deathcount,
          //  item.deathcount = sort
        }
    }
}
