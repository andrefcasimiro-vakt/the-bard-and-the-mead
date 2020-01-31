using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Control;
using RPG.Character;

namespace RPG.Inventory
{

    [System.Serializable]
    public class Slot
    {
        public Image image;
        public ScriptableEquipment equipment;

        public Slot(Image image, ScriptableEquipment equipment)
        {
            this.image = image;
            this.equipment = equipment;
        }
    }

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
        GameObject inventoryOwner;
        List<ScriptableItem> inventory;

        // Equipment Slots
        public Slot head;
        public Slot torso;
        public Slot leftUpperArm;
        public Slot leftLowerArm;
        public Slot leftHand;
        public Slot rightUpperArm;
        public Slot rightLowerArm;
        public Slot rightHand;
        public Slot hips;
        public Slot leftLeg;
        public Slot rightLeg;

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
            this.inventoryOwner = characterInventory.gameObject;
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

                Button itemBtn = itemButton.GetComponent<Button>();
                itemBtn.onClick.RemoveAllListeners();

                if (item.itemType == ItemEnum.ITEM)
                {
                    itemBtn.onClick.AddListener(() =>
                    {
                        item.Consume(inventoryOwner);

                        print("clciked");

                        // Refresh inventory panel
                        Draw();
                    });
                }

                if (item.itemType == ItemEnum.EQUIPMENT)
                {
                    ScriptableEquipment equipment = (ScriptableEquipment)item;

                    bool itemIsEquipped = GetSlot(equipment.bodyPart).equipment == equipment;
                    FindNestedGameObjectByTag(itemButton, UI_INVENTORY_EQUIPPED).GetComponent<Image>().enabled = itemIsEquipped;

                    itemBtn.onClick.AddListener(() =>
                    {
                       
                        Slot slot = GetSlot(equipment.bodyPart);
                        FindNestedGameObjectByTag(itemButton, UI_INVENTORY_EQUIPPED).GetComponent<Image>().enabled = false;

                        // Item is already equipped?
                        if (slot.equipment == equipment)
                        {
                            equipment.Unequip(inventoryOwner);
                            slot.image.sprite = null;
                            slot.equipment = null;
                            return;
                        }

                        // Unequip current slot before attempting to equip new item
                        if (slot.equipment != null)
                        {
                            slot.equipment.Unequip(inventoryOwner);
                            slot.equipment = null;
                        }

                        // Slot is free. Equip item
                        equipment.Equip(inventoryOwner);

                        // Update Equipment Panel Sloot Sprite
                        slot.image.sprite = equipment.itemSprite;
                        slot.equipment = equipment;

                        FindNestedGameObjectByTag(itemButton, UI_INVENTORY_EQUIPPED).GetComponent<Image>().enabled = true;

                        // Update UI to update UI_INVENTORY_EQUIPPED borders for active items
                        Draw();
                    });
                }
            }
        }

        Slot GetSlot (BodyPart bodyPart)
        {
            switch (bodyPart)
            {
                case BodyPart.Head:
                    return head;
                case BodyPart.Torso:
                    return torso;
                case BodyPart.ArmUpperRight:
                    return rightUpperArm;
                case BodyPart.ArmLowerRight:
                    return rightLowerArm;
                case BodyPart.RightHand:
                    return rightHand;
                case BodyPart.ArmUpperLeft:
                    return leftUpperArm; 
                case BodyPart.ArmLowerLeft:
                    return leftLowerArm;
                case BodyPart.LeftHand:
                    return leftHand;
                case BodyPart.Hips:
                    return hips;
                case BodyPart.RightLeg:
                    return rightLeg;
                case BodyPart.LeftLeg:
                    return leftLeg;
                default:
                    return null;
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
