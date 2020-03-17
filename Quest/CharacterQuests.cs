using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Saving;

namespace RPG.Quest { 
    public class CharacterQuests : MonoBehaviour, ISaveable
    {
        public List<ScriptableQuest> currentQuests = new List<ScriptableQuest>();

        private SaveableQuest[] SerializeQuests()
        {
            List<SaveableQuest> saveableQuests = new List<SaveableQuest>();

            foreach(ScriptableQuest currentQuest in currentQuests)
            {
                int questId = currentQuest.questId;
                List<SaveableObjective> questObjectives = new List<SaveableObjective>();

                // Build objectives
                foreach(QuestObjective qObjective in currentQuest.objectives)
                {
                    questObjectives.Add(new SaveableObjective(qObjective.questObjectiveId, qObjective.isDone));

                }
                
                saveableQuests.Add(new SaveableQuest(questId, questObjectives));
            }

            return saveableQuests.ToArray();
        }


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

            currentQuests.Add(quest);
        }


        public object CaptureState()
        {
            return SerializeQuests();
        }

        public void RestoreState(object state)
        {
            SaveableQuest[] savedQuests = (SaveableQuest[])state;

            currentQuests.Clear();

            LoadQuests(savedQuests);
        }

        public void OnCleanState() {
            
        }

        public void LoadQuests(SaveableQuest[] savedQuests)
        {
            ScriptableQuest[] allExistingQuests = Resources.LoadAll<ScriptableQuest>("Quests");


            foreach (ScriptableQuest quest in allExistingQuests)
            {
                foreach (SaveableQuest savedQuest in savedQuests)
                {

                    if (quest.questId == savedQuest.questId)
                    {

                        for (int i = 0; i < quest.objectives.Count; i++)
                        {
                            quest.objectives[i].isDone = savedQuest.questObjectives[i].isDone;
                        }
                        
                        // Finally, add this match to the currentQuests list
                        currentQuests.Add(quest);
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class SaveableQuest
    {
        public int questId;
        public List<SaveableObjective> questObjectives = new List<SaveableObjective>();

        public SaveableQuest(int questId, List<SaveableObjective> questObjectives)
        {
            this.questId = questId;
            this.questObjectives = questObjectives;
        }
    }

    [System.Serializable]
    public class SaveableObjective
    {
        public int objectiveId;
        public bool isDone;

        public SaveableObjective(int objectiveId, bool isDone)
        {
            this.objectiveId = objectiveId;
            this.isDone = isDone;
        }
    }
}
