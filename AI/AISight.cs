using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.Core;

namespace RPG.AI {

    [RequireComponent(typeof(CapsuleCollider))]
    public class AISight : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        float fieldOfView = 110f;
        [SerializeField]
        float minDistanceToPlayer = 10f;

        [SerializeField]
        Vector3 playerLastKnownPosition;

        [SerializeField]
        UnityEvent OnPlayerSighted;

        CapsuleCollider ownerCollider => GetComponent<CapsuleCollider>();
        GameObject player;
        AIController controller => GetComponent<AIController>();


        private void Start()
        {
            player = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
            // If player dead, ignore
            if (player.GetComponent<Health>().IsDead())
            {
                return;
            }

            // Dont Run If Exhausted / Arrived At Player / Combating
            if (
                controller.GetCurrentState() == StateMachineEnum.REST
                || controller.GetCurrentState() == StateMachineEnum.ARRIVED_AT_PLAYER
                || controller.GetCurrentState() == StateMachineEnum.COMBAT
             )
            {
                return;
            }

            if (Vector3.Distance(player.transform.position, this.transform.position) < minDistanceToPlayer)
            {
                Visualize();
            }
        }

        private void Visualize()
        {
            Vector3 direction = player.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);


            if (angle < fieldOfView * 0.5f)
            {
                RaycastHit hit;

                Vector3 ownHeight = ownerCollider.bounds.center;

                if (Physics.Raycast(ownHeight, direction.normalized, out hit, minDistanceToPlayer))
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
