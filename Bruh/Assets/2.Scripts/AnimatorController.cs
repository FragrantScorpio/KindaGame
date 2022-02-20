using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public Animator anim;
    public FPCamera FPScript;
    int InCar;
    // Start is called before the first frame update
    bool W,A,S,D,WA,WD,SA,SD,WS,AD;
    // Update is called once per frame
    void start()
    {
        FPScript = GameObject.FindGameObjectWithTag("PlayerCam").GetComponent<FPCamera>();
    }
    void Update()
    {
        
        InCar = FPScript.driveMode;
        
        anim.SetInteger("InCar", InCar);
        W = Input.GetKey(KeyCode.W);
        anim.SetBool("is walking", W);
        S = Input.GetKey(KeyCode.S);
        anim.SetBool("back walk", S);
        A = Input.GetKey(KeyCode.A);
        anim.SetBool("left walk", A);
        D = Input.GetKey(KeyCode.D);
        anim.SetBool("right walk", D);
        

    }
        
    
}
