using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.AI
{
    [RequireComponent(typeof(ActionScheduler))]
    [RequireComponent(typeof(AIController))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Stamina))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIMovement : MonoBehaviour, IAction
    {
        [Range(0f, 10f)]
        [SerializeField] float maxSpeed;

        [SerializeField] float stoppingDistanceToPlayer = 1f;

        [SerializeField] float maxChasingDistance = 20f;

        [Header("Character Stamina")]
        [SerializeField] float staminaChasingCost = 3f;

        string ANIMATOR_PARAMETER_FORWARD_SPEED = "InputVertical";

        AIController aiController => GetComponent<AIController>();
        NavMeshAgent navMeshAgent => GetComponent<NavMeshAgent>();
        Health health => GetComponent<Health>();
        Stamina stamina => GetComponent<Stamina>();
        GameObject player;

        void Start()
        {
            player = GameObject.FindWithTag("Player");
        }

        void Update() 
        {
            navMeshAgent.enabled = !health.IsDead();

            UpdateAnimator();
        }

        void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat(ANIMATOR_PARAMETER_FORWARD_SPEED, speed);    
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {

            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }

        // Takes into account AI agent stamina
        // Which will define if we should continue this action or return to a previous one
        public void ChaseTowards(Vector3 destination, float speedFraction)
        {
            float currentDistance = Vector3.Distance(player.transform.position, transform.position);

            // Has reached player
            if (currentDistance < stoppingDistanceToPlayer)
            {
                aiController.SetState(StateMachineEnum.ARRIVED_AT_PLAYER);

                return;
            }

            // Player escaped
            if (currentDistance > maxChasingDistance)
            {
                aiController.SetState(StateMachineEnum.PATROL);
                return;
            }

            // Has Stamina To Continue chase?
            if (stamina.HasStaminaAgainstCostAction(staminaChasingCost))
            {
                MoveTo(destination, speedFraction);
                stamina.DecreaseStamina(staminaChasingCost);
            }
            else
            {
                // Stamina depleted, go to REST state
                aiController.SetState(StateMachineEnum.REST);
            }

        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }
    }
}
