using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quest {

    [System.Serializable]
    public class QuestObjective
    {
        public int questObjectiveId;
        public int questId;
        public string questObjectiveDescription;
        public bool isDone = false;

        public QuestObjective(
            int questObjectiveId,
            int questId,
            string questObjectiveDescription,
            bool isDone
        )
        {
            this.questObjectiveId = questObjectiveId;
            this.questId = questId;
            this.questObjectiveDescription = questObjectiveDescription;
            this.isDone = isDone;
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

