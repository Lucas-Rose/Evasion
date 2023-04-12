using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTrigger : ScriptableObject
{
        [SerializeField] private float time;
        [SerializeField] private float speed;
        [SerializeField] private int cannon;
        public CannonTrigger init(float time, float speed, int cannon)
        {
            this.time = time;
            this.speed = speed;
            this.cannon = cannon;
        return this;
        }
        public float getTime()
        {
            return time;
        }
        public float getSpeed()
        {
            return speed;
        }
        public int getCannon()
        {
            return cannon;
        }
}
