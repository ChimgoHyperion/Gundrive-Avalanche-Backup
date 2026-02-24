using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    public static ObjectPool instance;
   


    private List<GameObject> pooledFireBalls= new List<GameObject>();
    public int amountToPoolFireBall;
    [SerializeField] private GameObject FireBall;

    private List<GameObject> pooledMiniBombs = new List<GameObject>();
    public int amountToPoolMiniBomb;
    [SerializeField] private GameObject MiniBomb;

   

    private void Awake()
    {
        if(instance== null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
        for (int i = 0; i < amountToPoolFireBall; i++)
        {
            GameObject obj = Instantiate(FireBall);
            obj.SetActive(false);
            pooledFireBalls.Add(obj);
        }
        for (int i = 0; i < amountToPoolMiniBomb; i++)
        {
            GameObject obj = Instantiate(MiniBomb);
            obj.SetActive(false);
            pooledMiniBombs.Add(obj);
        }
       
    }
  
    public GameObject GetPooledFireBall()
    {
        for (int i = 0; i < pooledFireBalls.Count; i++)
        {
            if (!pooledFireBalls[i].activeInHierarchy)
            {
                return pooledFireBalls[i];
            }
        }
        return null;
    }
    public GameObject GetPooledMiniBomb()
    {
        for (int i = 0; i < pooledMiniBombs.Count; i++)
        {
            if (!pooledMiniBombs[i].activeInHierarchy)
            {
                return pooledMiniBombs[i];
            }
        }
        return null;
    }
   
}
