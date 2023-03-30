using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandColour : MonoBehaviour
{
    public GameObject Object;
    
    [Header("Materials")]
    public Material Material5;
    public Material Material4;
    public Material Material3;
    public Material Material2;
    public Material Material1;

    public void healthLost(float currentHealth)
    {
        switch(currentHealth)
        {
            case float i when i > 4:
                Object.GetComponent<MeshRenderer>().material = Material5;
                Debug.Log('5');
                break;
            case float i when i > 3 && i <=4:
                Object.GetComponent<MeshRenderer>().material = Material4;
                Debug.Log('4');
                break;
            case float i when i > 2 && i <=3:
                Object.GetComponent<MeshRenderer>().material = Material3;
                Debug.Log('3');
                break;
            case float i when i > 1 && i <=2:
                Object.GetComponent<MeshRenderer>().material = Material2;
                Debug.Log('2');
                break;
            case float i when i > 0 && i <=1:
                Object.GetComponent<MeshRenderer>().material = Material1;
                Debug.Log('1');
                break;
        }
    }
}
