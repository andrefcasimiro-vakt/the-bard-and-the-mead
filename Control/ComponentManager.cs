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

        bool lockAnimator = false;

        void Update()
        {
            if (health.IsDead())
            {
                ToggleComponents(false);
            }
        }

        void FixedUpdate()
        {
            if (lockAnimator)
            {
                GetComponent<Animator>().SetFloat("InputVertical", 0f);
            }
        }

        public void ToggleComponents(bool v)
        {
            tps.enabled = v;
            input.enabled = v;
            playerController.enabled = v;

            lockAnimator = !v;

            if (v == false)
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            }
        }

    }

}
