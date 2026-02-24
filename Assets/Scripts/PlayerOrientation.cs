using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOrientation : MonoBehaviour
{
    public Joystick joystick;
    public GameObject Object;
    Vector2 MainRotation;
    private float rotation2;
    private float JoystickX;
    public bool facingRight = true;

    public ControlType controlType;
    public enum ControlType
    {
        WASD, Joystick
    }

    public Image buttonImage;
    public Sprite WasdSprite, JoystickSprite;

    public TextMeshProUGUI ControlSwitchText;
    public CanvasGroup JoystickCanvasGroup;
    public GameObject WASDTutorial;
    // Start is called before the first frame update
    void Start()
    {
        // check the platform and change to the app
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            SwitchToWASD();
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            SwitchToJoystick();
        }
        else if (Application.isEditor)
        {
           SwitchToWASD();
        }

    }

    // Update is called once per frame
    void Update()
    {

        //view.RPC(nameof(UpdateGame), RpcTarget.AllBuffered);
        UpdateGame();
    }

    void UpdateGame()
    {
        // joystick controls

        if(controlType == ControlType.Joystick)
        {
            MainRotation = new Vector2(joystick.Horizontal, joystick.Vertical);
            JoystickX = MainRotation.x; // if the joystick crosses 0 on the x axis, flip

            if (facingRight)
            {
                rotation2 = MainRotation.x + MainRotation.y * 90;
                Object.transform.rotation = Quaternion.Euler(0, 0, rotation2); // flip the guncontainer
            }
            else
            {
                rotation2 = MainRotation.x + MainRotation.y * -90;
                Object.transform.rotation = Quaternion.Euler(0, 180, -rotation2); // flip the guncontainer
            }


            if (JoystickX < 0 && facingRight)
            {
                Flip();// if the joystick crosses 0 on the x axis, flip the parent

            }
            else if (JoystickX > 0 && !facingRight)
            {
                Flip();// if the joystick crosses 0 on the x axis, flip the parent

            }
        }

       


        // for mouse and laptop normal play

        if(controlType == ControlType.WASD)
        {
            ///  1.Get the mouse position in world space
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f; // Optional: force Z to 0 in 2D

            // 2. Calculate direction from object to mouse
            Vector3 direction = mousePos - transform.position;

            // 3. Get the angle in degrees
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // 4. Rotate the object on the Z-axis
            Object.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));



            if (mousePos.x < transform.position.x && facingRight)
            {
                Flip();

            }
            else if (mousePos.x > transform.position.x && !facingRight)
            {
                Flip();

            }

        }
       
    }
    public void SwitchControls()
    {
        if (controlType == ControlType.Joystick)
        {
           SwitchToWASD();
        }
        else if (controlType == ControlType.WASD)
        {
           SwitchToJoystick();
        }
    }

    public void SwitchToWASD()
    {
        controlType = ControlType.WASD;
        buttonImage.sprite = JoystickSprite;

        ControlSwitchText.text = "Switch to Joystick(for Mobile)";

        WASDTutorial.SetActive(true);
        JoystickCanvasGroup.alpha = 0;
        JoystickCanvasGroup.interactable = false;
        JoystickCanvasGroup.blocksRaycasts = false;
    }
    public void SwitchToJoystick()
    {
        controlType = ControlType.Joystick;
        buttonImage.sprite = WasdSprite;

        ControlSwitchText.text = "Switch to WASD(for PC)";

        WASDTutorial.SetActive(false);
        JoystickCanvasGroup.alpha = 1;
        JoystickCanvasGroup.interactable = true;
        JoystickCanvasGroup.blocksRaycasts = true;
    }
    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0); //  flip the parent object
      
    }
}
