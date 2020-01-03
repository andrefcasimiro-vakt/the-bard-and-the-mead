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

        Animator animator => GetComponent<Animator>();
        WeaponManager weaponManager => GetComponent<WeaponManager>();

        // Animator Parameter Keys
        const string AttackTrigger = "Attack";
        const string DefendTrigger = "Defend";
        const string DodgeTrigger = "Dodge";

        public void Attack()
        {
            animator.SetTrigger(AttackTrigger);

            weaponManager.Attack(gruntAudioClip);
        }

        public void Defend()
        {
            print("Defend");
            animator.SetTrigger(DefendTrigger);
        }

        public void Dodge()
        {
            print("Dodge");
            animator.SetTrigger(DodgeTrigger);
        }





    }
}
