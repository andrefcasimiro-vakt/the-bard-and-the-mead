using UnityEngine;
using UnityEngine.AI;
using RPG.AI;
using RPG.Combat;
using RPG.Core;
using RPG.Saving;
using System.Collections;

namespace RPG.AI {
    
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AIMovement))]
    [RequireComponent(typeof(Battler))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(AISight))]
    [RequireComponent(typeof(BehaviourCombat))]
    [RequireComponent(typeof(BehaviourTakeDamage))]
    [RequireComponent(typeof(BehaviourRunAway))]
    [RequireComponent(typeof(BehaviourRest))]
    public class AIController : MonoBehaviour, ISaveable
    {
        [Header("Movement Settings")]
        [Range(0, 1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;

        [Header("Chasing Settings")]
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [Range(0, 1)]
        [SerializeField] float chaseSpeedFraction = 0.4f;

        [Header("Fleeing Settings")]
        [Range(0, 1)]
        public float fleeSpeedFraction = 0.3f;

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
        AISight sight => GetComponent<AISight>();

        GameObject player = null;

        [SerializeField]
        StateMachineEnum state = StateMachineEnum.PATROL;
        StateMachineEnum previousState;

        // Actions
        BehaviourCombat combat => GetComponent<BehaviourCombat>();
        BehaviourTakeDamage takeDamage => GetComponent<BehaviourTakeDamage>();
        BehaviourRunAway runAway => GetComponent<BehaviourRunAway>();
        BehaviourRest rest => GetComponent<BehaviourRest>();

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
                case StateMachineEnum.CHASE:
                    ChaseBehaviour();
                    break;
                case StateMachineEnum.FLEE:
                    FleeBehaviour();
                    break;
                case StateMachineEnum.ARRIVED_AT_PLAYER:
                    ArrivedAtPlayerBehaviour();
                    break;
                case StateMachineEnum.TAKE_DAMAGE:
                    TakeDamageBehaviour();
                    break;
                case StateMachineEnum.REST:
                    RestBehaviour();
                    break;
                case StateMachineEnum.CHAT:
                    ChatBehaviour();
                    break;
                case StateMachineEnum.IDLE:
                default:
                    return;
            }
        }

        // Editor Event Actions
        public void Chase()
        {
            previousState = state;
            state = StateMachineEnum.CHASE;
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

        // Getters
        public StateMachineEnum GetCurrentState()
        {
            return state;
        }

        // FSM Behaviour Logic


        // =====================> MOVEMENT RELATED
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

            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                // Return to the original position
                movement.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        void ChaseBehaviour()
        {
            movement.Cancel();

            movement.ChaseTowards(sight.GetLastKnownPositionOfPlayer(), chaseSpeedFraction);
        }

        void FleeBehaviour()
        {
            movement.Cancel();

            runAway.Dispatch();
        }

        // ==================> PROXIMITY BEHAVIOURS
        void ArrivedAtPlayerBehaviour()
        {
            // If is NPC like a courier, invoke an event maybe

            // If is agressive towards the player, enter combat
            CombatBehaviour();
        }

        // ==================> COMBAT BEHAVIOURS

        void CombatBehaviour()
        {
            movement.Cancel();
            
            combat.Dispatch();
        }

        void TakeDamageBehaviour()
        {
            movement.Cancel();

            takeDamage.Dispatch();
        }

        // ===============> GENERIC BEHAVIOURS

        void RestBehaviour()
        {
            movement.Cancel();

            rest.Dispatch();
        }

        void ChatBehaviour()
        {
            transform.LookAt(player.transform);
            
            movement.Cancel();
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

        public object CaptureState()
        {
            return new SaveableAgent(transform.position, GetCurrentState());
        }

        public void RestoreState(object state)
        {
            SaveableAgent loadedState = (SaveableAgent)state;

            // Reapply saved position
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = loadedState.position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;

            SetState(loadedState.currentState);
        }
    }

    // For the saving system
    [System.Serializable]
    public class SaveableAgent
    {
        public SerializableVector3 position;
        public StateMachineEnum currentState;

        public SaveableAgent(Vector3 position, StateMachineEnum currentState)
        {
            this.position = new SerializableVector3(position);
            this.currentState = currentState;
        }
    }
}
