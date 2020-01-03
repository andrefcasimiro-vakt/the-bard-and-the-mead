using UnityEngine;

namespace RPG.Weapon {

    public class WeaponManager: MonoBehaviour {

        [Header("Weapon Slot")]
        public WeaponSlot[] weaponSlots;

        public void Attack(
            AudioClip gruntAudioClip
        )
        {
            foreach(WeaponSlot weaponSlot in weaponSlots)
            {
                weaponSlot.Attack(gruntAudioClip);
            }
        }


    }

}
