using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.AI;

namespace RPG.AIV2 {

    public class PatrolBehaviour : Behaviour {

        [Range(0f, 10f)]
        [SerializeField] float maxSpeed = 3f;

        [Range(0, 1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;

        [SerializeField]
        PatrolPath patrolPath = null;
    
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;

        float timeSinceArrivedAtWaypoint = Mathf.Infinity;

        int currentWaypointIndex = 0;
        Vector3 originalPosition;

        AICore aiCore => GetComponent<AICore>();
        NavMeshAgent navMeshAgent => GetComponent<NavMeshAgent>();

        void Start() {
            originalPosition = transform.position;
        }

        public override IEnumerator Dispatch() {
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            Vector3 nextPosition = originalPosition;

            if (patrolPath != null)
            {
                if (AtWaypoint()) {
                    timeSinceArrivedAtWaypoint = 0f;
                    CycleWaypoint();
                }

                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime) {
                MoveTo(nextPosition);
            }

            yield return null;
        }

        public void MoveTo(Vector3 destination)
        {

            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(patrolSpeedFraction);
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }

        void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        bool AtWaypoint()
        {
            return Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointTolerance;
        }

        Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }
        
    }
}
