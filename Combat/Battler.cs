using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Weapon;

namespace RPG.Combat {

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(WeaponManager))]
    public class Battler : MonoBehaviour
    {
        [Header("Battler Audio")]
        public AudioClip gruntAudioClip;

        WeaponManager weaponManager => GetComponent<WeaponManager>();

        const int COMBAT_LAYER_INDEX = 1;

        public void Attack()
        {
            weaponManager.Attack(gruntAudioClip);
        }

        public void Defend()
        {
            print("Defend");
        }

        public void Dodge()
        {
            print("Dodge");
        }

        // Getters
        public bool IsAttacking()
        {
            return GetComponent<Animator>().GetCurrentAnimatorStateInfo(COMBAT_LAYER_INDEX).IsTag("Attack"); ;
        }
        public bool IsDefending()
        {
            return GetComponent<Animator>().GetCurrentAnimatorStateInfo(COMBAT_LAYER_INDEX).IsTag("Defend"); ;
        }
        public bool IsDodging()
        {
            return GetComponent<Animator>().GetCurrentAnimatorStateInfo(COMBAT_LAYER_INDEX).IsTag("Dodge"); ;
        }
    }
}
