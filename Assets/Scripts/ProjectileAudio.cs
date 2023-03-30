using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAudio : MonoBehaviour
{
    public AudioSource source;

    // Start is called before the first frame update

    // can be randomised down the line if we have more possible SFX
    // void Start()
    // {
    //     //Debug.Log("Setting");
    //     source = GetComponent<AudioSource>(); 
    // }

    public void PlayAudio()
    {
        Debug.Log("Called to play.");
        source.Play();
    }
}
