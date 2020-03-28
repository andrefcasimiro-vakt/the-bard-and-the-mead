using System.Collections;
using UnityEngine;
using RPG.Core;
using RPG.Combat;
using RPG.Inventory;
using RPG.AIV3;
using RPG.Stats;

namespace RPG.Weapon {

    [RequireComponent(typeof(AudioSource))]
    [System.Serializable]
    public class WeaponSlot: MonoBehaviour {

        public string slotName = "";
        public ScriptableWeapon currentWeapon = null;
        // In world stored reference
        GameObject weaponGameObject = null;

        // Path to the unarmed weapon scriptable object in Database/Resources/Weapons folder
        const string UNARMED_WEAPON_PATH = "Weapons/Unarmed";
        ScriptableWeapon unarmedWeapon = null;

        GameObject weaponOwner = null;
        Hitbox hitbox = null;
        AudioSource audioSource => GetComponent<AudioSource>();

        Battler battler => GetComponent<Battler>();

        // ANIMATOR STRINGS
        const string AttackTrigger = "Attack";
        const string DefendTrigger = "Defend";
        const string DodgeTrigger = "Dodge";


        bool canAttack;

        void Awake()
        {
            unarmedWeapon = Resources.Load<ScriptableWeapon>(UNARMED_WEAPON_PATH);
        }

        void Update()
        {
            if (currentWeapon == null)
            {
                EquipUnarmed();
            }
        }

        // Helpers
        void EquipUnarmed()
        {
            UnequipWeapon();

            currentWeapon = unarmedWeapon;
            SpawnWeapon();
        }

        // Setters
        public void SetOwner(GameObject owner)
        {
            weaponOwner = owner;
        }

        public void UnequipWeapon()
        {
            // Remove listeners
            if (hitbox != null)
            {
                hitbox.OnHit.RemoveListener(HitTarget);
            }

            // Destroy old weapon gameobject instance
            if (weaponGameObject != null)
            {
                Destroy(weaponGameObject);
            }

            // Cleanup references
            hitbox = null;
            weaponGameObject = null;
            currentWeapon = null;
        }

        public void EquipWeapon(ScriptableWeapon weaponToEquip)
        {
            UnequipWeapon();

            currentWeapon = weaponToEquip;
            SpawnWeapon();
        }

