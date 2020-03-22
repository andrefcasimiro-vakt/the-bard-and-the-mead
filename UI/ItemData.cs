using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RPG.Inventory;

namespace RPG.UI {
    public class ItemData : MonoBehaviour {

        public GameObject itemOwner;
        public GameObject inventoryUI;
        public ScriptableItem equippedItem;

        void Start()
        {
        }
    
        public void RemoveItem()
        {
            itemOwner.GetComponent<CharacterInventory>().Remove(equippedItem);

            // Redraw inventory
            inventoryUI.GetComponent<InventoryManager>().Draw();

            if (equippedItem.droppedInstance == null) return;

            // Instantiate droppable graphic
            Instantiate(equippedItem.droppedInstance, Camera.main.transform.position +  Vector3.forward, Quaternion.identity);
        }

    }
}
