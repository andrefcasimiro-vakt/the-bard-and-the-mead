using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using RPG.Saving;
using RPG.Character;

namespace RPG.Inventory
{

    [System.Serializable]
    public class Slot
    {
        public ScriptableEquipment equipment;

        public Slot(ScriptableEquipment equipment)
        {
            this.equipment = equipment;
        }
    }

    /// Holds the current equipped slots of a given character

    public class CharacterEquipmentSlot : MonoBehaviour, ISaveable
    {

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

        public Slot leftWeapon;
        public Slot rightWeapon;

        public float GetArmorBonus()
        {
            return
            (
                // Whenever I have time, I should probably unregret this
                (head != null && head.equipment ? head.equipment.defenseRate : 0f)
                + (torso != null && torso.equipment ? torso.equipment.defenseRate : 0f)
                + (leftUpperArm != null && leftUpperArm.equipment ? leftUpperArm.equipment.defenseRate  : 0f)
                + (leftLowerArm != null && leftLowerArm.equipment ? leftLowerArm.equipment.defenseRate  : 0f)
                + (leftHand != null && leftHand.equipment ? leftHand.equipment.defenseRate  : 0f)
                + (rightUpperArm != null && rightUpperArm.equipment ? rightUpperArm.equipment.defenseRate : 0f)
                + (rightLowerArm != null && rightLowerArm.equipment ? rightLowerArm.equipment.defenseRate : 0f)
                + (rightHand != null && rightHand.equipment ? rightHand.equipment.defenseRate : 0f)
                + (hips != null && hips.equipment ? hips.equipment.defenseRate : 0f)
                + (leftLeg != null && leftLeg.equipment ? leftLeg.equipment.defenseRate : 0f)
                + (rightLeg != null && rightLeg.equipment ?  rightLeg.equipment.defenseRate : 0f)
            );
        }

        public void EquipOnSlot (BodyPart bodyPart, ScriptableEquipment equipment)
        {
            switch (bodyPart)
            {
                case BodyPart.Head:
                    head = null;
                    head = new Slot(equipment);
                    break;
                case BodyPart.Torso:
                    torso = null;
                    torso = new Slot(equipment);
                    break;
                case BodyPart.ArmUpperRight:
                    rightUpperArm = null;
                    rightUpperArm = new Slot(equipment);
                    break;
                case BodyPart.ArmLowerRight:
                    rightLowerArm = null;
                    rightLowerArm = new Slot(equipment);
                    break;
                case BodyPart.RightHand:
                    rightHand = null;
                    rightHand = new Slot(equipment);
                    break;
                case BodyPart.ArmUpperLeft:
                    leftUpperArm = null;
                    leftUpperArm = new Slot(equipment);
                    break;
                case BodyPart.ArmLowerLeft:
                    leftLowerArm = null;
                    leftLowerArm = new Slot(equipment);
                    break;
                case BodyPart.LeftHand:
                    leftHand = null;
                    leftHand = new Slot(equipment);
                    break;
                case BodyPart.Hips:
                    hips = null;
                    hips = new Slot(equipment);
                    break;
                case BodyPart.RightLeg:
                    rightLeg = null;
                    rightLeg = new Slot(equipment);
                    break;
                case BodyPart.LeftLeg:
                    leftLeg = null;
                    leftLeg = new Slot(equipment);
                    break;
                case BodyPart.RightHandWeapon:
                    rightWeapon = null;
                    rightWeapon = new Slot(equipment);
                    break;
                default:
                    break;
            }
        }

