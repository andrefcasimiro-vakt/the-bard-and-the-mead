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


        void Update()
        {
            tps.enabled = !health.IsDead();
            input.enabled = !health.IsDead();
            playerController.enabled = !health.IsDead();
        }

    }

}
