using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateManager : MonoBehaviour
{
    [SerializeField] private GameObject cBallPrefab;
    [SerializeField] private GameObject player;

    [SerializeField] private GameObject cannons;
    [SerializeField] private List<GameObject> cannonObjects;
    [SerializeField] private List<CannonTrigger> cannonTriggers;
    [SerializeField] private List<float> triggerTimes = new List<float>();

    private float timePassed;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < cannons.transform.childCount; i++)
        {
            cannonObjects.Add(cannons.transform.GetChild(i).gameObject);
        }
        // Add cannon sequence here
        cannonTriggers.Add(ScriptableObject.CreateInstance<CannonTrigger>().init(3f, 50f, 3));
        cannonTriggers.Add(ScriptableObject.CreateInstance<CannonTrigger>().init(6f, 50f, 7));
        cannonTriggers.Add(ScriptableObject.CreateInstance<CannonTrigger>().init(9f, 50f, 1));
        cannonTriggers.Add(ScriptableObject.CreateInstance<CannonTrigger>().init(12f, 50f, 10));
        cannonTriggers.Add(ScriptableObject.CreateInstance<CannonTrigger>().init(15f, 50f, 13));
        cannonTriggers.Add(ScriptableObject.CreateInstance<CannonTrigger>().init(18f, 50f, 16));

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
        if (timePassed >= triggerTimes[0])
        {
            GameObject newBall = Instantiate(cBallPrefab, cannonObjects[cannonTriggers[0].getCannon()].transform);
            Rigidbody rb = newBall.AddComponent<Rigidbody>();
            Vector3 direction = (player.transform.position - newBall.transform.position).normalized;
            rb.velocity = direction * cannonTriggers[0].getSpeed();
            rb.useGravity = false;
            Debug.Log("spawned ball");
            triggerTimes.RemoveAt(0);
            cannonTriggers.RemoveAt(0);
        }
    }
}
