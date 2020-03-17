using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using RPG.Core;
using RPG.Combat;
using RPG.Weapon;
using RPG.AI;
using RPG.Saving;
using System.Linq;

namespace RPG.AIV3 {

    public class AI_Core_V3 : MonoBehaviour {

        [Header("Alliance")]
        public ALLIANCE alliance = ALLIANCE.PLAYER;

        [Header("Vision Settings")]
        [SerializeField] float fieldOfView = 110f;
        [SerializeField] float minimumDistanceToCastVision = 10f;
        [SerializeField] Vector3 targetLastKnownPosition;
        [SerializeField] UnityEvent OnTargetSpotted;

        [Header("Patrol Settings")]
        [Range(0f, 10f)] [SerializeField] float maxSpeed = 3f;
        [Range(0, 1)] [SerializeField] float patrolSpeedFraction = 0.2f;
        [SerializeField] PatrolPath patrolPath = null;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;
        Vector3 originalPosition;

        [Header("Chase Settings")]
        [Range(0f, 10f)] [SerializeField] float chaseSpeed = 5f;
        [SerializeField] float maxChasingDistance = 20f;
        [SerializeField] float stoppingDistanceToPlayer = 1f;
        [SerializeField] float staminaChasingCost = 3f;

        [Header("Flee Settings")]
        public float safeDistanceFromTarget = 15f;

        [Header("Target")]
        public string[] candidateTags;
        [HideInInspector] public GameObject target;

        [SerializeField] AGENT_STATE state = AGENT_STATE.PATROL;

        // Private         
        [HideInInspector] public Health health => GetComponent<Health>();
        [HideInInspector] public Stamina stamina => GetComponent<Stamina>();
        [HideInInspector] public NavMeshAgent navMeshAgent => GetComponent<NavMeshAgent>();
        [HideInInspector] public Battler battler => GetComponent<Battler>();
        WeaponManager weaponManager => GetComponent<WeaponManager>();

        public bool inProgress = false;
        
        // Behaviours
        Fight fight;
        Flee flee;
        Talk talk;

        void Start() {
            // Create behaviours
            fight = new Fight(this);
            flee = new Flee(this);
            talk = new Talk(this);
        }

        void Update() {

            if (health.IsDead()) {
                GetComponent<CapsuleCollider>().enabled = false;
                GetComponent<Battler>().enabled = false;
                GetComponent<NavMeshAgent>().enabled = false;

                // Can cause bugs if enemy dies in the air
                GetComponent<Rigidbody>().isKinematic = true;

                state = AGENT_STATE.DEAD;
                return;
            }

            HandleFSM();
        }

        void HandleFSM() {
            if (state == AGENT_STATE.TALKING) {
                GetComponent<Animator>().SetFloat("InputVertical", 0);   
                navMeshAgent.isStopped = true;
            } else {
                HandleMovement();
                HandleVision();
            }

            switch (state)
            {
                case AGENT_STATE.PATROL:
                    Patrol();
                    break;
                case AGENT_STATE.CHASE:
                    Chase();
                    break;
                case AGENT_STATE.FIGHTING:
                    fight.Dispatch();
                    break;
                case AGENT_STATE.FLEE:
                    flee.Dispatch();
                    break;
                case AGENT_STATE.TALKING:
                    talk.Dispatch();
                    break;
                case AGENT_STATE.DEAD:
                default:
                    return;
            }

            // GetComponent<Animator>().SetBool("IsStrafing", inProgress);
        }


        // Components

        void HandleMovement() {
            navMeshAgent.enabled = !health.IsDead();

            // Update animator
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;

            if (state == AGENT_STATE.PATROL)
            {
                speed = speed >= 0.5f ? 0.5f : speed;
            }

            GetComponent<Animator>().SetFloat("InputVertical", speed);   
        }

        void HandleVision() {
            if (target && target.GetComponent<Health>().IsDead())
            {
                return;
            }

            // Consider only casting the vision on patrol
            bool castVision = !(
                state == AGENT_STATE.FIGHTING
                || state == AGENT_STATE.FLEE
            ) && (alliance == ALLIANCE.ENEMY);

            if (castVision == false)
            {
                return;
            }


            RaycastHit hit;
            Vector3 ownHeight = GetComponent<Collider>().bounds.center;

            Vector3 direction = target != null
                ? target.transform.position - transform.position
                : transform.forward;

            float angle = Vector3.Angle(direction, transform.forward);

            if (
                target != null
                    ? angle < fieldOfView * 0.5f
                    : true
            )
            {

                if (Physics.Raycast(ownHeight, direction.normalized, out hit, minimumDistanceToCastVision))
                {
                    if (candidateTags.Contains(hit.collider.gameObject.tag))
                    {
                        OnTargetDetection(hit.collider.gameObject);
                    }
                }
            }
        }
        public void OnTargetDetection(GameObject spottedTarget)
        {
            target = spottedTarget;

            // Record last known position of player
            targetLastKnownPosition = spottedTarget.transform.position;

            if (state != AGENT_STATE.CHASE)
            {
                // Chase Target
                state = AGENT_STATE.CHASE;

                OnTargetSpotted.Invoke();
            }
        }

