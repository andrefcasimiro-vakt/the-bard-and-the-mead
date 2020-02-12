using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Utils { 
    
    
    public class RotateAround : MonoBehaviour
    {
        public enum Axis { X, Y, Z }

        public Axis axis = Axis.X;
        public float speed = 0.1f;

        // Update is called once per frame
        void Update()
        {
            if (axis == Axis.X)
            {
                this.transform.Rotate(speed * Time.deltaTime, 0f, 0f);
            } else if (axis == Axis.Y)
            {
                this.transform.Rotate(0f, speed * Time.deltaTime, 0f);
            } else
            {
                this.transform.Rotate(0f, 0f, speed * Time.deltaTime);
            }
        }
    }
}
