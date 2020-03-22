using UnityEngine;
using RPG.Character;
using RPG.Stats;
using RPG.Weapon;

namespace RPG.Inventory
{
    [CreateAssetMenu(fileName = "Equipments", menuName = "Equipments/New Equipment", order = 0)]
    public class ScriptableEquipment : ScriptableItem
    {
        [Header("Stats")]
        [Tooltip("Will be calculated along the other equipped armors. If it is a weapon, this value will be used when parrying an attack")]
        public float defenseRate = 1f;
        public float agilityMultiplier = 0f;

        [Header("Graphic")]
        public string maleGraphicGameObjectName;
        public string femaleGraphicGameObjectName;

        public BodyPart bodyPart;

        RuntimeAnimatorController defaultAnimatorOverrideController;

        GameObject instance;

        public void Equip(GameObject target)
        {
            GameObject equipmentGraphic = null;
            BaseStats baseStats = target.GetComponent<BaseStats>();
            bool isMale = baseStats.IsMale();
            string graphicName = isMale ? maleGraphicGameObjectName : femaleGraphicGameObjectName;

            if (this.itemType == ItemEnum.WEAPON) {

                defaultAnimatorOverrideController = target.GetComponent<Animator>().runtimeAnimatorController;
                    
                target.GetComponent<CharacterEquipmentSlot>().EquipOnSlot(bodyPart, this);
                target.GetComponent<WeaponManager>().weaponSlots[0].EquipWeapon(this as ScriptableWeapon);

                // Instantiate weapon graphic here
                // Hardcode for right hand for now
                // instance = Instantiate(this.weaponPrefab, target.GetComponent<WeaponManager>().weaponSlots[0].gameObject.transform);

                target.GetComponent<Animator>().runtimeAnimatorController = this.animatorOverrideController as RuntimeAnimatorController;

                return;
            }


            foreach (Transform t in target.GetComponentsInChildren<Transform>(true))
            {
                if (t.gameObject.name == graphicName)
                {
                    equipmentGraphic = t.gameObject;
                    break;
                }
            }

            if (equipmentGraphic == null)
            {
                return;
            }

            equipmentGraphic.SetActive(true);

            CharacterGraphic charGraphic = target.GetComponent<CharacterGraphic>();
            ToggleBodyPart(charGraphic, bodyPart, false);

            target.GetComponent<CharacterEquipmentSlot>().EquipOnSlot(bodyPart, this);
        }

        public void Unequip(GameObject target)
        {
            GameObject equipmentGraphic = null;
            BaseStats baseStats = target.GetComponent<BaseStats>();
            bool isMale = baseStats.IsMale();
            string graphicName = isMale ? maleGraphicGameObjectName : femaleGraphicGameObjectName;

            if (this.itemType == ItemEnum.WEAPON) {
                target.GetComponent<CharacterEquipmentSlot>().EquipOnSlot(bodyPart, null);
                target.GetComponent<Animator>().runtimeAnimatorController = defaultAnimatorOverrideController as RuntimeAnimatorController;

                target.GetComponent<WeaponManager>().weaponSlots[0].UnequipWeapon();

                Destroy(instance);
                return;
            }


            foreach (Transform t in target.GetComponentsInChildren<Transform>(true))
            {
                if (t.gameObject.name == graphicName)
                {
                    equipmentGraphic = t.gameObject;
                    break;
                }
            }

            equipmentGraphic.SetActive(false);

            CharacterGraphic charGraphic = target.GetComponent<CharacterGraphic>();
            ToggleBodyPart(charGraphic, bodyPart, true);

            target.GetComponent<CharacterEquipmentSlot>().EquipOnSlot(bodyPart, null);
        }

            
        void ToggleBodyPart(CharacterGraphic charGraphic, BodyPart bodyPart, bool value)
        {
            switch (bodyPart)
            {
                case BodyPart.Hair:
                    charGraphic.TogglePart(BodyPart.Hair, value);
                    break;
                case BodyPart.Head:
                    charGraphic.TogglePart(BodyPart.Head, value);
                    break;
                case BodyPart.Torso:
                    charGraphic.TogglePart(BodyPart.Torso, value);
                    break;
                case BodyPart.ArmUpperRight:
                    charGraphic.TogglePart(BodyPart.ArmUpperRight, value);
                    break;
                case BodyPart.ArmLowerRight:
                    charGraphic.TogglePart(BodyPart.ArmLowerRight, value);
                    break;
                case BodyPart.RightHand:
                    charGraphic.TogglePart(BodyPart.RightHand, value);
                    break;
                case BodyPart.ArmUpperLeft:
                    charGraphic.TogglePart(BodyPart.ArmUpperLeft, value);
                    break;
                case BodyPart.ArmLowerLeft:
                    charGraphic.TogglePart(BodyPart.ArmLowerLeft, value);
                    break;
                case BodyPart.LeftHand:
                    charGraphic.TogglePart(BodyPart.LeftHand, value);
                    break;
                case BodyPart.Hips:
                    charGraphic.TogglePart(BodyPart.Hips, value);
                    break;
                case BodyPart.RightLeg:
                    charGraphic.TogglePart(BodyPart.RightLeg, value);
                    break;
                case BodyPart.LeftLeg:
                    charGraphic.TogglePart(BodyPart.LeftLeg, value);
                    break;
                default:
                    break;
            }
        }

    }
}
