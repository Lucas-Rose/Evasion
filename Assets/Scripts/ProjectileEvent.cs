using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEvent : ScriptableObject
{
    private float time;
    private float tracking;
    private float cannon;
    private float speed;
    public ProjectileEvent init(float time, float speed, float tracking, float cannon)
    {
        this.speed = speed;
        this.time = time;
        this.tracking = tracking;
        this.cannon = cannon;
        return this;
    }
    public float getTime()
    {
        return time;
    }
    public bool getTracking()
    {
        return tracking == 1;
    }
    public int getCannon()
    {
        return (int)cannon;
    }
    public float getSpeed()
    {
        return speed;
    }
}
