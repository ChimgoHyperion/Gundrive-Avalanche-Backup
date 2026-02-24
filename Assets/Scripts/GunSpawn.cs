using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpawn : MonoBehaviour
{
    public GameObject Gun;
    public WeaponScript weaponScript;
    public Transform GunContainer, player;
    public bool equipped;
    public static bool slotFull;
        

   /* public void AttachGunToPlayer()
    {
        // Gun.transform.SetParent(GunContainer);
        Gun.transform.localPosition = new Vector3(16.22f, 10.56f);
        //Gun.transform.localRotation = player.localRotation;
        Gun.transform.localScale = new Vector3(9.08f, 9.08f, 0);


        equipped = true;
        slotFull = true;
        Instantiate(Gun, GunContainer.transform, false);
        Destroy(gameObject);
        
    }
    public void DropWeapon()
    {
        equipped = false;
        slotFull = false;
        Gun.transform.SetParent(null);
        Destroy(gameObject);
        weaponScript.enabled = false;
    }
    */
    // Start is called before the first frame update
    void Start()
    {
        GunContainer = GameObject.FindGameObjectWithTag("GunContainer").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
