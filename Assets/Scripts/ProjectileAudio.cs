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
    void Awake()
    {
        PlayAudio();
    }

    public void PlayAudio()
    {
        Debug.Log("Called to play.");
        source.Play();

        StartCoroutine(InevitableDeath());
    }

    public IEnumerator InevitableDeath()
    {
        //This is here to prevent lagging out by having a million on screen at once.
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
