using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [Header ("CheckPoints")]
    [SerializeField] private List<float> checkPointTimes;

    [Header("CheckPoint Info")]
    private float lastCheckPointTime;
    private int lastCheckPointIndex;

    [Header("References")]
    private Animator anim;
    private GameManager gManager;

    private float currentTime;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
        gManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (checkPointTimes.Count != 0)
        {
            if (currentTime > checkPointTimes[0])
            {
                HitCheckPoint();
            }
        }
    }

    public float getLastCheckpointTime()
    {
        return lastCheckPointTime;
    }
    public int getLastCheckPointIndex()
    {
        return lastCheckPointIndex;
    }

    public void HitCheckPoint()
    {
        lastCheckPointTime = checkPointTimes[0];
        lastCheckPointIndex++;
        checkPointTimes.RemoveAt(0);
        anim.SetTrigger("checkpoint");
    }
    public void SetCurrentTime(float val)
    {
        currentTime = val;
    }

    // forbidden code
    // public void setSection6()
    // {
    //     lastCheckPointTime = 170;
    //     lastCheckPointIndex = 5;
    // }
}
