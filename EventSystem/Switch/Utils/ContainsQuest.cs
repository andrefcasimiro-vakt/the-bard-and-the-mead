using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.Quest;

namespace RPG.Switch {

    public class ContainsQuest : ICondition {
        
        public CharacterQuests characterQuest;
        public ScriptableQuest quest;

        public override bool Check() {
            bool hasQuest = characterQuest.currentQuests.Find(x => x.questId == quest.questId) != null;

            return hasQuest;
        }
    
    }
}
