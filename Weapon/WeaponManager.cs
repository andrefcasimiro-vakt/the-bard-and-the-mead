using UnityEngine;

namespace RPG.Weapon {

    public class WeaponManager: MonoBehaviour {

        [Header("Weapon Slot")]
        public WeaponSlot[] weaponSlots;

        private void Start()
        {
            foreach(WeaponSlot weaponSlot in weaponSlots)
            {
                weaponSlot.SetOwner(this.gameObject);
            }
        }

        public void Attack(
            AudioClip gruntAudioClip
        )
        {
            foreach(WeaponSlot weaponSlot in weaponSlots)
            {
                weaponSlot.Attack(gruntAudioClip);
            }
        }

        // Combine all weaponRanges from equipped weapons to determine how far from the target should we be
        public float GetStoppingDistance()
        {
            float distance = 0f;

            foreach (WeaponSlot weaponSlot in weaponSlots)
            {
                distance += weaponSlot.currentWeapon.weaponRange;
            }


            return distance / weaponSlots.Length;
        }
    }
}
