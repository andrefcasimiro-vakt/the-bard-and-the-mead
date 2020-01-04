using UnityEngine;
using RPG.Core;
using RPG.Combat;
using Invector.CharacterController;

namespace RPG.Control {

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Battler))]
    [RequireComponent(typeof(vThirdPersonInput))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Combat Input")]
        [SerializeField] string attackInput;
        [SerializeField] string defendInput;
        [SerializeField] string dodgeInput;
        [SerializeField] float inputCooldown = 0.5f;
        float timer = Mathf.Infinity;

        [Header("Sprint Input")]
        [SerializeField] float sprintStaminaCost = 5f;

        vThirdPersonInput vThirdPersonInput => GetComponent<vThirdPersonInput>();
        Battler battler => GetComponent<Battler>();
        Stamina stamina => GetComponent<Stamina>();

        void Update()
        {
            timer += Time.deltaTime;

            CombatInput();
        }

        private void LateUpdate()
        {
            SprintInput();
        }

        void CombatInput()
        {
            if (timer < inputCooldown)
            {
                return;
            }

            if (Input.GetButtonDown(attackInput))
            {
                battler.Attack();
                timer = 0f;

                return;
            }

            if (Input.GetButtonDown(defendInput))
            {
                battler.Defend();
                timer = 0f;

                return;
            }

            if (Input.GetButtonDown(dodgeInput))
            {
                battler.Dodge();
                timer = 0f;

                return;
            }
        }

        // Depends entirely on Invector Free Third Person Controller
        // At least until we have our own controller
        void SprintInput()
        {
            if (stamina.HasStamina())
            {
                vThirdPersonInput.canRun = true;
            }
            else
            {
                vThirdPersonInput.canRun = false;
            }

            // If actually running, decrease stamina
            if (vThirdPersonInput.cc.isSprinting)
            {
                // TPS warns us the user is sprinting regardless of the velocity
                // But it relies to on the shift command to also make the character run
                // So we need to check the animator velocity here.
                if (GetComponent<Animator>().GetFloat("InputVertical") < 1f)
                {
                    // Left shift is pressed but animation is not sprint but walk
                    // Dont decrease stamina
                    return;
                }

                stamina.DecreaseStamina(sprintStaminaCost);
            }
        }

        public float GetInputCooldown()
        {
            return inputCooldown;
        }
    }
}
