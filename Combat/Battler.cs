using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Weapon;
using RPG.Core;
using RPG.Stats;
using RPG.Control;

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
        public AudioClip dodgeClip;

        [Header("Rewards")]
        public int baseExpForKilling = 100;

        // Constants
        const int COMBAT_LAYER_INDEX = 1;
        const string ANIM_TRIGGER_DEFENSE_PARAMETER = "Defend";
        const string ANIM_TRIGGER_DODGE_PARAMETER = "Dodge";
        const string ANIM_TRIGGER_TAKE_DAMAGE_PARAMETER = "TakeDamage";

        bool isPlayer = false;

        void Start()
        {
            isPlayer = this.gameObject.tag == "Player";
        }

        public void Attack()
        {
            if (IsTakingDamage())
                return;

            StartCoroutine(ExecuteAttack());
        }

        public void Defend()
        {   
            if (isPlayer)
            {

                return;
            }

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

        public void TakeDamage()
        {
            StartCoroutine(ExecuteTakeDamage());
        }

        IEnumerator ExecuteAttack()
        {
            weaponManager.Attack(gruntAudioClip);

            // Stop movement when attacking

            yield return new WaitUntil(() => IsAttacking());
            
            yield return new WaitUntil(() => !IsAttacking());

            // Restore movement
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
            health.enabled = false;

            yield return new WaitUntil(() => IsDodging());

            yield return new WaitUntil(() => !IsDodging());
            health.enabled = true;
        }

        IEnumerator ExecuteTakeDamage()
        {
            animator.SetTrigger(ANIM_TRIGGER_TAKE_DAMAGE_PARAMETER);
            health.enabled = false;

            yield return new WaitUntil(() => IsTakingDamage());
            yield return new WaitUntil(() => !IsTakingDamage());
            health.enabled = true;
        }

        // Getters
        public bool IsAttacking()
        {
            return GetComponent<Animator>().GetCurrentAnimatorStateInfo(COMBAT_LAYER_INDEX).IsTag("Attack");
        }
        public bool IsDefending()
        {   
            if (isPlayer)
            {
                return GetComponent<Animator>().GetBool("IsDefending");
            }

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

        public int GetRewardExperience()
        {
            return baseExpForKilling * GetComponent<BaseStats>().currentLevel;
        }
    }
}
