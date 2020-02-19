using UnityEngine;
using UnityEngine.AI;

namespace RPGV2.AI
{
    public class AnimatorNavmeshSync : MonoBehaviour
    {
        [Header("---")]
        [Header("Syncs animator and navmesh so the agent has animation when moving")]
        public string FORWARD_PARAM = "InputVertical";

        Animator animator;
        NavMeshAgent navMeshAgent;

        void Start()
        {
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update() 
        {
            if (animator == null) {
                print("Missing animator component on Movement script for " + gameObject.name);
                return;
            } else if (navMeshAgent == null) {
                print("Missing navMeshAgent component on Movement script for " + gameObject.name);
                return;
            }

            UpdateAnimator();
        }

        void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;
            animator.SetFloat(FORWARD_PARAM, speed);    
        }

    }
}
