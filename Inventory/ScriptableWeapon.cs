using UnityEngine;
using System.Collections.Generic;

namespace RPG.Inventory {

    [System.Serializable]
    public class Soundclip
    {
        public string key;
        public float timeToTrigger;
        public AudioClip clip;
    }

    [CreateAssetMenu(fileName = "Weapons", menuName = "Weapons/New Weapon", order = 0)]
    public class ScriptableWeapon : ScriptableEquipment
    {
        [Header("Stats")]
        public float weaponDamage = 1f;
        public float weaponRange = 1f;
        public float staminaCost = 3f; // Stamina cost per attack

        public float hitboxActivationTime = 0f;
        public float hitboxLifespan = 0.2f;

        [Header("Audioclips")]
        public List<Soundclip> soundclips = new List<Soundclip>();
        public List<Soundclip> defenseSoundClips = new List<Soundclip>();

        [Header("---")]

        // The time to trigger the grunt of the owner who wields the weapon
        // This is done so the owner's grunt can know when to match with the animation of this weapon
        public float gruntTimeToTrigger = 0.1f;

        public AudioClip hitSFX;

        // If is a weapon, can be a metallic sound when impact happens
        public AudioClip parryHitSFX;

        [Header("Particles")]
        public GameObject particlePrefab;

    }
}
