using UnityEngine;

namespace RPG.Stats {
        
    [CreateAssetMenu(fileName = "Stats", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        // =====================================> [ CLASS ] <================================

        // Character Class sets the base stats for health, attack, defense, agility, etc.
        [SerializeField]
        CharacterStat[] characterStats;

        [System.Serializable]
        class CharacterStat {
            public string classDisplayName = "";
            public CharacterClassEnum characterClass;
            public float initialHealthPoints;
            public float initialStaminaPoints;
            public float initialAttackPoints;
            public float initialDefensePoints;
            public float initialAgilityPoints;
            public float levelMultiplier;

        }

        // =====================================> [ RACE ] <================================
        // Races set bonus point stats for health, attack, defense, agility, etc.
        [SerializeField]
        CharacterRace[] characterRaces;

        [System.Serializable]
        class CharacterRace {
            public string raceDisplayName = "";
            public CharacterRaceEnum characterRace;
            public float bonusHealthPoints;
            public float bonusStaminaPoints;
            public float bonusAttackPoints;
            public float bonusDefensePoints;
            public float bonusAgilityPoints;
        }

        // =====================================> [ GENDER ] <================================
        // Races set bonus point stats for health, attack, defense, agility, etc.
        [SerializeField]
        CharacterGender[] characterGenders;

        [System.Serializable]
        class CharacterGender {
            public string genderDisplayName = "";
            public CharacterGenderEnum characterGender;
            public float bonusHealthPoints;
            public float bonusStaminaPoints;
            public float bonusAttackPoints;
            public float bonusDefensePoints;
            public float bonusAgilityPoints;
        }

        // =====================================> [ GETTERS BASED ON CURRENT CHARACTER STATS CALCULATIONS ] <================================

        // HEALTH
        public float GetHealth(
            int startLevel,
            CharacterClassEnum characterClass,
            CharacterRaceEnum characterRace,
            CharacterGenderEnum characterGender
        ) {
            float healthPoints = 0f;
            
            // Evaluate base health per class and current level
            foreach(CharacterStat stat in characterStats)
            {
                if (stat.characterClass == characterClass)
                {
                    healthPoints = stat.initialHealthPoints + (startLevel * stat.levelMultiplier);
                }
            }

            // Add bonus based on race
            foreach(CharacterRace race in characterRaces)
            {
                if (race.characterRace == characterRace)
                {
                    healthPoints += race.bonusHealthPoints;
                }
            }

            // Add bonus based on gender
            foreach(CharacterGender gender in characterGenders)
            {
                if (gender.characterGender == characterGender)
                {
                    healthPoints += gender.bonusHealthPoints;
                }
            }

            // Finally, return constructed health points
            return healthPoints;           
        }

        // STAMINA
        public float GetStamina(
            int startLevel,
            CharacterClassEnum characterClass,
            CharacterRaceEnum characterRace,
            CharacterGenderEnum characterGender
        )
        {
            float staminaPoints = 0f;

            // Evaluate base health per class and current level
            foreach (CharacterStat stat in characterStats)
            {
                if (stat.characterClass == characterClass)
                {
                    staminaPoints = stat.initialStaminaPoints + (startLevel * stat.levelMultiplier);
                }
            }

            // Add bonus based on race
            foreach (CharacterRace race in characterRaces)
            {
                if (race.characterRace == characterRace)
                {
                    staminaPoints += race.bonusStaminaPoints;
                }
            }

            // Add bonus based on gender
            foreach (CharacterGender gender in characterGenders)
            {
                if (gender.characterGender == characterGender)
                {
                    staminaPoints += gender.bonusStaminaPoints;
                }
            }

            // Finally, return constructed health points
            return staminaPoints;
        }

        // AGILITY
        public float GetAgility(
            int startLevel,
            CharacterClassEnum characterClass,
            CharacterRaceEnum characterRace,
            CharacterGenderEnum characterGender
        )
        {
            float agilityPoints = 0f;

            // Evaluate base health per class and current level
            foreach (CharacterStat stat in characterStats)
            {
                if (stat.characterClass == characterClass)
                {
                    agilityPoints = stat.initialAgilityPoints + (startLevel * stat.levelMultiplier);
                }
            }

            // Add bonus based on race
            foreach (CharacterRace race in characterRaces)
            {
                if (race.characterRace == characterRace)
                {
                    agilityPoints += race.bonusAgilityPoints;
                }
            }

            // Add bonus based on gender
            foreach (CharacterGender gender in characterGenders)
            {
                if (gender.characterGender == characterGender)
                {
                    agilityPoints += gender.bonusAgilityPoints;
                }
            }

            // Finally, return constructed health points
            return agilityPoints;
        }

    }

}
