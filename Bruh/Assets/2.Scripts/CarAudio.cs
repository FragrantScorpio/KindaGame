using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class CarAudio : MonoBehaviour
{
    

    public FPCamera FPScript;
    public GameObject ASource;
    public AudioSource changeGearAudio;
    public float minPitch = 0.05f;
    public CarContoller CC;
    private float CarPitch;
    [Header("Clips")]
    public AudioClip lowAccelClip;
    public AudioClip lowDecelClip;
    public AudioClip HighDecelClip;
    public AudioClip HighAccelClip;
    public AudioClip MaxRPMclip;
    public AudioClip changeGearClip;
    private AudioSource m_LowAccel;
    private AudioSource m_LowDecel;
    private AudioSource m_HighAccel;
    private AudioSource m_HighDecel;
    public AudioSource m_MaxRPM, RadioSource;
    public InputManager im;
    private int r = 0;
    private bool RadioSwitch = false;
    private float accFade = 0;
    private float acceleration, RadioStartTime;
    [Header("Music")]
    public Object[] RadioClips;
    [Header("Pitch")]
    [Range(0.5f, 1)] public float Pitch = 1f;
    [Range(.5f, 3)] public float lowPitchMin = 1f;
    [Range(2, 7)] public float lowPitchMax = 6f;
    [Range(0, 1)] public float highPitchMultiplier = 0.25f;
    [Range(0, 1)] public float pitchMultiplier = 1f;

    // Start is called before the first frame update
    public static void Shuffle(Object[] arr)
    {
        // создаем экземпляр класса Random для генерирования случайных чисел
        System.Random rand = new System.Random();

        for (int i = arr.Length - 1; i >= 1; i--)
        {
            int j = rand.Next(i + 1);

            Object tmp = arr[j];
            arr[j] = arr[i];
            arr[i] = tmp;
        }
    }
    
    void Start()
    {
        RadioClips = Resources.LoadAll("Music", typeof(AudioClip));
        Shuffle(RadioClips);
        RadioSource = SetUpRadio(RadioClips[r] as AudioClip);

        CC = GameObject.FindGameObjectWithTag("PlayerCarera").GetComponent<CarContoller>();
        FPScript = GameObject.FindGameObjectWithTag("PlayerCam").GetComponent<FPCamera>();
        m_LowAccel = SetUpEngineAudioSource(lowAccelClip);
        m_LowDecel = SetUpEngineAudioSource(lowDecelClip);
        m_HighAccel = SetUpEngineAudioSource(HighAccelClip);
        m_HighDecel = SetUpEngineAudioSource(HighDecelClip);
        m_MaxRPM = SetUpEngineAudioSource(MaxRPMclip);
        changeGearAudio = SetUpEngineAudioSource(changeGearClip);
        changeGearAudio.loop = false;
        im = this.gameObject.GetComponent<InputManager>();
       




    }
    private void Update()
    {
        this.gameObject.transform.position = ASource.transform.position;
        if (Input.GetKeyDown(KeyCode.B))
        {
            RadioSwitch = !RadioSwitch;
            switch (RadioSwitch)
            {
                case false:
                    RadioSource.Stop();
                    break;
                case true:
                    RadioSource.Play();
                    RadioStartTime = Time.time;
                    print("Current playing " + RadioSource.clip.length);
                    
                    break;
            }
        }
        if ((Time.time - RadioStartTime) == RadioSource.clip.length || Input.GetKeyDown(KeyCode.K)) 
        { r++;
            print("Track over... changing clip");
            RadioSource.Stop();
            RadioSource.clip = RadioClips[r] as AudioClip; 
           
            RadioSource.Play();
            if (r == RadioClips.Length) { r = 0; }
            RadioStartTime = Time.time;
        };
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (FPScript.driveMode > 0) {
            if (im.throttle > 0)
                acceleration = 1;
            else acceleration = 0;
        }
        else acceleration = 0;
            accFade = Mathf.Lerp(accFade, Mathf.Abs(acceleration), 15 * Time.deltaTime);
            CarPitch = (CC.engineRpm + 100) / 3200;
           
            

            float pitch = ULerp(lowPitchMin, lowPitchMax, CC.engineRpm / CC.maxRpm);
            pitch = Mathf.Min(lowPitchMax, pitch);
            m_LowAccel.pitch = pitch * pitchMultiplier;
            m_LowDecel.pitch = pitch * pitchMultiplier;
            m_HighAccel.pitch = pitch * highPitchMultiplier * pitchMultiplier;
            m_HighDecel.pitch = pitch * highPitchMultiplier * pitchMultiplier;

            float decFade = 1 - accFade;
            float highFade = Mathf.InverseLerp(0.2f, 0.8f, CC.engineRpm / CC.maxRpm);
            float lowFade = 1 - highFade;
            highFade = 1 - ((1 - highFade) * (1 - highFade));
            lowFade = 1 - ((1 - lowFade) * (1 - lowFade));
            //accFade = 1 - ((1 - accFade)*(1 - accFade));
            decFade = 1 - ((1 - decFade) * (1 - decFade));

            m_LowAccel.volume = (lowFade * accFade) ;
            m_LowDecel.volume = (lowFade * decFade) ;
            m_HighAccel.volume = (highFade * accFade) ;
            m_HighDecel.volume = (highFade * decFade) ;
            m_MaxRPM.volume = 0;
        
        
        
        
       
        

    }
    private AudioSource SetUpEngineAudioSource(AudioClip clip)
    {
        AudioSource source = this.gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = 0;
        source.spatialBlend = 1;
        source.loop = true;
        source.dopplerLevel = 0;
        source.time = Random.Range(0f, clip.length);
        source.Play();
        source.minDistance = 1;
        source.maxDistance = 20;
        return source;
    }
   
    private AudioSource SetUpRadio(AudioClip clip)
    {
        AudioSource source = this.gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.priority = 0;
        source.volume = 1f;
        source.dopplerLevel = 0;
       
        source.minDistance = 5;
        source.maxDistance = 500;
        return source;
    }
    private float ULerp(float from, float to, float value)
    {
        return (1.0f - value) * from + value * to;
    }
}
