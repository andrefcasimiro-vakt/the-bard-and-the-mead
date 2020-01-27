using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Control;

namespace RPG.Inventory
{
    public class InventoryManager : MonoBehaviour
    {

        [Header("Player Settings")]
        [SerializeField]
        public KeyCode inventoryKey;
        [SerializeField]
        GameObject inventoryUI;

        [Header("Inventory UI")]
        [SerializeField] GameObject slotPanel;
        [SerializeField] GameObject itemButtonPrefab;

        // A way to identify each child of each button slot
        const string UI_INVENTORY_EQUIPPED = "UI_Inventory_Equipped";
        const string UI_INVENTORY_STACK = "UI_Inventory_Stack";
        const string UI_INVENTORY_IMAGE = "UI_Inventory_Image";

        GameObject player;

        // The player or an npc inventory system
        CharacterInventory currentCharacterInventory;
        List<ScriptableItem> inventory;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            inventoryUI.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(inventoryKey))
            {

                // By default, open the player inventory
                if (!inventoryUI.activeSelf)
                {
                    Open(player.GetComponent<CharacterInventory>());
                }

                inventoryUI.SetActive(!inventoryUI.activeSelf);

                HandleSystem();
            }

        }

        void HandleSystem()
        {
            player.SetActive(!inventoryUI.activeSelf);

            if (inventoryUI.activeSelf)
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

            }
            else
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        // Inventory GUI
        public void Open(CharacterInventory characterInventory)
        {
            this.currentCharacterInventory = characterInventory;
            this.inventory = characterInventory.inventory;

            Draw();
        }

        void Draw()
        {
            // Clean panel first
            foreach (Transform child in slotPanel.transform)
            {
                Destroy(child.gameObject);
            }

            // Store reference for performance
            List<ScriptableItem> visited = new List<ScriptableItem>();
            List<GameObject> visitedButtons = new List<GameObject>();

            foreach (ScriptableItem item in inventory)
            {
                GameObject itemButton = null;

                int visitedIndex = item.stackable ? visited.FindIndex(entry => entry.name == item.name) : -1;

                // Check if visited contains this item
                if (visitedIndex != -1)
                {
                    GameObject stackCounter = FindNestedGameObjectByTag(visitedButtons[visitedIndex], UI_INVENTORY_STACK);
                    itemButton = visitedButtons[visitedIndex];

                    // Count all existing items so far
                    int count = inventory.FindAll(inventoryItem => inventoryItem.name == visited[visitedIndex].name).Count;
                    stackCounter.GetComponent<Text>().text = "x" + count;
                }
                else
                {
                    // Register item in visited list for stackable cases
                    visited.Add(item);

                    // Instantiate Item Button
                    itemButton = Instantiate(itemButtonPrefab);
                    visitedButtons.Add(itemButton);
                }

                // Icon imagery logic
                GameObject sprite = FindNestedGameObjectByTag(itemButton, UI_INVENTORY_IMAGE);
                if (sprite != null)
                {
                    sprite.GetComponent<Image>().sprite = item.itemSprite;
                }

                // Parent item button to panel
                itemButton.transform.SetParent(slotPanel.transform);

                if (item.itemType == ItemEnum.ITEM)
                {
                    itemButton.GetComponent<Button>().onClick.RemoveAllListeners();
                    itemButton.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        item.Consume(currentCharacterInventory.gameObject);

                        print("clciked");

                        // Refresh inventory panel
                        Draw();
                    });
                }

                if (item.itemType == ItemEnum.EQUIPMENT)
                {
                    itemButton.GetComponent<Button>().onClick.RemoveAllListeners();
                    itemButton.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        ScriptableEquipment equipment = (ScriptableEquipment)item;
                        equipment.Equip(currentCharacterInventory.gameObject);
                    });
                }

                /*
                // @ Equippable Item Logic
                if (item.graphic != null)
                {
                    // @ If item is already equipped, show Equipped banner
                    ScriptableItem equippedItem = equipmentManager.GetEquippedItem(item.slot);

                    GameObject equippedBanner = FindNestedGameObjectByTag(itemButton, Constants.UI_INVENTORY_EQUIPPED);
                    equippedBanner.SetActive(false);

                    if (equippedItem == item)
                    {
                        // Search for the Equipped Banner and activate it
                        equippedBanner?.SetActive(true);

                    }

                    itemButton.GetComponent<Button>().onClick.AddListener(() => {

                        // @Is Item equipped already?
                        if (equippedItem == item)
                        {
                            equipmentManager.Unequip(item.slot);
                            equippedBanner?.SetActive(false);
                        }
                        else
                        {
                            equipmentManager.Equip(item.slot, item);
                            equippedBanner?.SetActive(true);
                        }

                        // Redraw
                        Draw();

                    });
                }*/

            }

        }

        private GameObject FindNestedGameObjectByTag(GameObject parent, string _tag)
        {
            foreach (Transform child in parent.transform)
            {
                if (child.gameObject.tag == _tag)
                {
                    return child.gameObject;
                }
            }

            return null;
        }

    }

}
