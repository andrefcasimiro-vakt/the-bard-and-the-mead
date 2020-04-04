using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quest {

    [System.Serializable]
    public class QuestObjective
    {
        public int questObjectiveId;
        public string questObjectiveDescription;
        public bool isDone = false;

        // If objective needs a counter to be completed (e. g. Kill 10 slimes)
        public bool useCounter = false;
        public int counter = 0;
        public int maxCounter = 10; 

        public QuestObjective(
            int questObjectiveId,
            int questId,
            string questObjectiveDescription,
            bool isDone,
            bool useCounter,
            int counter,
            int maxCounter
        )
        {
            this.questObjectiveId = questObjectiveId;
            this.questObjectiveDescription = questObjectiveDescription;
            this.isDone = isDone;

            this.useCounter = useCounter;
            this.counter = counter;
            this.counter = maxCounter;
        }
    }

    
    [CreateAssetMenu(fileName = "Quests", menuName = "Quest/New Quest", order = 0)]
    public class ScriptableQuest : ScriptableObject {

        public int questId;
        public string questTitle;
        public string questLocation;
        public string questDescription;
        public List<QuestObjective> objectives = new List<QuestObjective>();
    
    }
}

