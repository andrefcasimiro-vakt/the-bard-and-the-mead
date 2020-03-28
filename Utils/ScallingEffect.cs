using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Utils {
    public class ScallingEffect : MonoBehaviour
    {
        Vector3 floatY;
        Vector3 originalY;

        public float scaleStrengthY = 0.03f;
        public float scaleStrengthX = 0.01f;

        void Start ()
        {
            this.originalY = this.transform.localScale;
        }

        void Update () {
            floatY = transform.localScale;
            floatY.y = originalY.y + (Mathf.Sin(Time.time) * scaleStrengthY);
            floatY.x = originalY.x + (Mathf.Cos(Time.time) * scaleStrengthX);
            transform.localScale = floatY;
        }
    }
}