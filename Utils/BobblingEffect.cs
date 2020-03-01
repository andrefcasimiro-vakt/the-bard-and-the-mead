using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Utils {
    public class BobblingEffect : MonoBehaviour
    {
        Vector3 floatY;
        Vector3 originalY;

        public float floatStrengthY = 0.03f;
        public float floatStrengthX = 0.01f;

        void Start ()
        {
            this.originalY = this.transform.position;
        }

        void Update () {
            floatY = transform.position;
            floatY.y = originalY.y + (Mathf.Sin(Time.time) * floatStrengthY);
            floatY.x = originalY.x + (Mathf.Cos(Time.time) * floatStrengthX);
            transform.position = floatY;
        }
    }
}