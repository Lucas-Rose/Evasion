using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateManager : MonoBehaviour
{
    [SerializeField] private GameObject cBallPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private int maxCannonBalls;

    [SerializeField] private GameObject cannons;
    [SerializeField] private List<GameObject> cannonObjects;
    private List<GameObject> activeCannonBalls = new List<GameObject>();

    [SerializeField] private Vector3[] cannonEvents;
    public List<CannonTrigger> cannonTriggers;
    public List<float> triggerTimes = new List<float>();

    private float timePassed;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < cannons.transform.childCount; i++)
        {
            cannonObjects.Add(cannons.transform.GetChild(i).gameObject);
        }
        for(int i = 0; i < cannonEvents.Length; i++)
        {
            cannonTriggers.Add(ScriptableObject.CreateInstance<CannonTrigger>().init(cannonEvents[i].x, cannonEvents[i].y, cannonEvents[i].z));
        }

        for (int i = 0; i < cannonTriggers.Count; i++)
        {
            triggerTimes.Add(cannonTriggers[i].getTime());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(timePassed);
        timePassed += Time.deltaTime;
        if (triggerTimes.Count > 0)
        {
            if (timePassed >= triggerTimes[0])
            {
                ShootCannon();
            }
        }
    }

    public void ShootCannon()
    {
        GameObject newBall = Instantiate(cBallPrefab, cannonObjects[cannonTriggers[0].getCannon()].transform.position, Quaternion.identity);
        Rigidbody rb = newBall.GetComponent<Rigidbody>();
        Vector3 dir = player.transform.position - newBall.transform.position;
        rb.useGravity = false;
        rb.velocity = dir.normalized * cannonTriggers[0].getSpeed();
        Debug.Log("spawned ball");
        activeCannonBalls.Add(newBall);
        triggerTimes.RemoveAt(0);
        cannonTriggers.RemoveAt(0);

        Transform cannon = newBall.GetComponentInParent<Transform>();
        if (activeCannonBalls.Count > maxCannonBalls)
        {
            Destroy(activeCannonBalls[0]);
            activeCannonBalls.RemoveAt(0);
        }
    }
}
