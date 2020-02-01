using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.Events;

namespace RPG.Dialogue {
    
    [System.Serializable]
    public class DialogueChoice
    {
        public string text;
        public int nextDialogueEntryId;

        public DialogueChoice(string text, int nextDialogueEntryId)
        {
            this.text = text;
            this.nextDialogueEntryId = nextDialogueEntryId;
        }

    }

    [System.Serializable]
    public class DialogueEntry
    {
        public string text;
        public int id;
        public int nextDialogueEntryId;
        public List<DialogueChoice> choices = new List<DialogueChoice>();
        public List<UnityEvent> events = new List<UnityEvent>();

        public DialogueEntry(
            string text,
            int id,
            int nextDialogueEntryId,
        List<DialogueChoice> choices,
            List<UnityEvent> events
        )
        {
            this.text = text;
            this.id = id;
            this.nextDialogueEntryId = nextDialogueEntryId;
            this.choices = choices;
            this.events = events;
        }

    }

    public class Dialogue : MonoBehaviour
    {
        public string name;
        public string description;
        public List<DialogueEntry> entries = new List<DialogueEntry>();
    }
}
