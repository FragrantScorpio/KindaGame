using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DestructionController : MonoBehaviour
{
    public GameObject CarBody;
    public GameObject[] BodyParts; 
    public AudioClip[] MedHits, MinHits, MaxHits;
    private AudioSource HitSound;
    private float CarDamage;
    public float CarContactSpeed,CarHp = 1000;
    public AudioSource BlowUpSound;

    // Start is called be
    // fore the first frame update
    
    
    void Start()
    {
        HitSound = CarBody.GetComponent<AudioSource>();
        CarBody = GameObject.FindGameObjectWithTag("PlayerCarera");
        BodyParts = GameObject.FindGameObjectsWithTag("BodyParts");
    }
    void Update()
    {
        if (CarHp < 0)
        {
            print("Машина уничтожена");
            BlowUpSound.Play();
            CarBody.GetComponent<InputManager>().brake = true;
            CarBody.GetComponent<InputManager>().throttle = 0;
            CarBody.GetComponent<InputManager>().enabled = false;
            for (int i = 0; i < BodyParts.Length;)
            {

                DestroyPart(BodyParts[i]);
                i++;
            }
            
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        CarContactSpeed = collision.relativeVelocity.magnitude;
        
       
        // определение столкновения с двумя разноименными объектами
        CarDamage = CarContactSpeed < 15 ? 1 : CarContactSpeed > 15 && CarContactSpeed < 25 ? 2 : 3;
        switch (CarDamage)
        {
            case 1:
                SoundManager.PlaySound(MinHits[Random.Range(0, MinHits.Length)].name);
                
                break;
            case 2:
                SoundManager.PlaySound(MedHits[Random.Range(0, MedHits.Length)].name);
                break;
            case 3:
                SoundManager.PlaySound(MaxHits[Random.Range(0, MaxHits.Length)].name);
                break;
            
        }


        CarHp -= CarContactSpeed;
        
        
    }

    public void DestroyPart(GameObject Part)
    {
        print("Деталь сломана");
        Part.GetComponent<Rigidbody>().isKinematic = false;
        Part.transform.parent = null;
        Part.layer = default;
        Part.GetComponent<BoxCollider>().isTrigger = false;
    }
    
}
