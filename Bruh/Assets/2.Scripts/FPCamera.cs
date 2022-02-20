using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCamera : MonoBehaviour
{
    private float mouseX;
    private float mouseY;
    float mouseXcurrent;
    float mouseYcurrent;
    float smoothTime = 0.1f;
    float currentVelocityX;
    float currentVelocityY;
    public float sensitivityMouse = 200f;
    public Vector3 LocalCamPosition;
    public Camera PlayerCam;
    public GameObject Player, PlayerCamObject;
    public int driveMode;
    private GameObject DriverSit;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        DriverSit = GameObject.FindGameObjectWithTag("DriverSit");
        Player = GameObject.FindGameObjectWithTag("Player");
        
        PlayerCamObject = GameObject.FindGameObjectWithTag("PlayerCam");
        PlayerCam = PlayerCamObject.GetComponent<Camera>();
        LocalCamPosition = PlayerCamObject.transform.localPosition;
    }
    public void MouseMove()
    {
        mouseX += Input.GetAxis("Mouse X");
        mouseY += Input.GetAxis("Mouse Y");
        mouseY = Mathf.Clamp(mouseY, -90, 90);

        mouseXcurrent = Mathf.SmoothDamp(mouseX, mouseXcurrent, ref currentVelocityX, smoothTime);
        mouseYcurrent = Mathf.SmoothDamp(mouseY, mouseYcurrent, ref currentVelocityY, smoothTime);

        PlayerCam.transform.rotation = Quaternion.Euler(-mouseY, mouseXcurrent, 0f);
        Player.transform.rotation = Quaternion.Euler(0f, mouseXcurrent, 0f);
    }
    
    
    public void Interaction()
    {
        
            print("взаимодействие");
            Ray ray = new Ray(PlayerCam.transform.position, PlayerCam.transform.forward * 100);
            

            RaycastHit hit;

            if (driveMode == 0)
            {
                if (Physics.Raycast(ray, out hit, 1))
                {
                    if (hit.collider.gameObject.name == "RightDoor" || hit.collider.gameObject.name == "LeftDoor")
                    {
                        driveMode = driveMode + 1 % 2;
                        Player.transform.parent = DriverSit.transform;
                        Player.transform.position = DriverSit.transform.position;
                        
                        Player.GetComponent<CapsuleCollider>().enabled = false;
                        Player.GetComponent<CharacterController>().enabled = false;
                        Player.transform.rotation = new Quaternion(0,0,0,0);
                        PlayerCamObject.transform.parent = null;

                }
                    return;
                }
            }
            if(driveMode == 1|| driveMode == 2)
            {
                driveMode = 0;
                Player.transform.parent = null;
                Player.transform.Translate(Vector3.left * 5);
                Player.GetComponent<CapsuleCollider>().enabled = true;
                Player.GetComponent<CharacterController>().enabled = true;
                PlayerCamObject.transform.parent = Player.transform;
                PlayerCamObject.transform.localPosition = LocalCamPosition;
                
            }

        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) { Interaction(); }
        if (driveMode == 0) { MouseMove(); }
        


    }
}

