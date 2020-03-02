using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Core;

namespace RPG.AIV2 {

    public class Movement : Component {
        // Private
        Health health => GetComponent<Health>();
        NavMeshAgent navMeshAgent => GetComponent<NavMeshAgent>();

        public override IEnumerator Dispatch() {
            navMeshAgent.enabled = !health.IsDead();

            UpdateAnimator();

            yield return null;
        }

        void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("InputVertical", speed);    
        }
        
        // Save System
        public object CaptureState()
        {
            return new SerializableVector3(this.transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            transform.position = position.ToVector();
        }

        public void OnCleanState() {}
    }
}
