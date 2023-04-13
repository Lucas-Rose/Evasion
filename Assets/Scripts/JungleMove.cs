using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JungleMove : MonoBehaviour
{

    private Vector3 startPos;
    public GameObject moveLevel;
    private float length;
    private float speed;
    



    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        moveLevel = GameObject.FindWithTag("Level");

        //called here to minimise number of times called.
        assign();
        

    }

    void assign()
    {
        MoveLevel move = moveLevel.GetComponent<MoveLevel>();
        length = move.length;
        //Should not be noticeable without significant speed ramp.
        speed = move.speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.back * Time.deltaTime * speed);

        //Off screen (Same pop out point as movelevel reset)
        if(transform.position.z < startPos.z - length*4)
        {
            Destroy(gameObject);
        }
    }
}
