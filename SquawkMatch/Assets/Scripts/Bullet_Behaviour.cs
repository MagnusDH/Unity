using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Behaviour : MonoBehaviour
{
    [SerializeField] public float Projectile_Destroy_Delay = 0f;
    [SerializeField] private GameObject explosionEffectPrefab;

    public AudioClip Explosion_Sound;       //In the unity inspector you can assign the sound effect here
    private AudioSource Audio_Source;       //A source component needed to play sounds in unity

    //Start is called before the first frame update
    void Start()
    {
        Audio_Source = GetComponent<AudioSource>();     //Retrieve the "Audio_Source" component which allows us to control and play audio
        Audio_Source.clip = Explosion_Sound;                //Prepare the Audio_Source component to play sound effects
    }

    void Update()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Projectile_Target"))
        {
            // Play a sound effect
            Audio_Source.Play();

            // Instantiate the explosion effect at the collision point
            Instantiate(explosionEffectPrefab, collision.contacts[0].point, Quaternion.identity);

            // Destroy the projectile
            // Destroy(gameObject);
        }
    }
}