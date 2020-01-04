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


    }

}
