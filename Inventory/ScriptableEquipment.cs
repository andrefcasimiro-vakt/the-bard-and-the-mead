using UnityEngine;
using RPG.Character;
using RPG.Stats;

namespace RPG.Inventory
{
    [CreateAssetMenu(fileName = "Equipments", menuName = "Equipments/New Equipment", order = 0)]
    public class ScriptableEquipment : ScriptableItem
    {
        [Header("Stats")]
        public float defenseRate = 1f;
        public float agilityMultiplier = 0f;

        [Header("Graphic")]
        public string maleGraphicGameObjectName;
        public string femaleGraphicGameObjectName;

        public BodyPart bodyPart;

        public void Equip(GameObject target)
        {
            GameObject equipmentGraphic = null;
            BaseStats baseStats = target.GetComponent<BaseStats>();
            bool isMale = baseStats.IsMale();
            string graphicName = isMale ? maleGraphicGameObjectName : femaleGraphicGameObjectName;

            foreach (Transform t in target.GetComponentsInChildren<Transform>(true))
            {
                if (t.gameObject.name == graphicName)
                {
                    equipmentGraphic = t.gameObject;
                    break;
                }
            }

            equipmentGraphic.SetActive(true);

            if (equipmentGraphic != null)
            {
                equipmentGraphic.gameObject.SetActive(true);
            }


            CharacterGraphic charGraphic = target.GetComponent<CharacterGraphic>();
            ToggleBodyPart(charGraphic, bodyPart, false);
        }

        public void Unequip(GameObject target)
        {
            GameObject equipmentGraphic = null;
            BaseStats baseStats = target.GetComponent<BaseStats>();
            bool isMale = baseStats.IsMale();
            string graphicName = isMale ? maleGraphicGameObjectName : femaleGraphicGameObjectName;

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
