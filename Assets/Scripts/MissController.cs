using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private List<AudioSource> missSounds;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("projectile"))
        {
            missSounds[Random.Range(0, missSounds.Count)].Play();
        }
    }
}
