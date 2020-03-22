using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Saving;
using System.Linq;

namespace RPG.Quest { 

    [System.Serializable]
    public class Quest
    {
        public int questId;
        public string questTitle;
        public string questLocation;
        public string questDescription;
        public List<QuestObjective> objectives = new List<QuestObjective>();
    
        public Quest(
            int questId,
            string questTitle,
            string questLocation,
            string questDescription,
            List<QuestObjective> objectives
        )
        {
            this.questId = questId;
            this.questTitle = questTitle;
            this.questLocation = questLocation;
            this.questDescription = questDescription;
            this.objectives = objectives;
        }
    }


    public class CharacterQuests : MonoBehaviour, ISaveable
    {
        public List<Quest> currentQuests = new List<Quest>();

        /// Adds a quest and notifies the player
        public void AddQuest(ScriptableQuest quest)
        {
             GameObject popup = GameObject.FindWithTag("Popup");

            if (popup == null)
            {
                return;
            }

            popup.GetComponent<Animator>().Play("Show");
            popup.transform.GetChild(0).gameObject.GetComponent<Text>().text = "New Quest: " + quest.name;

            // Create a copy of the ScriptableQuest since we can't mutate ScriptableObjects
            Quest newQuest = new Quest(
                quest.questId,
                quest.questTitle,
                quest.questLocation,
                quest.questDescription,
                quest.objectives
            );

            currentQuests.Add(newQuest);
        }

        public void CompleteObjective(int objectiveId, int questId)
        {
            currentQuests.Find(x => x.questId == questId).objectives.Find(o => o.questObjectiveId == objectiveId).isDone = true;
        }

        // For editor purposes
        public void CompleteObjective(string questInfo) // Should follow the following structure: 1:0 (left is questId, right is objectiveId)
        {
            string[] split = questInfo.Split(':');
            int questId = System.Convert.ToInt32(split[0]);
            int objectiveId = System.Convert.ToInt32(split[1]);


            currentQuests.Find(x => x.questId == questId).objectives.Find(o => o.questObjectiveId == objectiveId).isDone = true;
        }

        private Quest[] SerializeQuests()
        {
            return currentQuests.ToArray();
        }

        public object CaptureState()
        {
            return SerializeQuests();
        }

        public void RestoreState(object state)
        {
            Quest[] savedQuests = (Quest[])state;

            currentQuests.Clear();

            LoadQuests(savedQuests);
        }

        public void OnCleanState() {
            
        }

        public void LoadQuests(Quest[] savedQuests)
        {
            currentQuests = savedQuests.ToList();
        }

    }

}
