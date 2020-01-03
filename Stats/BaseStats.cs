using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats {
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField]
        int startLevel = 1;

        [SerializeField]
        CharacterClassEnum characterClass;

        [SerializeField]
        CharacterRaceEnum characterRace;

        [SerializeField]
        CharacterGenderEnum characterGender;

        [SerializeField]
        Progression progression;
        
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

    }
}