        public Slot GetSlot (BodyPart bodyPart)
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
                case BodyPart.RightHandWeapon:
                    return rightWeapon;
                default:
                    return null;
            }
        }

        // Saving / Loading slots logic

        private SaveableSlot[] CreateSaveableSlots()
        {
            SaveableSlot s_head = new SaveableSlot("head", head?.equipment?.itemName);
            SaveableSlot s_torso = new SaveableSlot("torso", torso?.equipment?.itemName);
            SaveableSlot s_leftUpperArm = new SaveableSlot("leftUpperArm", leftUpperArm?.equipment?.itemName);
            SaveableSlot s_leftLowerArm = new SaveableSlot("leftLowerArm", leftLowerArm?.equipment?.itemName);
            SaveableSlot s_leftHand = new SaveableSlot("leftHand", leftHand?.equipment?.itemName);
            SaveableSlot s_rightUpperArm = new SaveableSlot("rightUpperArm", rightUpperArm?.equipment?.itemName);
            SaveableSlot s_rightLowerArm = new SaveableSlot("rightLowerArm", rightLowerArm?.equipment?.itemName);
            SaveableSlot s_rightHand = new SaveableSlot("rightHand", rightHand?.equipment?.itemName);
            SaveableSlot s_hips = new SaveableSlot("hips", hips?.equipment?.itemName);
            SaveableSlot s_leftLeg = new SaveableSlot("leftLeg", leftLeg?.equipment?.itemName);
            SaveableSlot s_rightLeg = new SaveableSlot("rightLeg", rightLeg?.equipment?.itemName);
            
            SaveableSlot s_leftWeapon = new SaveableSlot("leftWeapon", leftWeapon?.equipment?.itemName);
            SaveableSlot s_rightWeapon = new SaveableSlot("rightWeapon", rightWeapon?.equipment?.itemName);
            
            List<SaveableSlot> s_slots = new List<SaveableSlot>();
            s_slots.Add(s_head);
            s_slots.Add(s_torso);
            s_slots.Add(s_leftUpperArm);
            s_slots.Add(s_leftLowerArm);
            s_slots.Add(s_leftHand);
            s_slots.Add(s_rightUpperArm);
            s_slots.Add(s_rightLowerArm);
            s_slots.Add(s_rightHand);
            s_slots.Add(s_hips);
            s_slots.Add(s_leftLeg);
            s_slots.Add(s_rightLeg);
            s_slots.Add(s_leftWeapon);
            s_slots.Add(s_rightWeapon);

            return s_slots.ToArray();
        }

        public object CaptureState()
        {
            return CreateSaveableSlots();
        }

        public void RestoreState(object state)
        {
            SaveableSlot[] savedSlots = (SaveableSlot[])state;

            LoadSlots(savedSlots);
        }

        public void OnCleanState() {
            
        }

        private void LoadSlots(SaveableSlot[] savedSlots) {
            head = null;
            torso = null;
            leftUpperArm = null;
            leftLowerArm = null;
            leftHand = null;
            rightUpperArm = null;
            rightLowerArm = null;
            rightHand = null;
            hips = null;
            leftLeg = null;
            rightLeg = null;
            leftWeapon = null;
            rightWeapon = null;

            // We need to access the character's inventory so we can access each instance of each equipped item
            CharacterInventory characterInventory = this.gameObject.GetComponent<CharacterInventory>();

            if (characterInventory == null) {
                Debug.LogError("No character inventory found. Were you trying to load slots on the wrong character?");
            }

            // Head
            string headEquipment = savedSlots.Where(x => x.slotKey == "head").First().currentEquipment;
            if (!string.IsNullOrEmpty(headEquipment)) {
                ScriptableEquipment headEquipmentInstance = characterInventory.FindByItemName(headEquipment) as ScriptableEquipment;
                if (headEquipment != null) {
                    head = new Slot(headEquipmentInstance);
                    head.equipment.Equip(this.gameObject);
                }
            }

            // Torso
            string torsoEquipment = savedSlots.Where(x => x.slotKey == "torso").First().currentEquipment;
            if (!string.IsNullOrEmpty(torsoEquipment)) {
                ScriptableEquipment torsoEquipmentInstance = characterInventory.FindByItemName(torsoEquipment) as ScriptableEquipment;
                if (torsoEquipment != null) {
                    torso = new Slot(torsoEquipmentInstance);
                    torso.equipment.Equip(this.gameObject);
                }
            }

            // Left Upper Arm
            string leftUpperArmEquipment = savedSlots.Where(x => x.slotKey == "leftUpperArm").First().currentEquipment;
            if (!string.IsNullOrEmpty(leftUpperArmEquipment)) {
                ScriptableEquipment leftUpperArmEquipmentInstance = characterInventory.FindByItemName(leftUpperArmEquipment) as ScriptableEquipment;
                if (leftUpperArmEquipmentInstance != null) {
                    leftUpperArm = new Slot(leftUpperArmEquipmentInstance);
                    leftUpperArm.equipment.Equip(this.gameObject);
                }
            }

            // Left Lower Arm
            string leftLowerArmEquipment = savedSlots.Where(x => x.slotKey == "leftLowerArm").First().currentEquipment;
            if (!string.IsNullOrEmpty(leftLowerArmEquipment)) {
                ScriptableEquipment leftLowerArmEquipmentInstance = characterInventory.FindByItemName(leftLowerArmEquipment) as ScriptableEquipment;
                if (leftLowerArmEquipmentInstance != null) {
                    leftLowerArm = new Slot(leftLowerArmEquipmentInstance);
                    leftLowerArm.equipment.Equip(this.gameObject);
                }
            }

            // Left Hand
            string leftHandEquipment = savedSlots.Where(x => x.slotKey == "leftHand").First().currentEquipment;
            if (!string.IsNullOrEmpty(leftHandEquipment)) {
                ScriptableEquipment leftHandEquipmentInstance = characterInventory.FindByItemName(leftHandEquipment) as ScriptableEquipment;
                if (leftHandEquipmentInstance != null) {
                    leftHand = new Slot(leftHandEquipmentInstance);
                    leftHand.equipment.Equip(this.gameObject);
                }
            }

            // Right Upper Arm
            string rightUpperArmEquipment = savedSlots.Where(x => x.slotKey == "rightUpperArm").First().currentEquipment;
            if (!string.IsNullOrEmpty(rightUpperArmEquipment)) {
                ScriptableEquipment rightUpperArmEquipmentInstance = characterInventory.FindByItemName(rightUpperArmEquipment) as ScriptableEquipment;
                if (rightUpperArmEquipmentInstance != null) {
                    rightUpperArm = new Slot(rightUpperArmEquipmentInstance);
                    rightUpperArm.equipment.Equip(this.gameObject);
                }
            }
            
            // Right Lower Arm
            string rightLowerArmEquipment = savedSlots.Where(x => x.slotKey == "rightLowerArm").First().currentEquipment;
            if (!string.IsNullOrEmpty(rightLowerArmEquipment)) {
                ScriptableEquipment rightLowerArmEquipmentInstance = characterInventory.FindByItemName(rightLowerArmEquipment) as ScriptableEquipment;
                if (rightLowerArmEquipmentInstance != null) {
                    rightLowerArm = new Slot(rightLowerArmEquipmentInstance);
                    rightLowerArm.equipment.Equip(this.gameObject);
                }
            }

            // Right Hand
            string rightHandEquipment = savedSlots.Where(x => x.slotKey == "rightHand").First().currentEquipment;
            if (!string.IsNullOrEmpty(rightHandEquipment)) {
                ScriptableEquipment rightHandEquipmentInstance = characterInventory.FindByItemName(rightHandEquipment) as ScriptableEquipment;
                if (rightHandEquipmentInstance != null) {
                    rightHand = new Slot(rightHandEquipmentInstance);
                    rightHand.equipment.Equip(this.gameObject);
                }
            }
            
            // Hips
            string hipsEquipment = savedSlots.Where(x => x.slotKey == "hips").First().currentEquipment;
            if (!string.IsNullOrEmpty(hipsEquipment)) {
                ScriptableEquipment hipsEquipmentInstance = characterInventory.FindByItemName(hipsEquipment) as ScriptableEquipment;
                if (hipsEquipmentInstance != null) {
                    hips = new Slot(hipsEquipmentInstance);
                    hips.equipment.Equip(this.gameObject);
                }
            }

            // Left Leg
            string leftLegEquipment = savedSlots.Where(x => x.slotKey == "leftLeg").First().currentEquipment;
            if (!string.IsNullOrEmpty(leftLegEquipment)) {
                ScriptableEquipment leftLegEquipmentInstance = characterInventory.FindByItemName(leftLegEquipment) as ScriptableEquipment;
                if (leftLegEquipmentInstance != null) {
                    leftLeg = new Slot(leftLegEquipmentInstance);
                    leftLeg.equipment.Equip(this.gameObject);
                }
            }

            // Right Leg
            string rightLegEquipment = savedSlots.Where(x => x.slotKey == "rightLeg").First().currentEquipment;
            if (!string.IsNullOrEmpty(rightLegEquipment)) {
                ScriptableEquipment rightLegEquipmentInstance = characterInventory.FindByItemName(rightLegEquipment) as ScriptableEquipment;
                if (rightLegEquipmentInstance != null) {
                    rightLeg = new Slot(rightLegEquipmentInstance);
                    rightLeg.equipment.Equip(this.gameObject);
                }
            }

            // Left Weapon
            string leftWeaponEquipment = savedSlots.Where(x => x.slotKey == "leftWeapon").First().currentEquipment;
            if (!string.IsNullOrEmpty(leftWeaponEquipment)) {
                ScriptableEquipment leftWeaponEquipmentInstance = characterInventory.FindByItemName(leftWeaponEquipment) as ScriptableEquipment;
                if (leftWeaponEquipmentInstance != null) {
                    leftWeapon = new Slot(leftWeaponEquipmentInstance);
                    leftWeapon.equipment.Equip(this.gameObject);
                }
            }

            // Right Weapon
            string rightWeaponEquipment = savedSlots.Where(x => x.slotKey == "rightWeapon").First().currentEquipment;
            if (!string.IsNullOrEmpty(rightWeaponEquipment)) {
                ScriptableEquipment rightWeaponEquipmentInstance = characterInventory.FindByItemName(rightWeaponEquipment) as ScriptableEquipment;
                if (rightWeaponEquipmentInstance != null) {
                    rightWeapon = new Slot(rightWeaponEquipmentInstance);
                    rightWeapon.equipment.Equip(this.gameObject);
                }
            }

        }

    }

    [System.Serializable]
    public class SaveableSlot {
        public string slotKey; // head, torso
        public string currentEquipment; // brown shirt, null

        public SaveableSlot(string slotKey, string currentEquipment) {
            this.slotKey = slotKey;
            this.currentEquipment = currentEquipment;
        }
    }
}
