using UnityEngine;
using System.Collections;
using RPG.Combat;

namespace RPG.AI
{
    [RequireComponent(typeof(BehaviourCombat))]
    [RequireComponent(typeof(Battler))]
    public class BehaviourTakeDamage : MonoBehaviour
    {

        Animator animator => GetComponent<Animator>();
        AIController controller => GetComponent<AIController>();
        Battler battler => GetComponent<Battler>();

        BehaviourCombat combatBehaviour => GetComponent<BehaviourCombat>();

        // Constants
        const string ANIMATOR_TRIGGER_TAKE_DAMAGE = "TakeDamage";

        // Internal
        bool inProgress = false;

        public void Dispatch()
        {
            if (inProgress == false)
            {
                inProgress = true;
                StartCoroutine(TakeDamage());
            }
        }

        IEnumerator TakeDamage()
        {
            animator.SetTrigger(ANIMATOR_TRIGGER_TAKE_DAMAGE);

            yield return new WaitUntil(() => battler.IsTakingDamage());

            yield return new WaitUntil(() => !battler.IsTakingDamage());

            HandleNextState();

            inProgress = false;
        }

        void HandleNextState()
        {
            // If player is out of range, chase him
            if (combatBehaviour.PlayerIsFarAway())
            {
                controller.SetState(StateMachineEnum.CHASE);
                return;
            }

            // Otherwise, enter combat
            controller.SetState(StateMachineEnum.COMBAT);
        }

    }

}
