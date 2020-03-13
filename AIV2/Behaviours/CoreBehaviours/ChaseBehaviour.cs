using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using RPG.Saving;
using RPG.AI;
using RPG.Core;
using RPG.Combat;
using RPG.Weapon;

namespace RPG.AIV2 {

    public class ChaseBehaviour : Behaviour {

        [Range(0f, 10f)]
        [SerializeField] float chaseSpeed = 5f;

        [SerializeField] float maxChasingDistance = 20f;

        [SerializeField] float stoppingDistanceToPlayer = 1f;

        [SerializeField] float staminaChasingCost = 3f;

        public UnityEvent OnChaseSuccessfully;
        public UnityEvent OnChaseFailed;
        public UnityEvent OnStaminaDepleted;

        NavMeshAgent navMeshAgent => GetComponent<NavMeshAgent>();

        Stamina stamina => GetComponent<Stamina>();

        GameObject player;

        void Start()
        {
            player = GameObject.FindWithTag("Player");
        }

        public override IEnumerator Dispatch() {

            StartCoroutine(Chase());

            yield return null;
        }

        public IEnumerator Chase()
        {
            Vector3 destination = GetComponent<Vision>().GetLastKnownPositionOfPlayer();

            float currentDistance = Vector3.Distance(player.transform.position, transform.position);

            // Has reached player
            if (currentDistance < stoppingDistanceToPlayer)
            {
                OnChaseSuccessfully.Invoke();
                yield return null;
            }

            // Player escaped
            if (currentDistance > maxChasingDistance)
            {
                OnChaseFailed.Invoke();
                yield return null;
            }

            // Has Stamina To Continue chase?
            if (stamina.HasStaminaAgainstCostAction(staminaChasingCost))
            {
                MoveTo(destination);
                stamina.DecreaseStamina(staminaChasingCost);
            }
            else
            {
                OnStaminaDepleted.Invoke();
                yield return null;
            }

            yield return null;
        }

        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.speed = chaseSpeed;
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }

    }
}
