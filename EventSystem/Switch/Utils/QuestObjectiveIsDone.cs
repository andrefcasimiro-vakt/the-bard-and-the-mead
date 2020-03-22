using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.Quest;

namespace RPG.Switch {

    public class QuestObjectiveIsDone : ICondition {
        
        public CharacterQuests characterQuest;
        public ScriptableQuest quest;
        public int objectiveId;

        public override bool Check() {
            RPG.Quest.Quest questMatch = characterQuest.currentQuests.Find(x => x.questId == quest.questId);

            if (questMatch == null)
                return false;

            QuestObjective objective = questMatch.objectives.Find(o => o.questObjectiveId == objectiveId);

            if (objective == null)
                return false;

            return objective.isDone;
        }
    
    }
}
