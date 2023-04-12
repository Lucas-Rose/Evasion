using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmPulser : MonoBehaviour
{
    public float duration;
    private Material myMat;

    // Update is called once per frame
    void Start()
    {
        myMat = GetComponent<Renderer>().material;
    }
    void Update()
    {
        float phi = Time.deltaTime / duration * 2 * Mathf.PI;
        float amplitude = Mathf.Cos(phi) * 0.5f + 0.5f;
        float R = amplitude;
        float B = amplitude/8;

        myMat.SetColor("_EmissionColor", new Color(R, 0f, B));
    }
}
