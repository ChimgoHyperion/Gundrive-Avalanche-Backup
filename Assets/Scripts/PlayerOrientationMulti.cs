using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerOrientationMulti : MonoBehaviour
{
    public Joystick joystick;
    public GameObject Object;
    Vector2 Rotation;
    private float rotation2;
    private float rotation3;
    public bool facingRight = true;
   public PhotonView view;
    public SpriteRenderer myRend;
    public SpriteRenderer ParentRend;
    
   

    // Start is called before the first frame update
    void Start()
    {
       
        
    }

    

    // Update is called once per frame
    void Update()
    {
       // UpdateGame();
        if (view.IsMine)
        {
            view.RPC(nameof(UpdateGame), RpcTarget.AllBuffered);

          
        }
      


        view = GetComponent<PhotonView>();
       
    }

    [PunRPC]
    void UpdateGame()
    {
        Rotation = new Vector2(joystick.Horizontal, joystick.Vertical);
        rotation3 = Rotation.x;

        if (facingRight)
        {
            rotation2 = Rotation.x + Rotation.y * 90;
            Object.transform.rotation = Quaternion.Euler(0, 0, rotation2);
        }
        else
        {
            rotation2 = Rotation.x + Rotation.y * -90;
            Object.transform.rotation = Quaternion.Euler(0, 180, -rotation2);
        }
        if (rotation3 < 0 && facingRight)
        {
            Flip();
            //  view.RPC("Flip", RpcTarget.AllBuffered);
        }
        else if (rotation3 > 0 && !facingRight)
        {
            Flip();
            //  view.RPC("Flip", RpcTarget.AllBuffered);
        }
    }


    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
        // check
        /* Vector3 Scaler = transform.localScale;
         Scaler.x *= -1;*/
        // transform.localScale *= -1;
    }
}
