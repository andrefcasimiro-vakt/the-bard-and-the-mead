using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.EventSystem { 
    public class FaceTarget : Template
    {
        [Header("---")]
        [Header("Rotates a gameobject towards a given rotation")]

        public Transform target;
        public Transform destination;


        public override IEnumerator Dispatch()
        {
            yield return StartCoroutine(RotateTowards());
        }

        public IEnumerator RotateTowards() {

            Vector3 targetPosition = new Vector3(destination.position.x,
                                                   target.transform.position.y,
                                                   destination.position.z);
            
            target.LookAt(targetPosition);

            yield return null;

        }
    }
}
