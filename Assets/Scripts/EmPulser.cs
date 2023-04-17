using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmPulser : MonoBehaviour
{
    private Material myMat;
    [Header("Colours")]
    [SerializeField] private Color glowColour;
    private float hue;
    private float saturation;
    private float value;

    [Header("Pulse")]
    [SerializeField] private float pulseFrequency;
    [SerializeField] private float pulseIntensity;
    [SerializeField] private float dimMultiplier;
    private float pulseTime;

    [Header("Wall Pulse")]
    [SerializeField] private GameObject wallPulse;
    [SerializeField] private float pulseSpeed;
    [SerializeField] private float wallPulseFrequency;
    private float startingPoint;
    private float wallPulseTime;


    // Update is called once per frame
    void Start()
    {
        if(wallPulse != null)
        {
            wallPulseTime = wallPulseFrequency;
            startingPoint = wallPulse.transform.position.z;
        }
        if(GetComponent<Renderer>() != null)
        {
            myMat = GetComponent<Renderer>().material;
        }
        
        if (myMat != null)
        {
            pulseTime = pulseFrequency;
            Color.RGBToHSV(glowColour, out hue, out saturation, out value);
        }
    }
    void Update()
    {
        if(myMat != null)
        {
            value -= Time.deltaTime * dimMultiplier;
            pulseTime -= Time.deltaTime;

            if (pulseTime <= 0)
            {
                ReLight();
                pulseTime = pulseFrequency;
            }
            glowColour = Color.HSVToRGB(hue, saturation, value);
            myMat.SetColor("_EmissionColor", glowColour);
        }

        if(wallPulse != null)
        {
            wallPulseTime -= Time.deltaTime;
            if (wallPulseTime <= 0)
            {
                RePulse();
                wallPulseTime = wallPulseFrequency;
            }
            wallPulse.transform.Translate(Vector3.back * pulseSpeed * Time.deltaTime);
        }
        

        
        
        
    }
    public void RePulse()
    {
        wallPulse.transform.position = new Vector3(0, 5, startingPoint);
    }

    public void ReLight()
    {
        value = pulseIntensity;
    }
}
