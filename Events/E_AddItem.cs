using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.Inventory;

namespace RPG.Events {

    [System.Serializable]
    public class E_AddItem : MonoBehaviour
    {
        public GameObject target;
        public ScriptableItem item;

        public void AddItem()
        {
            CharacterInventory targetInventory = target.GetComponent<CharacterInventory>();

            if (targetInventory == null)
            {
                throw new System.Exception("No inventory found for gameObject: " + target.name);
            }

            targetInventory.Add(item);
        }

    }
}
