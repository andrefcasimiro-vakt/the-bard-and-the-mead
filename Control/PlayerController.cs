using UnityEngine;
using RPG.Core;
using RPG.Combat;
using RPG.Weapon;
using RPG.Saving;
using Invector.CharacterController;

namespace RPG.Control {

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Battler))]
    [RequireComponent(typeof(vThirdPersonInput))]
    public class PlayerController : MonoBehaviour, ISaveable
    {
        [Header("Combat Input")]
        [SerializeField] string attackInput;
        [SerializeField] string defendInput;
        [SerializeField] float inputCooldown = 0.5f;
        float timer = Mathf.Infinity;

        [Header("Sprint Input")]
        [SerializeField] float sprintStaminaCost = 5f;

        vThirdPersonController vThirdPersonCC => GetComponent<vThirdPersonController>();
        vThirdPersonInput vThirdPersonInput => GetComponent<vThirdPersonInput>();
        Battler battler => GetComponent<Battler>();
        Stamina stamina => GetComponent<Stamina>();

        void Update()
        {
            timer += Time.deltaTime;

            // Only allow combat if time scale is bigger than zero
            if (Time.timeScale > 0) CombatInput();
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

            // HANDLE PARRYING SOUNDS
            if (Input.GetButtonDown(defendInput))
            {
                GetComponent<WeaponManager>().Defend();
            }

            if (Input.GetButton(defendInput))
            {
                battler.Defend();
                timer = 0f;
                GetComponent<Animator>().SetBool("IsDefending", true);
                return;
            }
            else
            {
                GetComponent<Animator>().SetBool("IsDefending", false);
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

        public void Strafe (bool strafeValue)
        {
            vThirdPersonCC.isStrafing = strafeValue;
        }

        public float GetInputCooldown()
        {
            return inputCooldown;
        }


        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;

            // Reapply saved position
            transform.position = position.ToVector();
        }

        public void OnCleanState() {

        }
    }
}
