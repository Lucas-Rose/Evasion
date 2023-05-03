using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private List<float> checkPointTimes;
    private float currentTime;
    private Animator anim;
    private float lastCheckPoint;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (checkPointTimes.Count != 0)
        {
            if (currentTime > checkPointTimes[0])
            {
                lastCheckPoint = checkPointTimes[0];
                checkPointTimes.RemoveAt(0);
                anim.SetTrigger("checkpoint");
            }
        }
    }

    public float getLastCheckpoint()
    {
        return lastCheckPoint;
    }
}
