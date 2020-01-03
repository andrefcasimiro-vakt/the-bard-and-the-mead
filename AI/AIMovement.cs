using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.AI
{
    [RequireComponent(typeof(ActionScheduler))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIMovement : MonoBehaviour, IAction
    {
        [Range(0f, 10f)]
        [SerializeField] float maxSpeed;

        string ANIMATOR_PARAMETER_FORWARD_SPEED = "InputVertical";

        NavMeshAgent navMeshAgent;
        Health health;

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
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

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }
    }
}
