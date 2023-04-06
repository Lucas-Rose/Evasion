using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAnim : MonoBehaviour
{
    public GameObject anim;
    public float xMod;
    public float yMod;
    public float zMod;
    // Start is called before the first frame update
    void Awake()
    {
        anim = Instantiate(anim, transform.position, transform.rotation);

        //PlayAnim();
    }

    // public void PlayAnim()
    // {
    //     anim.Play();
    // }
}
