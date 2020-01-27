using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character {

    public class CharacterGraphic : MonoBehaviour
    {

        public GameObject head;
        public GameObject torso;
        public GameObject upperRightArm;
        public GameObject lowerRightArm;
        public GameObject rightHand;
        public GameObject upperLeftArm;
        public GameObject lowerLeftArm;
        public GameObject leftHand;
        public GameObject hips;
        public GameObject rightLeg;
        public GameObject leftLeg;

        // Accessories
        public GameObject hair;
        public GameObject eyebrows;
        public GameObject beard;

        public void ShowHead()
        {
            head.SetActive(true);
        }
        public void HideHead()
        {
            head.SetActive(false);
        }

        public void ShowTorso()
        {
            torso.SetActive(true);
        }
        public void HideTorso()
        {
            torso.SetActive(false);
        }

        public void ShowUpperRightArm()
        {
            upperRightArm.SetActive(true);
        }
        public void HideUpperRightArm()
        {
            upperRightArm.SetActive(false);
        }

        public void ShowLowerRightArm()
        {
            lowerRightArm.SetActive(true);
        }
        public void HideLowerRightArm()
        {
            lowerRightArm.SetActive(false);
        }

        public void ShowRightHand()
        {
            rightHand.SetActive(true);
        }
        public void HideRightHand()
        {
            rightHand.SetActive(false);
        }

        public void ShowUpperLeftArm() { upperLeftArm.SetActive(true); }
        public void HideUpperLeftArm() { upperLeftArm.SetActive(false); }

        public void ShowLowerLeftArm() { lowerLeftArm.SetActive(true); }
        public void HideLowerLeftArm() { lowerLeftArm.SetActive(false); }

        public void ShowLeftHand() { leftHand.SetActive(true); }
        public void HideLeftHand() { leftHand.SetActive(false); }

        public void ShowHips() { hips.SetActive(true); }
        public void HideHips() { hips.SetActive(false); }

        public void ShowRightLeg() { rightLeg.SetActive(true); }
        public void HideRightLeg() { rightLeg.SetActive(false); }

        public void ShowLeftLeg() { leftLeg.SetActive(true); }
        public void HideLeftLeg() { leftLeg.SetActive(false); }

        public void ShowHair() { hair?.SetActive(true); }
        public void HideHair() { hair?.SetActive(false); }

        public void ShowEyebrows() { eyebrows?.SetActive(true); }
        public void HideEyebrows() { eyebrows?.SetActive(false); }

        public void ShowBeard() { beard?.SetActive(true); }
        public void HideBeard() { beard?.SetActive(false); }

        private void Start()
        {
            ShowHead();
            ShowTorso();
            ShowUpperRightArm();
            ShowLowerRightArm();
            ShowRightHand();
            ShowUpperLeftArm();
            ShowLowerLeftArm();
            ShowLeftHand();
            ShowHips();
            ShowRightLeg();
            ShowLeftLeg();
            ShowHair();
            ShowEyebrows();
            ShowBeard();
        }

    }
}
