using UnityEngine;
using RPG.Combat;

namespace RPG.AI
{
    [RequireComponent(typeof(BehaviourCombat))]
    [RequireComponent(typeof(Battler))]
    [RequireComponent(typeof(AIMovement))]
    public class BehaviourRunAway : MonoBehaviour
    {
        [Header("Flee Settings")]
        [SerializeField] float safeDistance = 10f;

        AIController controller => GetComponent<AIController>();
        AIMovement movement => GetComponent<AIMovement>();

        GameObject player;


        private void Start()
        {
            player = GameObject.FindWithTag("Player");
        }

        public void Dispatch()
        {
            Flee();
        }

        void Flee()
        {
            Vector3 destination = transform.position - player.transform.position;

            movement.MoveTo(destination, controller.fleeSpeedFraction);

            CheckDistance();
        }

        void CheckDistance()
        {
            // If player is out of range, chase him
            bool isFarAwayFromPlayer = Vector3.Distance(transform.position, player.transform.position) > safeDistance;


            if (isFarAwayFromPlayer) {
                controller.SetState(StateMachineEnum.PATROL);
                return;
            }
        }

    }

}
