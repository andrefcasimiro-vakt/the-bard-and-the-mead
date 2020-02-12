using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.AI { 
    /// <summary>
    /// Triggers a custom animation action when the agent collides with this trigger
    /// E. g. Picking flowers, mopping the floor, etc.
    /// </summary>
    public class AIAction : MonoBehaviour
    {

        public GameObject actor;
        AIController aiController => actor.GetComponent<AIController>();
        Animator animator => actor.GetComponent<Animator>();

        public AnimatorOverrideController defaultAnimatorOverrideController;
        public AnimatorOverrideController animatorOverrideController;

        [Header("Action Settings")]
        [Tooltip("The time this action lasts. If longer than the animation clip, the animation should loop.")]
        public float stateDuration = 10f;

        private string IS_CUSTOM_ACTION_ENABLED_ANIMATOR_PARAMETER = "isCustomActionEnabled";

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == actor)
            {
                StartCoroutine(PerformAction());
            }
        }

        IEnumerator PerformAction()
        {
            // Stop AI
            aiController.SetState(StateMachineEnum.CUSTOM_ACTION);

            animator.runtimeAnimatorController = animatorOverrideController as RuntimeAnimatorController;

            animator.SetBool(IS_CUSTOM_ACTION_ENABLED_ANIMATOR_PARAMETER, true);
            yield return new WaitForSeconds(stateDuration);

            animator.SetBool(IS_CUSTOM_ACTION_ENABLED_ANIMATOR_PARAMETER, false);

            animator.runtimeAnimatorController = defaultAnimatorOverrideController as RuntimeAnimatorController;

            // Return to old state
            aiController.SetState(StateMachineEnum.PATROL);
        }
    }
}
