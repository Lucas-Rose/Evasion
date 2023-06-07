using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallSelfDestruct : MonoBehaviour
{
    //Scuffed clean up for objects not in an array
    void Update()
    {
        if(transform.position.z < -40)
        {
            Destroy(gameObject);
        }
    }
}
