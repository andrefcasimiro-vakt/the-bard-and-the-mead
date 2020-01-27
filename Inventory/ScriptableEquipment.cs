using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using RPG.Character;

namespace RPG.Inventory
{
    [CreateAssetMenu(fileName = "Equipments", menuName = "Equipments/New Equipment", order = 0)]
    public class ScriptableEquipment : ScriptableItem
    {
        [Header("Stats")]
        public float defenseRate = 1f;
        public float agilityMultiplier = 0f;

        [Header("Graphic")]
        public string graphicGameObjectName;

        public BodyPart bodyPart;

        public void Equip(GameObject target)
        {
            GameObject equipmentGraphic = null;

            foreach (Transform t in target.GetComponentsInChildren<Transform>(true))
            {
                if (t.gameObject.name == graphicGameObjectName)
                {
                    equipmentGraphic = t.gameObject;
                    break;
                }
            }


            if (equipmentGraphic.activeSelf)
            {
                Debug.Log("Item already equipped");
                Unequip(target);
                return;
            }

            equipmentGraphic.SetActive(true);


            if (equipmentGraphic != null)
            {
                equipmentGraphic.gameObject.SetActive(true);
            }


            CharacterGraphic charGraphic = target.GetComponent<CharacterGraphic>();

            switch (bodyPart)
            {
                case BodyPart.HAIR:
                    charGraphic.HideHair();
                    break;
                case BodyPart.HEAD:
                    charGraphic.HideHead();
                    charGraphic.HideEyebrows();
                    charGraphic.HideBeard();
                    break;
                case BodyPart.TORSO:
                    charGraphic.HideTorso();
                    break;
                case BodyPart.UPPER_RIGHT_ARM:
                    charGraphic.HideUpperRightArm();
                    break;
                case BodyPart.LOWER_RIGHT_ARM:
                    charGraphic.HideLowerRightArm();
                    break;
                case BodyPart.RIGHT_HAND:
                    charGraphic.HideRightHand();
                    break;
                case BodyPart.UPPER_LEFT_ARM:
                    charGraphic.HideUpperLeftArm();
                    break;
                case BodyPart.LOWER_LEFT_ARM:
                    charGraphic.HideLowerLeftArm();
                    break;
                case BodyPart.LEFT_HAND:
                    charGraphic.HideLeftHand();
                    break;
                case BodyPart.HIPS:
                    charGraphic.HideHips();
                    break;
                case BodyPart.RIGHT_LEG:
                    charGraphic.HideRightLeg();
                    break;
                case BodyPart.LEFT_LEG:
                    charGraphic.HideLeftLeg();
                    break;
                default:
                    break;
            }
        }

        public void Unequip(GameObject target)
        {
            GameObject equipmentGraphic = null;

            foreach (Transform t in target.GetComponentsInChildren<Transform>(true))
            {
                if (t.gameObject.name == graphicGameObjectName)
                {
                    equipmentGraphic = t.gameObject;
                    break;
                }
            }

            equipmentGraphic.SetActive(false);

            CharacterGraphic charGraphic = target.GetComponent<CharacterGraphic>();

            switch (bodyPart)
            {
                case BodyPart.HAIR:
                    charGraphic.ShowHair();
                    break;
                case BodyPart.HEAD:
                    charGraphic.ShowHead();
                    charGraphic.ShowEyebrows();
                    charGraphic.ShowBeard();
                    break;
                case BodyPart.TORSO:
                    charGraphic.ShowTorso();
                    break;
                case BodyPart.UPPER_RIGHT_ARM:
                    charGraphic.ShowUpperRightArm();
                    break;
                case BodyPart.LOWER_RIGHT_ARM:
                    charGraphic.ShowLowerRightArm();
                    break;
                case BodyPart.RIGHT_HAND:
                    charGraphic.ShowRightHand();
                    break;
                case BodyPart.UPPER_LEFT_ARM:
                    charGraphic.ShowUpperLeftArm();
                    break;
                case BodyPart.LOWER_LEFT_ARM:
                    charGraphic.ShowLowerLeftArm();
                    break;
                case BodyPart.LEFT_HAND:
                    charGraphic.ShowLeftHand();
                    break;
                case BodyPart.HIPS:
                    charGraphic.ShowHips();
                    break;
                case BodyPart.RIGHT_LEG:
                    charGraphic.ShowRightLeg();
                    break;
                case BodyPart.LEFT_LEG:
                    charGraphic.ShowLeftLeg();
                    break;
                default:
                    break;
            }
        }


    }


    public enum BodyPart
    {
        HEAD,
        HAIR,
        TORSO,
        UPPER_RIGHT_ARM,
        LOWER_RIGHT_ARM,
        RIGHT_HAND,
        UPPER_LEFT_ARM,
        LOWER_LEFT_ARM,
        LEFT_HAND,
        HIPS,
        RIGHT_LEG,
        LEFT_LEG
    }
}
