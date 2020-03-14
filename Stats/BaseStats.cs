using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Saving;

namespace RPG.Stats {
    public class BaseStats : MonoBehaviour, ISaveable
    {
        [Range(1, 99)]
        [SerializeField]
        int startLevel = 1;


        [Header("Level & XP")]
        [SerializeField] GameObject expSplider; // The current exp bar
        [SerializeField] GameObject levelText; // Lv. 1
        [SerializeField] GameObject experienceText; // 10/200

        public int currentLevel = 1;
        public int currentExperience = 0;

        [SerializeField]
        CharacterClassEnum characterClass;

        [SerializeField]
        CharacterRaceEnum characterRace;

        [SerializeField]
        CharacterGenderEnum characterGender;

        [SerializeField]
        Progression progression;


        private void Update()
        {
            if (expSplider != null)
            {
                UpdateUI();
            }
        }

        // Updates the owner stamina HUD
        void UpdateUI()
        { 
            int expToNextLevel = GetRequiredExpToNextLevel();

            expSplider.GetComponent<Slider>().maxValue = expToNextLevel;
            expSplider.GetComponent<Slider>().minValue = 0;
            expSplider.GetComponent<Slider>().value = currentExperience;

            experienceText.GetComponent<Text>().text = currentExperience + "/" + expToNextLevel;
            levelText.GetComponent<Text>().text = "Lv. " + currentLevel;
        }

        
        public float GetHealth()
        {
            return progression.GetHealth(startLevel, characterClass, characterRace, characterGender);
        }

        public float GetStamina()
        {
            return progression.GetStamina(startLevel, characterClass, characterRace, characterGender);
        }

        public float GetAgility()
        {
            return progression.GetAgility(startLevel, characterClass, characterRace, characterGender);
        }

        public bool IsMale()
        {
            return characterGender == CharacterGenderEnum.MALE;
        }

        public string GetRace()
        {
            return characterRace.ToString();
        }


        // Experience and Levelling up

        public void LevelUp ()
        {
            float diff = currentExperience - GetRequiredExpToNextLevel();

            currentLevel++;

            currentExperience = (int)Mathf.Round(diff);
        }

        public void IncreaseExperience (int amount)
        {
            currentExperience += amount;

            GameObject expPopup = GameObject.FindWithTag("EXP_Popup");

            if (currentExperience >= GetRequiredExpToNextLevel())
            {
                expPopup.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Level Up!";

                LevelUp();
            } else {
                expPopup.transform.GetChild(0).gameObject.GetComponent<Text>().text = "+ " + amount + " Exp";
            }

            expPopup.GetComponent<Animator>().SetTrigger("Show");
        }

        public int GetRequiredExpToNextLevel()
        {
            return progression.GetRequiredExperienceForLevel(currentLevel, characterClass);
        }

        // SAVE

        public object CaptureState()
        {
            return new SaveableStats(currentLevel, currentExperience);
        }

        public void RestoreState(object state)
        {
            SaveableStats restoredStats = (SaveableStats)state;

            currentExperience = restoredStats.currentExperience;
            currentLevel = restoredStats.currentLevel;
        }

        public void OnCleanState() {
            currentLevel = startLevel;
        }
    }

    [System.Serializable]
    public class SaveableStats {
        public int currentLevel;
        public int currentExperience;

        public SaveableStats (int currentLevel, int currentExperience)
        {
            this.currentLevel = currentLevel;
            this.currentExperience = currentExperience;
        }
    }
}
