using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;

namespace RPG.Character {

    public enum BodyPart
    {
        Head,
        Torso,
        ArmUpperRight,
        ArmLowerRight,
        RightHand,
        ArmUpperLeft,
        ArmLowerLeft,
        LeftHand,
        Hips,
        LeftLeg,
        RightLeg,
        Hair,
        Eyebrows,
        Beard,
        // NEW:
        LeftShoulder,
        RightShoulder,
        Shield,
        Accessory,
        RightHandWeapon
    }

    [System.Serializable]
    public class CharacterBody
    {
        public BodyPart id;
        public GameObject gameObject;
        public string prefix;

        public CharacterBody(BodyPart id, string prefix)
        {
            this.id = id;
            this.prefix = prefix;
        }

        public void SetGameObject(GameObject g)
        {
            this.gameObject = g;
        }
    }


    /*
        This class must store the character gender, race and base graphics 
    */
    public class CharacterGraphic : MonoBehaviour
    {
        public GameObject placeholder;

        public string CharacterBase_Prefix = "Chr_";

        public List<CharacterBody> characterBody = new List<CharacterBody>();

        [Header("Body")]
        public string Head_Prefix = "Head_00";
        public string Torso_Prefix = "Torso_00";

        public string ArmUpperRight_Prefix = "ArmUpperRight_00";
        public string ArmLowerRight_Prefix = "ArmLowerRight_00";
        public string HandRight_Prefix = "HandRight_00";

        public string ArmUpperLeft_Prefix = "ArmUpperLeft_00";
        public string ArmLowerLeft_Prefix = "ArmLowerLeft_00";
        public string HandLeft_Prefix = "HandLeft_00";

        public string Hips_Prefix = "Hips_00";

        public string LegRight_Prefix = "LegRight_00";
        public string LegLeft_Prefix = "LegLeft_00";

        [Header("Optional Accessories")]
        public string Hair_Prefix = "";
        public string Eyebrows_Prefix = "";
        public string Beard_Prefix = "";

        [Header("Skin Tone")]
        public string Etnicity_Prefix = "";

        [Header("Gender")]
        public CharacterGenderEnum gender = CharacterGenderEnum.MALE;

        // Needs to be loaded before Start because some scripts depend on this
        void Awake()
        {
            if (placeholder != null) {
                placeholder.SetActive(false);
            }

            string genderPrefix = gender == CharacterGenderEnum.MALE ? "_Male" : "_Female";

            characterBody.Add(new CharacterBody(BodyPart.Head, AddGenderToBodyPrefix(Head_Prefix, genderPrefix) ));
            characterBody.Add(new CharacterBody(BodyPart.Torso, AddGenderToBodyPrefix(Torso_Prefix, genderPrefix) ));
            characterBody.Add(new CharacterBody(BodyPart.ArmUpperRight, AddGenderToBodyPrefix(ArmUpperRight_Prefix, genderPrefix) ));
            characterBody.Add(new CharacterBody(BodyPart.ArmLowerRight, AddGenderToBodyPrefix(ArmLowerRight_Prefix, genderPrefix) ));
            characterBody.Add(new CharacterBody(BodyPart.RightHand, AddGenderToBodyPrefix(HandRight_Prefix, genderPrefix) ));
            characterBody.Add(new CharacterBody(BodyPart.ArmUpperLeft, AddGenderToBodyPrefix(ArmUpperLeft_Prefix, genderPrefix) ));
            characterBody.Add(new CharacterBody(BodyPart.ArmLowerLeft, AddGenderToBodyPrefix(ArmLowerLeft_Prefix, genderPrefix) ));
            characterBody.Add(new CharacterBody(BodyPart.LeftHand, AddGenderToBodyPrefix(HandLeft_Prefix, genderPrefix) ));
            characterBody.Add(new CharacterBody(BodyPart.Hips, AddGenderToBodyPrefix(Hips_Prefix, genderPrefix) ));
            characterBody.Add(new CharacterBody(BodyPart.LeftLeg, AddGenderToBodyPrefix(LegRight_Prefix, genderPrefix) ));
            characterBody.Add(new CharacterBody(BodyPart.RightLeg, AddGenderToBodyPrefix(LegLeft_Prefix, genderPrefix) ));
            // Accessories
            characterBody.Add(new CharacterBody(BodyPart.Hair, Hair_Prefix));
            characterBody.Add(new CharacterBody(BodyPart.Eyebrows, Eyebrows_Prefix));
            characterBody.Add(new CharacterBody(BodyPart.Beard, Beard_Prefix));

            BuildCharacter();
        }

        string AddGenderToBodyPrefix(string bodyPrefix, string genderPrefix)
        {
            return CharacterBase_Prefix + bodyPrefix.Insert(bodyPrefix.Length - 3, genderPrefix) + Etnicity_Prefix;
        }

        public void BuildCharacter()
        {
            foreach (Transform t in GetComponentsInChildren<Transform>(true))
            {
                foreach (CharacterBody c in characterBody)
                {   
                    if (c.prefix == t.gameObject.name)
                    {
                        c.SetGameObject(t.gameObject);
                        c.gameObject.SetActive(true);
                    }
                }
            }
        }

        public void TogglePart(BodyPart bodyPart, bool value)
        {
            CharacterBody _c = characterBody.Find(c => c.id == bodyPart);

            if (_c != null) {
                _c.gameObject.SetActive(value);
            }
        }

    }
}
