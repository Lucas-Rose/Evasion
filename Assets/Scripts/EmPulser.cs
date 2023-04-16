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
    private float currTime;


    // Update is called once per frame
    void Start()
    {
        myMat = GetComponent<Renderer>().material;
        currTime = pulseFrequency;
        Color.RGBToHSV(glowColour, out hue, out saturation, out value);
        Debug.Log(value);
    }
    void Update()
    {
        value -= Time.deltaTime * dimMultiplier;
        currTime -= Time.deltaTime;
        if(currTime <= 0)
        {
            ReLight();
            currTime = pulseFrequency;
        }
        glowColour = Color.HSVToRGB(hue, saturation, value);
        myMat.SetColor("_EmissionColor", glowColour);
        Debug.Log(value);
    }

    public void ReLight()
    {
        value = pulseIntensity;
    }
}
