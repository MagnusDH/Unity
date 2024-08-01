using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Controller : MonoBehaviour
{
    [SerializeField] Transform Camera;  //Transform body of the camera. Assign this in the inspector
    public AudioClip Shoot_Sound;       //In the unity inspector you can assign the sound effect here
    private AudioSource Audio_Source;   //A source component needed to play sounds in unity

    [SerializeField] public GameObject Bullet_Object;           //The bullet object to be spawned
    [SerializeField] public float Bullet_Speed = 40.0f;         //Speed of the bullet
    [SerializeField] public float Bullet_Despawn_Time = 10.0f;   //How many seconds the spawned bullet should exist before despawning

    //Start is called before the first frame update
    void Start()
    {
        Audio_Source = GetComponent<AudioSource>();     //Retrieve the "Audio_Source" component which allows us to control and play audio
        Audio_Source.clip = Shoot_Sound;                //Prepare the Audio_Source component to play sound effects
    }

    // Update is called once per frame
    void Update()
    {
        RotateWithCamera();     //Call function to rotate gun in the vertical axis
        Shoot();                //Call function to shoot projectiles and play sound effects
    }

    //Rotates the gun inn a vertical axis with the camera
    void RotateWithCamera()
    {
        float Camera_Vertical_Rotation = Camera.localEulerAngles.x;                     //Get the vertical pitch of the camera
        transform.localEulerAngles = new Vector3(Camera_Vertical_Rotation, 0f, 0f);     //Apply the vertical rotation to the gun
    }

    //Handles the shooting of the gun
    void Shoot()
    {
        //If left mouse button is clicked
        if(Input.GetMouseButtonDown(0)){    
            
            GameObject Bullet_Instance = Object_Pool.Instance.SpawnObject(Bullet_Object.name, transform.position, transform.rotation);   //Request a bullet object from the Object Pool
            Debug.Log("Gun controller called SpawnObject with name: " + Bullet_Object.name);
            Debug.Log("Gun controller is given Bullet_Instance: " + Bullet_Instance);
            
            if(Bullet_Instance != null){
                Rigidbody Bullet_RigidBody = Bullet_Instance.GetComponent<Rigidbody>();                              //Get the rigid body component of the spawned bullet
                Audio_Source.Play();                                                                                    //Play shooting sound effect

                if(Bullet_RigidBody != null){
                    Vector3 Camera_Forward = Camera.forward;
                    Bullet_RigidBody.velocity = Camera_Forward * Bullet_Speed;
                    StartCoroutine(DespawnAfterTime(Bullet_Instance));                                                  //Start a coroutine to despawn the projectile after a certain time
                }
            }
            else{
                Debug.LogError("In Gun_Controller the bullet_instance is NULL!!");
            }


            // GameObject Bullet_Instance = Instantiate(Bullet_Object, transform.position, transform.rotation);        //Create an instance of the bullet
            // Rigidbody Bullet_RigidBody = Bullet_Instance.GetComponent<Rigidbody>();                                 //Get the Rigidbody component of the spawned bullet

            // if(Bullet_RigidBody != null){
            //     Vector3 Camera_Forward = Camera.forward;                                                            //Set the bullets velocity based on the cameras forward direction multiplied with the bullet speed variable
            //     Bullet_RigidBody.velocity = Camera_Forward * Bullet_Speed;

            //     StartCoroutine(DespawnAfterTime(Bullet_Instance));                                                  //Start a coroutine to despawn the projectile after a certain time                  
            // }  
        }
    }
    
    //A way to iterate over a collection of items
    IEnumerator DespawnAfterTime(GameObject bullet)
    {
        yield return new WaitForSeconds(Bullet_Despawn_Time);                                                       //Waits for a specified time, using WaitForSeconds()
        Destroy(bullet);                                                                                            //After the waiting period the bullet is destroyed
    }
}
