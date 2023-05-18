using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionPlayer : MonoBehaviour
{
    [SerializeField] Transform startingPosition;
    [SerializeField] GameObject vrController;
    [SerializeField] Camera playerHead;
    public float startElevation;
    //public Transform playerHead;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Calibrate()
    {
        //var distanceDiff = startingPosition.position - playerHead.transform.position;
        //playerHead.transform.position += distanceDiff;
        //Vector3 startElevation = new Vector3(0, playerHead.transform.position.y, 0);
        startElevation = playerHead.transform.position.y;
    }
}
