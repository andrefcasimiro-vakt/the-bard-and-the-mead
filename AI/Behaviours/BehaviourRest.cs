using UnityEngine;
using System.Collections;


namespace RPG.AI
{

    public class BehaviourRest: MonoBehaviour
    {

        Animator animator => GetComponent<Animator>();
        AIController controller => GetComponent<AIController>();

        const string ANIMATOR_TRIGGER_EXHAUSTED = "Exhausted";

        bool inProgress = false;

        public void Dispatch()
        {
            if (inProgress == false)
            {
                inProgress = true;
                StartCoroutine(RestForABit());
            }
        }


        IEnumerator RestForABit()
        {
            // Play exausted animation
            animator.SetTrigger(ANIMATOR_TRIGGER_EXHAUSTED);

            yield return new WaitForSeconds(7f);

            // After resting for a bit, resume patrol
            controller.SetState(StateMachineEnum.PATROL);

            // Reset
            inProgress = false;
        }
    }

}
