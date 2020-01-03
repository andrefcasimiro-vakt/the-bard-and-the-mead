using System.Collections;
using UnityEngine;
using RPG.Core;

namespace RPG.Weapon {

    [RequireComponent(typeof(AudioSource))]
    [System.Serializable]
    public class WeaponSlot: MonoBehaviour {

        public string slotName = "";
        public Weapon currentWeapon = null;
        // In world stored reference
        GameObject weaponGameObject = null;

        // Path to the unarmed weapon scriptable object in Database/Resources/Weapons folder
        const string UNARMED_WEAPON_PATH = "Weapons/Unarmed";
        Weapon unarmedWeapon = null;


        Hitbox hitbox = null;
        AudioSource audioSource => GetComponent<AudioSource>();

        void Awake()
        {
            unarmedWeapon = Resources.Load<Weapon>(UNARMED_WEAPON_PATH);
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

        public void EquipWeapon(Weapon weaponToEquip)
        {
            UnequipWeapon();

            currentWeapon = weaponToEquip;
            SpawnWeapon();
        }

        // Getters
        public Weapon GetEquippedWeapon()
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
            StartCoroutine(HandleAttack());
            StartCoroutine(HandleSound());
            StartCoroutine(HandleGrunt(gruntAudioClip));
        }

        // PRIVATE METHODS
        IEnumerator HandleAttack()
        {
            yield return new WaitForSeconds(GetHitboxActivationTime());
            hitbox.Enable();
            yield return new WaitForSeconds(GetHitboxLifespan());
            hitbox.Disable();
        }

        IEnumerator HandleSound()
        {
            foreach (Soundclip soundclip in currentWeapon.soundclips)
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
            weaponGameObject = Instantiate(currentWeapon.weaponPrefab, this.transform.position, Quaternion.identity, this.transform);

            // Assign damage amount to the prefab hitbox
            hitbox = GetHitbox();

            // Subscribe to Hit Event available on Hitbox
            hitbox.OnHit.AddListener(HitTarget);
        }

        void HitTarget()
        {
            // Apply damage to hitbox current target
            hitbox.target.GetComponent<Health>().TakeDamage(currentWeapon.weaponDamage);

            // Get center position of target based on their capsule collider
            Vector3 targetCenter = hitbox.target.GetComponent<CapsuleCollider>().bounds.center;

            // Add FX
            Instantiate(currentWeapon.particlePrefab, targetCenter, Quaternion.identity);

            // Play SFX
            audioSource.PlayOneShot(currentWeapon.hitSFX);
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
            return weaponGameObject.GetComponent<WeaponReferences>().hitbox.GetComponent<Hitbox>();
        }
    }
}
