using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEvent : ScriptableObject
{
        [SerializeField] private float time;
        [SerializeField] private float tracking;
        [SerializeField] private float cannon;
        public ProjectileEvent init(float time, float tracking, float cannon)
        {
            this.time = time;
            this.tracking = tracking;
            this.cannon = cannon;
        return this;
        }
        public float getTime()
        {
            return time;
        }
        public float getTracking()
        {
            return (int)tracking;
        }
        public int getCannon()
        {
            return (int)cannon;
        }
}
