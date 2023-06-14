using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissController : MonoBehaviour
{
    [Header("Audio")]
    //[SerializeField] private List<AudioSource> missSounds;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioSource audSource;

    [Header("Randomizer Values")]
    [Range(0,0.49f)][SerializeField] private float minVolumePercentage = 0.3f;
    [Range(0.5f,1f)][SerializeField] private float maxVolumePercentage = 0.7f;
    [Range(0,0.99f)][SerializeField] private float minPitchPercentage = 0.3f;
    [Range(1,2f)][SerializeField] private float maxPitchPercentage = 1;


    void Start()
    {
        audSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("projectile"))
        {
            //missSounds[Random.Range(0, missSounds.Count)].Play();

            PlayRandomClip();
        }
    }

    void Update()
    {
        //Uncommet below section for testing
        // if(Input.GetKeyDown(KeyCode.P))
        // {
        //     PlayRandomClip();
        // }
    }


    //Play sound at random pitch and volume
    private void PlayRandomClip()
    {
        audSource.clip = clips[Random.Range(0, clips.Length)];
        audSource.volume = Random.Range(minVolumePercentage, maxVolumePercentage);
        audSource.pitch = Random.Range(minPitchPercentage,maxPitchPercentage);

        audSource.PlayOneShot(audSource.clip);
    }
}
