using UnityEngine;
using System.Collections;
using RPG.Saving;

namespace RPG.Inventory
{
    public class PickableItem : MonoBehaviour, ISaveable
    {

        [SerializeField]
        ScriptableItem[] itemsToPick;

        public bool isCollected = false;

        public bool equipOnPickup = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                foreach (ScriptableItem item in itemsToPick)
                {
                    other.GetComponent<CharacterInventory>().Add(item);

                    ScriptableEquipment equipment = item as ScriptableEquipment;

                    if (equipment != null) {
                        if (equipOnPickup) {
                            equipment.Equip(other.gameObject);
                        }
                    }
                }

                isCollected = true;

                Deactivate();
            }
        }

        void Deactivate()
        {
            // If item was collected, deactivate this gameObject
            GetComponent<SphereCollider>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }

        public object CaptureState()
        {
            return isCollected;
        }

        public void RestoreState(object state)
        {
            bool collected = (bool)state;

            isCollected = collected;

            if (collected == true)
            {
                // If item was collected, deactivate this gameObject
                Deactivate();
            }
        }

        public void OnCleanState() {
            
        }
    }

}
