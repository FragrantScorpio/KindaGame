using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedPart : MonoBehaviour
{
    public DestructionController DS;
    public float HP = 100;
   
    // Start is called before the first frame update
    void OnTriggerEnter(Collider collision)
    {

        print(collision.GetComponent<Collider>().name);
        if (HP > 0)
        {
            HP -= DS.CarContactSpeed;
        }
        
        print("HP Детали " + this.gameObject.name + " " + HP);
        
    }
    void Update()
    {
        if (HP < 0 || DS.CarContactSpeed > HP - 5)
        {
            DS.DestroyPart(this.gameObject);
        }
    }

}