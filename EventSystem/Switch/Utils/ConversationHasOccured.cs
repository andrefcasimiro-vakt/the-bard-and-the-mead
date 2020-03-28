using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.Dialogue;

namespace RPG.Switch {

    public class ConversationHasOccured : ICondition {
        
        public ConversationManager conversationManager;

        public override bool Check() {
            return conversationManager.conversationHasOccurred;
        }
    
    }
}