        // Behaviours

        // PATROL
        void Patrol() {
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
                // MoveTo(nextPosition);
                AI_Helpers.MoveTo(
                    nextPosition,
                    maxSpeed,
                    patrolSpeedFraction,
                    navMeshAgent
                );
            }

            return;
        }

        // PATROL (Utils)
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

        // CHASE
        void Chase()
        {
            if (target == null) 
            {
                return;
            }

            Vector3 destination = targetLastKnownPosition;

            float currentDistance = Vector3.Distance(target.transform.position, transform.position);

            // Has reached target
            if (currentDistance < stoppingDistanceToPlayer)
            {
                state = AGENT_STATE.FIGHTING;
                return;
            }

            // Player escaped
            if (currentDistance > maxChasingDistance)
            {
                state = AGENT_STATE.PATROL;
                return;
            }

            // Has Stamina To Continue chase?
            if (stamina.HasStaminaAgainstCostAction(staminaChasingCost))
            {
                // MoveTo(destination);

                AI_Helpers.MoveTo(destination, maxSpeed, patrolSpeedFraction, navMeshAgent);

                stamina.DecreaseStamina(staminaChasingCost);
            }
            else
            {
                // Stamina depleted
                return;
            }
        }

        // FIGHTING
        private void LateUpdate()
        {

            bool facePlayer = state == AGENT_STATE.CHASE || state == AGENT_STATE.FIGHTING;

            if (facePlayer) {
                AI_Helpers.FaceTarget(
                    target,
                    this.gameObject
                );
            }
        }

        /*void FacePlayer()
        {
            Vector3 targetPosition = new Vector3(target.transform.position.x,
                                                   this.transform.position.y,
                                                   target.transform.position.z);
            this.transform.LookAt(targetPosition);
        }*/

        /*void Fight()
        {
            if (inProgress) {
                return;
            }

            if (target == null) {
                return;
            }

            if (target.GetComponent<Health>().IsDead())
            {
                return;
            }
        
            if (TargetIsFarAway())
            {
                state = AGENT_STATE.CHASE;
                return;
            }

            if (TargetIsAttacking() == true) {
                if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
                {
                    StartCoroutine(Defend());
                }
            }
            else
            {
                StartCoroutine(Attack());
            }
        }

        IEnumerator Attack()
        {
            inProgress = true;

            battler.Attack();

            // Wait Until We Trigger Attack Animation
            yield return new WaitUntil(() => battler.IsAttacking() == true);

            // Now Wait Until Attack Animation Is Over
            yield return new WaitUntil(() => battler.IsAttacking() == false);

            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 1f));

            inProgress = false;

            yield return null;
        }
        IEnumerator Defend()
        {
            inProgress = true;

            battler.Defend();

            // Wait Until We Trigger Attack Animation
            yield return new WaitUntil(() => battler.IsDefending() == true);

            // Now Wait Until Attack Animation Is Over
            yield return new WaitUntil(() => battler.IsDefending() == false);

            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 1f));
            
            inProgress = false;

            yield return null;
        }

        // COMBAT Utils
        public bool TargetIsFarAway()
        {
            return Vector3.Distance(target.transform.position, transform.position) > weaponManager.GetStoppingDistance();
        }
        bool TargetIsAttacking()
        {
            return target.GetComponent<Battler>().IsAttacking();
        }

        bool TargetIsDefending()
        {
            return target.GetComponent<Battler>().IsDefending();
        }
    */

        // PUBLIC
        public void SetState (AGENT_STATE state) {
            this.state = state;
        }

        public void SetTarget (GameObject target) {
            this.target = target;
        }

        public void TakeDamage(GameObject target) {
            this.target = target;

            transform.Translate(Vector3.back);

            state = AGENT_STATE.CHASE;
        }

        // Save System

        public object CaptureState()
        {
            return new SaveableAgent(transform.position, state);
        }

        public void RestoreState(object state)
        {
            SaveableAgent savedState = (SaveableAgent)state;

            // Reapply saved position
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = savedState.position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;

            state = savedState.currentState;
        }
    }

    // Serializable Class
    [System.Serializable]
    public class SaveableAgent
    {
        public SerializableVector3 position;
        public AGENT_STATE currentState;

        public SaveableAgent(Vector3 position, AGENT_STATE currentState)
        {
            this.position = new SerializableVector3(position);
            this.currentState = currentState;
        }
    }

    public enum AGENT_STATE {
        PATROL,
        CHASE,
        FLEE,
        FIGHTING,
        TALKING,
        DEAD,
    }

    public enum ALLIANCE {
        PLAYER, // Friends to player
        NEUTRAL, // Friends to everyone or no one
        ENEMY // Enemy of player
    }

}
