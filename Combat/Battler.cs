using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Weapon;
using RPG.Core;

namespace RPG.Combat {

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(WeaponManager))]
    public class Battler : MonoBehaviour
    {
        [Header("Battler Audio")]
        public AudioClip gruntAudioClip;

        Animator animator => GetComponent<Animator>();
        Health health => GetComponent<Health>();
        Stamina stamina => GetComponent<Stamina>();
        WeaponManager weaponManager => GetComponent<WeaponManager>();

        [Header("Dodge")]
        [SerializeField] float dodgeStaminaCost = 10f;

        // Constants
        const int COMBAT_LAYER_INDEX = 1;
        const string ANIM_TRIGGER_DEFENSE_PARAMETER = "Defend";
        const string ANIM_TRIGGER_DODGE_PARAMETER = "Dodge";

        public void Attack()
        {
            StartCoroutine(ExecuteAttack());
        }

        public void Defend()
        {
            StartCoroutine(ExecuteDefense());
        }

        public void Dodge()
        {
            if (!stamina.HasStaminaAgainstCostAction(dodgeStaminaCost))
            {
                return;
            }

            stamina.DecreaseStamina(dodgeStaminaCost * 100f);
            StartCoroutine(ExecuteDodge());
        }

        IEnumerator ExecuteAttack()
        {
            weaponManager.Attack(gruntAudioClip);

            // Stop movement when attacking
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            yield return new WaitUntil(() => IsAttacking());
            
            yield return new WaitUntil(() => !IsAttacking());

            // Restore movement
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }

        IEnumerator ExecuteDefense()
        {
            animator.SetTrigger(ANIM_TRIGGER_DEFENSE_PARAMETER);

            yield return new WaitUntil(() => IsDefending());
            // Deactivate healthbox
            health.enabled = false;
            yield return new WaitUntil(() => !IsDefending());
            health.enabled = true;
        }

        IEnumerator ExecuteDodge()
        {
            animator.SetTrigger(ANIM_TRIGGER_DODGE_PARAMETER);

            yield return new WaitUntil(() => IsDodging());
            // Deactivate healthbox
            health.enabled = false;
            yield return new WaitUntil(() => !IsDodging());
            health.enabled = true;
        }

        // Getters
        public bool IsAttacking()
        {
            return GetComponent<Animator>().GetCurrentAnimatorStateInfo(COMBAT_LAYER_INDEX).IsTag("Attack");
        }
        public bool IsDefending()
        {
            return GetComponent<Animator>().GetCurrentAnimatorStateInfo(COMBAT_LAYER_INDEX).IsTag("Defend");
        }
        public bool IsDodging()
        {
            return GetComponent<Animator>().GetCurrentAnimatorStateInfo(COMBAT_LAYER_INDEX).IsTag("Dodge");
        }
        public bool IsTakingDamage()
        {
            return GetComponent<Animator>().GetCurrentAnimatorStateInfo(COMBAT_LAYER_INDEX).IsTag("TakeDamage");
        }
    }
}
