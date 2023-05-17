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

    private float currentTime;
    public GameObject dispenser;
    private DispenserController dController;
    private int counter = 1;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
        dController = dispenser.GetComponent<DispenserController>();
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
        counter++;
        switch(counter)
        {
            case 2:
                dController.projectileSpeed = dController.section2Speed;
                break;
            case 3:
                dController.projectileSpeed = dController.section3Speed;
                break;
            case 4:
                dController.projectileSpeed = dController.section4Speed;
                break;
            case 5:
                dController.projectileSpeed = dController.section5Speed;
                break;
            case 6:
                dController.projectileSpeed = dController.section6Speed;
                break;
            default:
                dController.projectileSpeed = dController.section1Speed;
                break;
        }
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
