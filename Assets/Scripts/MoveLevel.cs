using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLevel : MonoBehaviour
{
    private Vector3 startPos;
    public float length;
    public float speed;
    [SerializeField] BoxCollider floor;


    void Start()
    {
        startPos = transform.position;
        length = floor.size.z;
    }
    void Update()
    {
        transform.Translate(Vector3.back * Time.deltaTime * speed);

        //Reset position once designated point is reached
        if(transform.position.z < startPos.z - length)
        {
            transform.position = startPos;
        }
    }
}
