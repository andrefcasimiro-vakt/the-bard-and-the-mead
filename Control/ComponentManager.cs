using UnityEngine;
using Invector.CharacterController;
using RPG.Core;

namespace RPG.Control {

    public class ComponentManager: MonoBehaviour {

        Animator animator => GetComponent<Animator>();
        Rigidbody rb => GetComponent<Rigidbody>();
        CapsuleCollider collider => GetComponent<CapsuleCollider>();
        vThirdPersonController tps => GetComponent<vThirdPersonController>();
        vThirdPersonInput input => GetComponent<vThirdPersonInput>();
        PlayerController playerController => GetComponent<PlayerController>();
        Health health => GetComponent<Health>();

        bool isDisabled = false;


        void Update()
        {
            if (isDisabled)
            {
                // Fixes for Invector third person controller
                animator.SetFloat("InputVertical", 0f);
                collider.material = null;
            }
        }

        public void Enable()
        {
            isDisabled = false;
            ToggleComponents(true);
        }

        public void Disable()
        {
            isDisabled = true;
            ToggleComponents(false);
        }

        private void ToggleComponents(bool value)
        {
            rb.isKinematic = !value;

            tps.enabled = value;
            input.enabled = value;
            playerController.enabled = value;
            health.enabled = value;
        }
    }

}
