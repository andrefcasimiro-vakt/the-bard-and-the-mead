using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.EventSystem { 
    public class TransformGameObject : MonoBehaviour
    {

        public GameObject target;

        public void TransformX(float value)
        {
            target.transform.localPosition = new Vector3(value, target.transform.localPosition.y, target.transform.localPosition.z);
        }

        public void TransformY(float value)
        {
            target.transform.localPosition = new Vector3(target.transform.localPosition.x, value, target.transform.localPosition.z);
        }

        public void TransformZ(float value)
        {
            target.transform.localPosition = new Vector3(target.transform.localPosition.x, target.transform.localPosition.y, value);
        }

    }
}
