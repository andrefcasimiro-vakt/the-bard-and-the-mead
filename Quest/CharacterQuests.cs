using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

                print("quest id to save: " + questId);

                saveableQuests.Add(new SaveableQuest(questId, questObjectives));
            }

            return saveableQuests.ToArray();
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

        public void LoadQuests(SaveableQuest[] savedQuests)
        {
            ScriptableQuest[] allExistingQuests = Resources.LoadAll<ScriptableQuest>("Quests");


            foreach (ScriptableQuest quest in allExistingQuests)
            {
                print("xisting quest");
                    print(quest.questTitle);

                foreach (SaveableQuest savedQuest in savedQuests)
                {
                    print("Saved quest");
                    print(savedQuest.questId);

                    if (quest.questId == savedQuest.questId)
                    {
                        // We found a quest id that was saved. We need to update each quest's objective according to isDone
                        foreach(QuestObjective questObjective in quest.objectives)
                        {
                            questObjective.isDone = savedQuest.questObjectives.Find(x => x.objectiveId == questObjective.questId).isDone;
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
