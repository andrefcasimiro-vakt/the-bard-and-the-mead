using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Core;

namespace RPG.AIV2 {

    public class Vision : Component {
        
        [SerializeField]
        float fieldOfView = 110f;

        [SerializeField]
        float minimumDistanceToCastVision = 10f;

        [SerializeField]
        Vector3 playerLastKnownPosition;

        [SerializeField]
        UnityEvent OnPlayerSighted;

        // Private
        CapsuleCollider col => GetComponent<CapsuleCollider>();
        GameObject player;
        AICore aICore => GetComponent<AICore>();

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
        }

        public override IEnumerator Dispatch() {
            if (player.GetComponent<Health>().IsDead()) {
                yield return null;
            }

            bool shouldIgnore =
                aICore.GetState() == AIState.ARRIVE_AT_TARGET
                || aICore.GetState() == AIState.FIGHT
                || aICore.GetState() == AIState.FLEE
            ;

            if (shouldIgnore) {
                yield return null;
            }

            CastVision();

            yield return null;
        }

        void CastVision()
        {
            Vector3 direction = player.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);


            if (angle < fieldOfView * 0.5f)
            {
                RaycastHit hit;

                Vector3 ownHeight = col.bounds.center;

                if (Physics.Raycast(ownHeight, direction.normalized, out hit, minimumDistanceToCastVision))
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        // Record last known position of player
                        playerLastKnownPosition = hit.collider.gameObject.transform.position;

                        OnPlayerSighted.Invoke();
                    }
                }
            }
        }

        public Vector3 GetLastKnownPositionOfPlayer()
        {
            return playerLastKnownPosition;
        }
    }
}
