using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class cARui : MonoBehaviour
{
    public CarContoller CC;
    public Text speed;
    private string stringSpeed;
    private const float MaxSpeedAngle = -210, ZeroSpeedAngle= 0;
    
    // Start is called before the first frame update
    void Start()
    {
        CC = GameObject.FindGameObjectWithTag("PlayerCarera").GetComponent<CarContoller>();
        speed = GameObject.Find("Speed").GetComponent<Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
        stringSpeed = Mathf.Round(CC.speed - 1).ToString();
        speed.text = stringSpeed;
        transform.Find("Needle").eulerAngles = new Vector3(0, 0, GetRPMRotation());

    }
    private float GetRPMRotation()
    {
        float TotalAngleSize = ZeroSpeedAngle - MaxSpeedAngle;
        float RPMNormalize = CC.engineRpm / CC.maxRpm;
        return ZeroSpeedAngle - RPMNormalize * TotalAngleSize;
    }
}
