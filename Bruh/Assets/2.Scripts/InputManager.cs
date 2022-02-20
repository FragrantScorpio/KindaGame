using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float throttle;
    public float steer;
    public bool lg;
    public bool brake;
    public bool clutch;
    
    // Update is called once per frame
    void Update()
    {
        throttle = Input.GetAxis("Vertical");
        steer = Input.GetAxis("Horizontal");
        lg = Input.GetKeyDown(KeyCode.L);
        brake = Input.GetKey(KeyCode.Space);
        

    }
}
