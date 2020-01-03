using UnityEngine;
using UnityEngine.AI;
using RPG.AI;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control {
    
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AIMovement))]
    [RequireComponent(typeof(Battler))]
    [RequireComponent(typeof(Health))]
    public class AIController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Range(0, 1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;


        [Header("Chasing Settings")]
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;

        [Header("Waypoint Settings")]
        [SerializeField] PatrolPath patrolPath = null;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;

        Vector3 originalPosition;
        int currentWaypointIndex = 0;

        // Timers
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;

        // Private components
        Battler battler => GetComponent<Battler>();
        Health health => GetComponent<Health>();
        AIMovement movement => GetComponent<AIMovement>();
        GameObject player = null;

        [SerializeField]
        StateMachineEnum state = StateMachineEnum.PATROL;

        StateMachineEnum previousState;

        void Start()
        {
            player = GameObject.FindWithTag("Player");

            // Store the original position
            originalPosition = transform.position;
        }

        void Update()
        {
            if (health.IsDead())
            {
                return;
            }

            FSM();

            UpdateTimers();
        }

        void FSM()
        {
            switch (state)
            {
                case StateMachineEnum.PATROL:
                    PatrolBehaviour();
                    break;
                case StateMachineEnum.CHAT:
                    ChatBehaviour();
                    break;
                case StateMachineEnum.IDLE:
                default:
                    return;
            }
        }

        // Setters
        public void SetState(StateMachineEnum nextState)
        {
            previousState = state;
            state = nextState;
        }
        public void SetPreviousState()
        {
            state = previousState;
        }

        // Behaviour Logic
        void ChatBehaviour()
        {
            transform.LookAt(player.transform);
            movement.Cancel();

        }

        void PatrolBehaviour()
        {
            Vector3 nextPosition = originalPosition;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0f;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime) { 
                // Return to the original position
                movement.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        // Private
        void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        bool AtWaypoint()
        {
            return Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointTolerance;
        }

        void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        bool InAttackRangeOfPlayer()
        {
            return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
        }

        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
