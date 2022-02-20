using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionController : MonoBehaviour
{
    
    public string dude;
    public GameObject[] BodyParts;
    public GameObject CarBody;
    public AudioClip[] MedHits;
    private AudioSource HitSound;
    public int BodypartHP;
    public List<int> primeNumbers = new List<int>();
    // Start is called be
    // fore the first frame update
    class Bodypart
    {
        public GameObject Name;   // имя
        public int Id;                     // возраст
    }
    void Start()
    {
        HitSound = CarBody.GetComponent<AudioSource>(); 
        
        CarBody = GameObject.FindGameObjectWithTag("PlayerCarera");
        
        for (int i = 0; i < BodyParts.Length; i++) { Physics.IgnoreCollision(BodyParts[i].GetComponent<BoxCollider>(), CarBody.GetComponent<BoxCollider>(), true);
            Physics.IgnoreCollision(BodyParts[i].GetComponent<BoxCollider>(), BodyParts[i+1].GetComponent<BoxCollider>(), true);    
        }
    }
    void OnCollisionEnter(Collision collision)
    { 
        Debug.Log(collision.collider.gameObject.name);
        // определение столкновения с двумя разноименными объектами
        if(CarBody.GetComponent<CarContoller>().speed > 40)
        {
            HitSound.clip = MedHits[Random.Range(0, MedHits.Length)];
            HitSound.Play();
            HitSound.volume = 200;
        }
        

        
        
        for (int i = 0; i < BodyParts.Length; i++) { if (BodyParts[i].name == collision.collider.name) { BodyParts[i].GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePosition; BodyParts[i].GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezeRotation; } }
    }
    void Update()
    {

       
    }
}
