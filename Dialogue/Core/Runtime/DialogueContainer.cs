using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue.Core { 
    [System.Serializable]
    public class DialogueContainer : ScriptableObject
    {
        public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();

        public List<DialogueNodeData> DialogueNodeData = new List<DialogueNodeData>();
    }
}
