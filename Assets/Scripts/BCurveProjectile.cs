using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]


public class BCurveProjectile : MonoBehaviour
{
    [SerializeField] private Vector3 origin;
    [SerializeField] private Vector3 dest;
    [SerializeField] private float timeToDest = 2;
    private float curTime = 0;
    private Rigidbody rb;
    private float randomRange;

    //NOTE: Noticing a bit of issue with the frame rate dips affecting the calculation of the curve
    //It's less noticible when the if the curve is shallow or if the projectlile travels at slower speed
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        origin = transform.position;
        //rb.angularVelocity = new Vector3(100f/Random.Range(1, 100), 100f/Random.Range(1, 100), 100f/Random.Range(1, 100));
    }

    //Fixedupdate for consistency of calculation
    void FixedUpdate()
    {
        //Advance of current time
        curTime += Time.fixedDeltaTime;
        //After t = 1, stop following the curve
        if(curTime < timeToDest)
        {   
            QuadraticBazierCurve(origin, dest);
        }
    }

    public void QuadraticBazierCurve(Vector3 p0, Vector3 p2)
    {
        //t calculation
        float t = curTime/timeToDest;

        //Randomize midpoint
        Vector3 p1 = (p0 + p2)/2 + new Vector3(0, 10, 0);

        //Lerp p0 ~ p1
        Vector3 lerp1 = Vector3.Lerp(p0, p1, t);

        //Lerp p1 ~ p2
        Vector3 lerp2 = Vector3.Lerp(p1, p2, t);

        //Lerp lerp1 ~ lerp2
        Vector3 targetP = Vector3.Lerp(lerp1,lerp2, t);
        
        //Moving the object to the next point in the curve
        rb.velocity =  (targetP - transform.position)/Time.fixedDeltaTime;
    }
}
