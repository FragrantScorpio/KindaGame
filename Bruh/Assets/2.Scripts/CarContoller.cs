using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CarContoller : MonoBehaviour
{
    [Range(0.2f, 0.8f)] public float EngineSmoothTime;
    
    public InputManager im;
    
    public CarAudio Audio;
    public GameObject[] throttleWheels, steerWheels;
    public WheelCollider[] throttleColliders, steerColliders;
    public int gearNum;
    public float changeGearSpeed;
    public float[] GStreight;
    public float engineRpm;
    public float[] Gear = { 7.9f, 7.09f, 4.44f, 2.24f, 1.44f, 1.00f};
    public float maxRpm = 3200, minRpm = 100, changeUpRPM = 2900, changeDownRPM = 200, speed, gearChangeRate;
    public float streigthCoefficient = 20000f;
    public float maxTurn = 20f, currentTurn;
    bool engineLerp;
    public Rigidbody rb;
    public GameObject CenterOfMass;
    public float brakePower, engineLerpValue;
    public GameObject[] backLights,backLightSource;
    private Vector3 FwheelPos, BwheelPos;
    private Quaternion FwheelRot, BwheelRot;
    private FPCamera FPC;
    private float firstFrame, secondFrame, acceleration;



    // Start is called before the first frame update
    void Start()
    {

        rb.centerOfMass = CenterOfMass.transform.localPosition;
        FPC = GameObject.FindGameObjectWithTag("PlayerCam").GetComponent<FPCamera>();
        im = this.gameObject.GetComponent<InputManager>();

    }
    void Update()
    {
        

        

        if (Input.GetKeyDown(KeyCode.R))
        {

            rb.gameObject.transform.rotation = new Quaternion(0, 0, 0,0);
            engineRpm = 0;
            rb.isKinematic = true;
            rb.isKinematic = false;
        }
        
        

        
    }
    private void LateUpdate()
    {
        StartCoroutine(AccelerationCheck());
    }
    IEnumerator AccelerationCheck()
    {
        firstFrame = speed;
        yield return new WaitForSeconds(1);
        secondFrame = speed;
        acceleration = secondFrame - firstFrame;

    }
    public bool isGrounded()
    {
        if (throttleColliders[0].isGrounded && throttleColliders[1].isGrounded && steerColliders[0].isGrounded && steerColliders[1].isGrounded)
            return true;
        else
            return false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        WheelAnimation();
        if (im.brake)
        {
            for (int i = 0; i < throttleWheels.Length; i++)
            {

                throttleColliders[i].brakeTorque = brakePower;
            }
            for (int b = 0; b < backLights.Length; b++) { backLights[b].GetComponent<Renderer>().materials[1].EnableKeyword("_EMISSION"); backLightSource[b].GetComponent<Light>().enabled = true; }
            
        }



        speed = 1 + (rb.velocity.magnitude) * 3.6f;
        currentTurn = maxTurn - (speed / 10);
        if (FPC.driveMode == 1 || FPC.driveMode == 2) { CarMovement(); }
        else { for (int i = 0; i < throttleWheels.Length; i++) { throttleColliders[i].brakeTorque =10000; }; engineRpm = 10; }



    }

    private void WheelAnimation()
    {
        for (int i = 0; i < throttleWheels.Length; i++)
        {
            throttleColliders[i].GetWorldPose(out BwheelPos, out BwheelRot);
            throttleWheels[i].transform.position = BwheelPos;
            throttleWheels[i].transform.rotation = BwheelRot;
        }
        for (int s = 0; s < steerWheels.Length; s++)
        {
            
            steerColliders[s].GetWorldPose(out FwheelPos, out FwheelRot);
            steerWheels[s].transform.position = FwheelPos;
            steerWheels[s].transform.rotation = FwheelRot;
        }
    }

    public void CarMovement()
    {
        if (!im.brake)
        {
            for (int d = 0; d < backLights.Length; d++)
            {
                backLights[d].GetComponent<Renderer>().materials[1].DisableKeyword("_EMISSION");
                backLightSource[d].GetComponent<Light>().enabled = false;
            }
            for (int i = 0; i < throttleWheels.Length; i++)
            {

                {

                    if (!engineLerp)
                    {
                        throttleColliders[i].motorTorque = streigthCoefficient * Time.deltaTime * im.throttle;
                        throttleColliders[i].brakeTorque = 0f;
                    }

                }


            }
        }
       

        for (int s = 0; s < steerWheels.Length; s++)
        {
            steerColliders[s].steerAngle = currentTurn * im.steer;
        }
            if (engineLerp)
        {
            streigthCoefficient = 0;
            engineRpm = Mathf.Lerp(engineRpm, engineLerpValue, 5 * Time.deltaTime);
            engineLerp = engineRpm > engineLerpValue + 100 ? true : false;

        }
        else {
            if (gearNum == 0)
            {

                if (im.throttle > 0) { gearNum++; engineRpm = 100; }
                if (im.throttle < 0)
                {
                    if (speed < changeGearSpeed && engineRpm < 1800)
                    {
                        streigthCoefficient = GStreight[0] - (speed * Gear[gearNum] * 50);
                        engineRpm += 500  * Time.deltaTime - Mathf.Abs(5 * acceleration);
                    }
                    else { streigthCoefficient = 0; }
                }
                if (im.throttle == 0) { streigthCoefficient = 10000; if (engineRpm > 0) { engineRpm -= 400 * Time.deltaTime; } }
            }
            else { streigthCoefficient = GStreight[gearNum] - (((speed * gearNum) * Gear[gearNum]) * 50); RPMChange(); AutoGearSwitch();  }
        }
        
    }

    private void AutoGearSwitch()
    {
        
        if (gearNum < 5 && engineRpm > changeUpRPM && speed >= changeGearSpeed * gearNum && isGrounded()) {
            gearNum = gearNum + 1;
            gearChangeRate = Time.time + 1f / 3f;
            setEngineLerp(engineRpm - (engineRpm > 1500 ? 1500 : 700));
        }
        if ((gearNum == 1 && engineRpm < 50))
        {
            gearNum--;
            
        }
        if ((gearNum > 1 && engineRpm < 1000) || (gearNum > 1 && speed < (changeGearSpeed - 5) * gearNum)) {
            gearNum = gearNum - 1;
            gearChangeRate = Time.time + 1f / 3f;
            
            setEngineLerp(engineRpm - (engineRpm < 1500 ? 1500 : 700));
        }
    }

    void setEngineLerp(float num)
    {
        engineLerp = true;
        engineLerpValue = num;
    }

    
    
    private void ManualGearSwitch()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            streigthCoefficient = 0;
            if (Input.GetKeyDown(KeyCode.C))
            {

                gearNum = gearNum + 1;
                gearChangeRate = Time.time + 1f / 3f;
                setEngineLerp(engineRpm - (engineRpm > 1500 ? 2000 : 700));

            }
            if (Input.GetKeyDown(KeyCode.X))
            {

                gearChangeRate = Time.time + 1f / 3f;
                gearNum--;
                setEngineLerp(engineRpm - (engineRpm > 1500 ? 1500 : 700));

            }
        }
    }
    
    private void RPMChange()
    {
        if (!engineLerp) {
            if (engineRpm < maxRpm && engineRpm > 0) 
            {
                if (im.throttle > 0 && speed <changeGearSpeed*gearNum || engineRpm < maxRpm/2) { engineRpm += 1500 * Gear[gearNum] * Mathf.Abs(acceleration) * Time.deltaTime; }
                engineRpm += 300 * Gear[gearNum] * acceleration * Time.deltaTime;
                if (im.throttle == 0 && speed < changeGearSpeed*gearNum) { engineRpm -= 500 * Time.deltaTime; }
                if (im.throttle == 1 && acceleration < 0) { engineRpm += 5000 * Time.deltaTime; }
            }
            
            if (engineRpm > maxRpm) { engineRpm = maxRpm - 300; }
           
        }
        if (engineRpm < 0) { engineRpm = 0; }

    }
}