        // Getters
        public ScriptableWeapon GetEquippedWeapon()
        {
            return currentWeapon;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        // PUBLIC METHODS
        public void Attack(AudioClip gruntAudioClip)
        {

            // Owner of weapon has enough stamina to perform next attack?
            Stamina ownerStamina = weaponOwner.GetComponent<Stamina>();

            canAttack =
                (ownerStamina != null && currentWeapon != null && ownerStamina.HasStaminaAgainstCostAction(currentWeapon.staminaCost) )
                || (battler != null && !battler.IsTakingDamage())
            ;

            if (!canAttack)
            {
                return;
            }

            ownerStamina.DecreaseStamina(currentWeapon.staminaCost * 100f);

            // Owner Animation
            weaponOwner.GetComponent<Animator>().SetTrigger(AttackTrigger);

            StartCoroutine(HandleAttack());
            StartCoroutine(HandleAttackSound());
            StartCoroutine(HandleGrunt(gruntAudioClip));
        }

        public void Defend()
        {
            /*
           // Owner of weapon has enough stamina to perform next attack?
            Stamina ownerStamina = weaponOwner.GetComponent<Stamina>();

            bool canAttack = 
                (ownerStamina != null && 
                    ownerStamina.HasStaminaAgainstCostAction(currentWeapon.staminaCost)
                )
                || !battler.IsTakingDamage()
            ;

            if (!canAttack)
            {
                return;
            }
            
            ownerStamina.DecreaseStamina(currentWeapon.staminaCost * 100f);

            
            */


            StartCoroutine(HandleDefenseSound());
        }

        // PRIVATE METHODS
        IEnumerator HandleAttack()
        {
            yield return new WaitForSeconds(GetHitboxActivationTime());
            hitbox.Enable();
            yield return new WaitForSeconds(GetHitboxLifespan());
            hitbox.Disable();
        }

        IEnumerator HandleAttackSound()
        {
            foreach (Soundclip soundclip in currentWeapon.soundclips)
            {
                yield return new WaitForSeconds(soundclip.timeToTrigger);
                if (canAttack) {
                    audioSource.PlayOneShot(soundclip.clip);
                }
            }
        }

        IEnumerator HandleDefenseSound()
        {
            foreach (Soundclip soundclip in currentWeapon.defenseSoundClips)
            {
                yield return new WaitForSeconds(soundclip.timeToTrigger);
                audioSource.PlayOneShot(soundclip.clip);
            }
        }

        IEnumerator HandleGrunt(AudioClip gruntAudioClip)
        {
            yield return new WaitForSeconds(currentWeapon.gruntTimeToTrigger);
            audioSource.PlayOneShot(gruntAudioClip);
        }

        void SpawnWeapon() {
            weaponGameObject = Instantiate(currentWeapon.weaponPrefab, this.transform);

            // Assign damage amount to the prefab hitbox
            hitbox = GetHitbox();

            // Subscribe to Hit Event available on Hitbox
            hitbox.OnHit.RemoveAllListeners();
            hitbox.OnHit.AddListener(HitTarget);
        }

        void HitTarget()
        {
            AI_Core_V3 targetAi = hitbox.target.GetComponent<AI_Core_V3>();

            // If I am player, I don't want to hit npcs or player friends
            if (weaponOwner.tag == "Player" && targetAi.alliance != ALLIANCE.ENEMY)
            {
                return;
            }

            // If target is not Player
            if (targetAi != null)
            {
                targetAi.SetTarget(weaponOwner);

                // Decide here if we take the damage
                float chance = UnityEngine.Random.Range(0, 1f);

                if (chance >= targetAi.minimumChanceToDefend)
                {

                    // Get parry sfx from our health owner
                    AudioClip parryClip = targetAi.GetComponent<WeaponManager>().weaponSlots[0].currentWeapon.parryHitSFX;
                    
                    if (parryClip)
                    {
                        audioSource.PlayOneShot(parryClip);
                    }

                    targetAi.SetState(AGENT_STATE.DEFEND);
                    return;
                }

                if (chance >= targetAi.minimumChanceToDodge)
                {
                    // Get parry sfx from our health owner
                    AudioClip dodgeClip = targetAi.GetComponent<Battler>().dodgeClip;
                    
                    if (dodgeClip)
                    {
                        audioSource.PlayOneShot(dodgeClip);
                    }

                    targetAi.SetState(AGENT_STATE.DODGE);
                    return;
                }
            }


            // Calculate damage (mix of weapon and characterStats)
            float damage = currentWeapon.weaponDamage;

            damage += weaponOwner.GetComponent<BaseStats>().GetAttack();
            
            Debug.Log("Character current attack power: "+ weaponOwner.GetComponent<BaseStats>().GetAttack());
            Debug.Log("Damage applied: " + damage);

            // Apply damage to hitbox current target
            hitbox.target.GetComponent<Health>().TakeDamage(damage, weaponOwner);

            // Play SFX
            if (hitbox.target.GetComponent<Battler>().IsDefending())
            {
                // Get target equipped weapon to know what the impact sound should sound like
                audioSource.PlayOneShot(hitbox.target.GetComponent<WeaponManager>().weaponSlots[0].currentWeapon.parryHitSFX);
                audioSource.PlayOneShot(currentWeapon.hitSFX);

                return;
            }

            audioSource.PlayOneShot(currentWeapon.hitSFX);
            // Get center position of target based on their capsule collider
            Vector3 targetCenter = hitbox.target.GetComponent<CapsuleCollider>().bounds.center;

            // Add FX
            Instantiate(currentWeapon.particlePrefab, targetCenter, Quaternion.identity);
        }

        float GetHitboxActivationTime()
        {
            return currentWeapon.hitboxActivationTime;
        }

        float GetHitboxLifespan()
        {
            return currentWeapon.hitboxLifespan;
        }

        Hitbox GetHitbox()
        {
            Hitbox _h = null;
            
            if (weaponGameObject.GetComponent<WeaponReferences>() != null)
            {
                _h = weaponGameObject.GetComponent<WeaponReferences>().hitbox.GetComponent<Hitbox>();
            }

            // TOOD: Improve this but not right now because its 01:30 am


            // If hitbox is not on parent, search inside
            if (_h == null)
            {
                _h = weaponGameObject.transform.GetChild(0).GetComponent<WeaponReferences>().hitbox.GetComponent<Hitbox>();
            }

            return _h;
        }
    }
}
