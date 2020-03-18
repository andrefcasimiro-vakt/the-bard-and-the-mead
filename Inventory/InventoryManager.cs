using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Control;
using RPG.Character;
using RPG.UI;

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
        CharacterEquipmentSlot currentCharacterEquipmentSlot;
        GameObject inventoryOwner;
        List<ScriptableItem> inventory;

        [Header("Equipment Panel UI")]
        // Equipment Slots
        public GameObject headSlotBTN;
        public GameObject torsoSlotBTN;
        public GameObject leftUpperArmSlotBTN;
        public GameObject leftLowerArmSlotBTN;
        public GameObject leftHandSlotBTN;
        public GameObject rightUpperArmSlotBTN;
        public GameObject rightLowerArmSlotBTN;
        public GameObject rightHandSlotBTN;
        public GameObject hipsSlotBTN;
        public GameObject leftLegSlotBTN;
        public GameObject rightLegSlotBTN;
        
        public GameObject leftShoulderBTN;
        public GameObject rightShoulderBTN;
        public GameObject accessoryBTN;
        public GameObject shieldBTN;
        public GameObject weaponSlotBTN;

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
            this.currentCharacterEquipmentSlot = characterInventory.gameObject.GetComponent<CharacterEquipmentSlot>();
            this.inventoryOwner = characterInventory.gameObject;
            this.inventory = characterInventory.inventory;

            Draw();
        }

        public void Draw()
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


                        // Refresh inventory panel
                        Draw();
                    });
                }

                if (item.itemType == ItemEnum.EQUIPMENT || item.itemType == ItemEnum.WEAPON)
                {
                    var equipment = new ScriptableEquipment();

                    if (item.itemType == ItemEnum.EQUIPMENT)
                        equipment = (ScriptableEquipment)item;

                    if (item.itemType == ItemEnum.WEAPON)
                        equipment = (ScriptableWeapon)item;
                    

                    bool itemIsEquipped = currentCharacterEquipmentSlot.GetSlot(equipment.bodyPart)?.equipment == equipment;

                    // Look for sprite gameobject to update its sprite
                    GetSlotButton(equipment.bodyPart).transform.GetChild(2).GetComponent<Image>().sprite = itemIsEquipped ? equipment.itemSprite : null;

                    FindNestedGameObjectByTag(itemButton, UI_INVENTORY_EQUIPPED).GetComponent<Image>().enabled = itemIsEquipped;

                    // Add item data to draggable slot
                    itemBtn.GetComponent<ItemData>().itemOwner = this.inventoryOwner;
                    itemBtn.GetComponent<ItemData>().inventoryUI = this.gameObject;
                    itemBtn.GetComponent<ItemData>().equippedItem = equipment;

                    itemBtn.onClick.AddListener(() =>
                    {
                       
                        Slot slot = currentCharacterEquipmentSlot.GetSlot(equipment.bodyPart);
                        FindNestedGameObjectByTag(itemButton, UI_INVENTORY_EQUIPPED).GetComponent<Image>().enabled = false;

                        // Item is already equipped?
                        if (slot?.equipment == equipment)
                        {
                            Debug.Log("unequipping: " + equipment.name);
                            equipment.Unequip(inventoryOwner);
                            Draw();
                            return;
                        }

                        // Unequip current slot before attempting to equip new item
                        if (slot?.equipment != null)
                        {
                            slot.equipment.Unequip(inventoryOwner);
                        }

                        // Slot is free. Equip item
                        equipment.Equip(inventoryOwner);

                        FindNestedGameObjectByTag(itemButton, UI_INVENTORY_EQUIPPED).GetComponent<Image>().enabled = true;

                        // Update UI to update UI_INVENTORY_EQUIPPED borders for active items
                        Draw();
                    });
                }
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

        public GameObject GetSlotButton (BodyPart bodyPart)
        {
            switch (bodyPart)
            {
                case BodyPart.Head:
                    return headSlotBTN;
                case BodyPart.Torso:
                    return torsoSlotBTN;
                case BodyPart.ArmUpperRight:
                    return rightUpperArmSlotBTN;
                case BodyPart.ArmLowerRight:
                    return rightLowerArmSlotBTN;
                case BodyPart.RightHand:
                    return rightHandSlotBTN;
                case BodyPart.ArmUpperLeft:
                    return leftUpperArmSlotBTN; 
                case BodyPart.ArmLowerLeft:
                    return leftLowerArmSlotBTN;
                case BodyPart.LeftHand:
                    return leftHandSlotBTN;
                case BodyPart.Hips:
                    return hipsSlotBTN;
                case BodyPart.RightLeg:
                    return rightLegSlotBTN;
                case BodyPart.LeftLeg:
                    return leftLegSlotBTN;
                case BodyPart.LeftShoulder:
                    return leftShoulderBTN;
                case BodyPart.RightShoulder:
                    return rightShoulderBTN;
                case BodyPart.Shield:
                    return weaponSlotBTN;
                case BodyPart.RightHandWeapon:
                    return weaponSlotBTN;
                case BodyPart.Accessory:
                    return accessoryBTN;
                default:
                    return null;
            }
        }

    }

}
