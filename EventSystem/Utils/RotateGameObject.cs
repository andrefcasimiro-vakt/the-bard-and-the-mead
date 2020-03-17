using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.EventSystem { 
    public class RotateGameObject : MonoBehaviour
    {

        public void RotateX(float value)
        {
            transform.Rotate(value, transform.rotation.y, transform.rotation.z);
        }

        public void RotateY(float value)
        {
            transform.Rotate(transform.rotation.x, value, transform.rotation.z);
        }

        public void RotateZ(float value)
        {
            transform.Rotate(transform.rotation.x, transform.rotation.y, value);
        }

    }
}
