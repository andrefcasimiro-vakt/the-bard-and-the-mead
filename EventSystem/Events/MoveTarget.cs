using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.EventSystem { 
    public class MoveTarget : Template
    {
        [Header("---")]
        [Header("Moves a gameobject towards a given position")]

        public Transform target;
        public Transform destination;

        public float stoppingDistance = 0.1f;

        public float moveSpeed = 0.5f;

        public override IEnumerator Dispatch()
        {
            yield return StartCoroutine(MoveUntilReachDestination());
        }

        public IEnumerator MoveUntilReachDestination() {

            bool hasNavMeshAgent = target.GetComponent<NavMeshAgent>() != null;

            if (hasNavMeshAgent) {
                while (Vector3.Distance(destination.position, target.position) > stoppingDistance) {
                    HandleAgentNavigation(true);

                    yield return null;
                }
            } else {
                while (Vector3.Distance(destination.position, target.position) > stoppingDistance) {
                    target.position = Vector3.MoveTowards(
                        target.position,
                        destination.position,
                        Time.deltaTime * moveSpeed
                    );

                    yield return null;
                }
            }

            if (hasNavMeshAgent) {
                HandleAgentNavigation(false);
            }

        }

        public void HandleAgentNavigation(bool isMoving) {
            NavMeshAgent navMeshAgent = target.GetComponent<NavMeshAgent>();

            if (navMeshAgent == null)
                return;

            if (isMoving) {
                navMeshAgent.destination = destination.position;
                navMeshAgent.speed = moveSpeed;
                navMeshAgent.isStopped = false;
            } else {
                navMeshAgent.speed = 0;
                navMeshAgent.velocity = Vector3.zero;
                navMeshAgent.isStopped = true;
            }
        }

    }
}
