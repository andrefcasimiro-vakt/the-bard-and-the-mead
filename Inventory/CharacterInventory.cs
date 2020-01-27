using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Inventory
{

    public class CharacterInventory : MonoBehaviour, ISaveable
    {

        const string RESOURCE_ITEMS_PREFIX = "Items/";
        const string RESOURCE_WEAPONS_PREFIX = "Weapons/";
        const string RESOURCE_EQUIPMENTS_PREFIX = "Equipments/";

        public List<ScriptableItem> inventory = new List<ScriptableItem>();


        // Public Methods

        public void Add(ScriptableItem itemToAdd)
        {
            inventory.Add(itemToAdd);
        }

        public void Remove(ScriptableItem itemToRemove)
        {
            int index = inventory.FindLastIndex(i => i == itemToRemove);
            inventory.RemoveAt(index);

        }


        // Saving System Helpers

        private void LoadItems(string[] itemNames)
        {
            foreach (string itemName in itemNames)
            {
                ScriptableItem _item = Resources.Load<ScriptableItem>(RESOURCE_ITEMS_PREFIX + itemName);
                inventory.Add(_item);
            }
        }

        private void LoadWeapons(string[] weaponNames)
        {
            foreach (string weaponName in weaponNames)
            {
                ScriptableItem _weapon = Resources.Load<ScriptableItem>(RESOURCE_WEAPONS_PREFIX + weaponName);
                inventory.Add(_weapon);
            }
        }

        private SaveableInventory CreateSaveableInventory()
        {
            List<string> items = new List<string>();
            List<string> weapons = new List<string>();

            foreach (ScriptableItem item in inventory)
            {
                if (item.itemType == ItemEnum.ITEM)
                {
                    items.Add(item.name);
                    continue;
                }

                if (item.itemType == ItemEnum.WEAPON)
                {
                    weapons.Add(item.name);
                    continue;
                }
            }

            return new SaveableInventory(items.ToArray(), weapons.ToArray());
        }

        public object CaptureState()
        {
            return CreateSaveableInventory();
        }

        public void RestoreState(object state)
        {
            SaveableInventory savedItems = (SaveableInventory)state;

            inventory.Clear();

            LoadItems(savedItems.items);
            LoadWeapons(savedItems.weapons);
        }
    }

    [System.Serializable]
    public class SaveableInventory
    {
        public string[] items;
        public string[] weapons;

        public SaveableInventory(string[] items, string[] weapons)
        {
            this.items = items;
            this.weapons = weapons;
        }
    }
}
