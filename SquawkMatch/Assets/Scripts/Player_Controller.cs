using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] Transform Player_Camera;               //Reference to the main camera's transform
    [SerializeField] float Mouse_Sensitivity = 5f;          //Sensitivity of the mouse
    [SerializeField] float Player_Speed = 8.0f;             //Walking speed of the player
    [SerializeField] float Player_Jump_Force = 5.0f;        //How high the player can jump
    [SerializeField] float Ground_Check_Distance = 0.6f;    //How far from the ground the player can be to be able to jump
    [SerializeField] bool Player_Is_Grounded;               //Is set to "true" if player touches the ground 
    [SerializeField] float Camera_Pitch = 0.0f;             //The angle of the camera
    [SerializeField] bool Lock_Cursor = true;               //Lock the cursor to the midle of the screen
    [SerializeField] bool Show_Cursor = true;               //Visibility if the cursor
    
    private Rigidbody Rigid_Body;

    // Start is called before the first frame update
    void Start()
    {
        Rigid_Body = GetComponent<Rigidbody>(); //Get the RigidBody component of the object and make it available from the Rigid_Body variable

        //Lock the cursor to the center of the screen or not
        if(Lock_Cursor == true){
            Cursor.lockState = CursorLockMode.Locked;
        }
        else{
            Cursor.lockState = CursorLockMode.None;
        }

        //Show cursor on screen or not
        if(Show_Cursor == true){
            Cursor.visible = true;
        }
        else{
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CameraMovement();   //Move the camera with mouse input
        PlayerMovement();   //Move player with "wasd" keys and jump with "space"
    }

    //Camera movement
    void CameraMovement()
    {
        //Create a horizontal motion of the camera by using the mouse input
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));   //????????

        //Create a vertical motion of the mouse
        Camera_Pitch -= mouseDelta.y * Mouse_Sensitivity;                   //Vertical (inverted) movement of the camera
        Camera_Pitch = Mathf.Clamp(Camera_Pitch, -90.0f, 90.0f);            //Hinder the camera from rotating more or less than -90 to +90 degrees in the vertical direction
        Player_Camera.localEulerAngles = Vector3.right * Camera_Pitch;

        transform.Rotate(Vector3.up * mouseDelta.x * Mouse_Sensitivity);    //Apply the rotation to the camera
    }

    //Player movement
    void PlayerMovement()
    {   
        float horizontalInput = Input.GetAxis("Horizontal");    //Capture the horizontal input values from the keyboard
        float verticalInput = Input.GetAxis("Vertical");        //Capture the vertical input values from the keyboard

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * Player_Speed * Time.deltaTime; //Create a vector based on the keyboard inputs
        
        transform.Translate(movement);      //Apply the vector to the movement of the player

        Player_Is_Grounded = Physics.Raycast(transform.position, Vector3.down, Ground_Check_Distance);      //Check if the player touches the ground (true or false)
        
        //Make player jump if he touches the ground
        if(Input.GetKeyDown(KeyCode.Space) && Player_Is_Grounded){
            Rigid_Body.AddForce(Vector3.up * Player_Jump_Force, ForceMode.Impulse);
        }
    }
}
