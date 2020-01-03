using UnityEngine;
using System.Collections.Generic;

namespace RPG.Weapon {

    [System.Serializable]
    public class Soundclip
    {
        public string key;
        public float timeToTrigger;
        public AudioClip clip;
    }

    [CreateAssetMenu(fileName = "Weapons", menuName = "Weapons/New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [Header("Stats")]
        public float weaponDamage = 1f;
        public float weaponRange = 1f;

        [Header("Animations")]
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        public float hitboxActivationTime = 0f;
        public float hitboxLifespan = 0.2f;

        [Header("Prefab")]
        public GameObject weaponPrefab;

        [Header("Audioclips")]
        public List<Soundclip> soundclips = new List<Soundclip>();

        // The time to trigger the grunt of the owner who wields the weapon
        // This is done so the owner's grunt can know when to match with the animation of this weapon
        public float gruntTimeToTrigger = 0.1f;

        public AudioClip hitSFX;


        [Header("Particles")]
        public GameObject particlePrefab;
    }
}
