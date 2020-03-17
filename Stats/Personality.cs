using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Saving;

namespace RPG.Stats {
    public class Personality : MonoBehaviour, ISaveable
    {
        // Influenced by speech
        public int proactivity = 0;
        public int intelligence = 0;
        public int politeness = 0;
        public int charisma = 0;

        public void IncreaseProactivity(int amount)
        {
            proactivity += amount;
            ShowPopup("+1 Proactivity");
        }
        public void DecreaseProactivity(int amount)
        {
            proactivity -= amount;
            ShowPopup("-1 Proactivity");
        }

        public void IncreaseIntelligence(int amount)
        {
            intelligence += amount;
            ShowPopup("+1 Intelligence");
        }
        public void DecreaseIntelligence(int amount)
        {
            intelligence -= amount;
            ShowPopup("-1 Intelligence");
        }

        public void IncreasePoliteness(int amount)
        {
            politeness += amount;
            ShowPopup("+1 Politeness");
        }
        public void DecreasePoliteness(int amount)
        {
            politeness -= amount;
            ShowPopup("-1 Politeness");
        }
        
        public void IncreaseCharisma(int amount)
        {
            charisma += amount;
            ShowPopup("+1 Charisma");
        }
        public void DecreaseCharisma(int amount)
        {
            charisma -= amount;
            ShowPopup("-1 Charisma");
        }

        void ShowPopup(string text) 
        {
            GameObject popup = GameObject.FindWithTag("Popup");

            if (popup == null)
            {
                return;
            }

            popup.GetComponent<Animator>().Play("Idle");

            popup.GetComponent<Animator>().Play("Show");
            popup.transform.GetChild(0).gameObject.GetComponent<Text>().text = text;

        }

        // SAVE

        public object CaptureState()
        {
            return new SerializablePersonality(proactivity, intelligence, politeness, charisma);
        }

        public void RestoreState(object state)
        {
            SerializablePersonality restoredStats = (SerializablePersonality)state;

            proactivity = restoredStats.proactivity;
            intelligence = restoredStats.intelligence;
            politeness = restoredStats.politeness;
            charisma = restoredStats.charisma;
        }

        public void OnCleanState() {
        }
    }

    [System.Serializable]
    public class SerializablePersonality {
        public int proactivity;
        public int intelligence;
        public int politeness;
        public int charisma;

        public SerializablePersonality(
            int proactivity,
            int intelligence,
            int politeness,
            int charisma
        ) {
            this.proactivity = proactivity;
            this.intelligence = intelligence;
            this.politeness = politeness;
            this.charisma = charisma;
        }
    }
}
