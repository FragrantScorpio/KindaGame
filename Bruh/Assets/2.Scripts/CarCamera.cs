using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCamera : MonoBehaviour
{
    public float fieldOfView = 68f;
    public GameObject CarCam, Car;
    public FPCamera FPScript;
    public Camera Camera;
    private CarContoller CC;
    public int DriveCamMode;
    public float heightFP = 0f, distanceFP = 0f, l = 0f;

    private float bandEffect;
    private Vector3 newPos;
    private Vector2[] cameraPos;
    private Transform target;
    public GameObject focusPoint;

    public float temp = 10;
    private float bias;
    private float smoothTime = .5f;
    private float smoothTimemin = .5f, max = .9f;

    // Start is called before the first frame update
    void Start()
    {

        cameraPos = new Vector2[3];
        cameraPos[0] = new Vector2(8f, 0f);
        cameraPos[1] = new Vector2(10f, 0.5f);
        cameraPos[2] = new Vector2(14f, 1f);



        Car = GameObject.FindGameObjectWithTag("PlayerCarera");
        CarCam = GameObject.FindGameObjectWithTag("PlayerCam");
        Camera = CarCam.GetComponent<Camera>();
        FPScript = CarCam.GetComponent<FPCamera>();


        target = focusPoint.transform;

        CC = Car.GetComponent<CarContoller>();
        
        GetComponent<Camera>().usePhysicalProperties = true;
        GetComponent<Camera>().fieldOfView = fieldOfView;

    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            DriveCamMode = DriveCamMode + 1;
            if (DriveCamMode == 3) { DriveCamMode = 0; };
            Debug.Log(DriveCamMode);
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            focusPoint.transform.Rotate(0,180,0);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            focusPoint.transform.Rotate(0, 180, 0);
        }
        
        


    }
    void FixedUpdate()
    {
        if (FPScript.driveMode == 1) { updateCam(); Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, fieldOfView, temp * Time.deltaTime); }
    }
    
    void updateCam()
    {
        bandEffect = (CC.speed < 400) ? 300 - (CC.speed / 400) : 200;

        Camera.fieldOfView = fieldOfView + CC.speed /10;

        newPos = target.position - (target.forward * cameraPos[DriveCamMode].x) + (target.up * cameraPos[DriveCamMode].y);
        Camera.transform.position = newPos * (1 - smoothTime) + (transform.position * smoothTime);
        Camera.transform.LookAt(target.transform);
        Camera.transform.localPosition += transform.forward * CC.speed / bandEffect;
        bias = max - CC.speed / 400;
        smoothTime = (bias >= smoothTimemin) ? bias : smoothTimemin;
    }
}

