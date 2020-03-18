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
    
        public void RemoveItem()
        {
            itemOwner.GetComponent<CharacterInventory>().Remove(equippedItem);

            // Redraw inventory
            inventoryUI.GetComponent<InventoryManager>().Draw();
        }

    }
}
