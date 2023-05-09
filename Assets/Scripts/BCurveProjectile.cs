using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]


//

public class BCurveProjectile : MonoBehaviour
{
    private Vector3 origin;
    private Vector3 dest;
    [SerializeField] private float timeToDest = 2;
    private float curTime = 0;
    private float minMid;
    private float maxMid;
    private bool init = false;
    private Vector3 midpoint;
    private MeshRenderer render;
    private Rigidbody rb;

    //NOTE: Noticing a bit of issue with the frame rate dips affecting the calculation of the curve
    //It's less noticible when the if the curve is shallow or if the projectlile travels at slower speed
    void Awake()
    {   
        rb = GetComponent<Rigidbody>();
        render = GetComponent<MeshRenderer>();
        
        rb.angularVelocity = new Vector3(100f/Random.Range(1, 100), 100f/Random.Range(1, 100), 100f/Random.Range(1, 100));
    }

    void Start()
    {

    }

    void Update()
    {
        //Ignore, code for testing
        // if (Input.GetKeyDown(KeyCode.Mouse0) && !init)
        // {
        //     Initiate(new Vector3(0,0,10), 3);
        // }
    }

    //Fixedupdate for consistency of calculation
    void FixedUpdate()
    {
        if(init)
        {
            //Advance of current time
            curTime += Time.fixedDeltaTime;

            //After t = 1 (reaching destination), stop following the curve
            if(curTime < timeToDest && init)
            {   
                QuadraticBazierCurve(origin,midpoint,dest);
            }
        }

    }

    public void QuadraticBazierCurve(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        Debug.Log(p1);
        //t calculation
        float t = curTime/timeToDest;

        Vector3 lerp1 = Vector3.Lerp(p0, p1, t);
        Vector3 lerp2 = Vector3.Lerp(p1, p2, t);
        Vector3 targetP = Vector3.Lerp(lerp1,lerp2, t);
        
        //Moving the object to the next point in the curve
        rb.velocity =  (targetP - transform.position)/Time.fixedDeltaTime;
    }


    private Vector3 RandomXY(Vector3 point, float range)
    {
        return point = new Vector3(point.x + Random.Range(-range, range), point.y +Random.Range(-range, range), point.z);
    }

    //Randomize x,y of vector3 but in the form of a hollowed square
    //Purpose: to make sure midpoints arcs aren't too small to be noticed while still being able to random in all directions
    //Use positive numbers only
    private Vector3 RandomXYSign(Vector3 point, float min, float max)
    {
        return point = new Vector3(point.x + RandomSign() * Random.Range(min, max), point.y + RandomSign() * Random.Range(min, max), point.z);
    }


    private float RandomSign()
    {
        int randomSign = Random.Range(0,2);
        float sign = ((float)randomSign - 0.5f) *2;
        return sign;
    }

    //Method needs to be called to render the cube
    public void Initiate(Vector3 destination, float timeToDestination, float minRange, float maxRange)
    {
        //Set origin to instantiated position
        origin = transform.position;
        //Set destination
        dest = destination;
        //Set duration of travel
        timeToDest = timeToDestination;

        minMid = minRange;
        maxMid = maxRange;

        render.enabled = true;

        //Randomize midpoint
        midpoint = RandomXYSign((origin + dest)/2, minMid, maxMid);

        init = true;
    }
}
