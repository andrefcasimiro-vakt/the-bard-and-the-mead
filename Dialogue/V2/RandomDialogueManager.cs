using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using RPG.AIV3;
using RPG.Dialogue;
using RPG.Dialogue.Core;
using RPG.Events;
using RPG.Control;
using System.Collections.Generic;
using RPG.Saving;
using RPG.V2.UI.Utils.Interfaces;

namespace RPG.Dialogue {

    [System.Serializable]
    public class RandomDialogue
    {
        public string key;
        [Range(0, 1f)] public float minChance;
        [Range(0, 1f)] public float maxChance;
        public ScriptableObject conversation;

        public RandomDialogue(string key, float minChance, float maxChance, ScriptableObject conversation)
        {
            this.key = key;
            this.minChance = minChance;
            this.maxChance = maxChance;
            this.conversation = conversation;
        }
    }

}
