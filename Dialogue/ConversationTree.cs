using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Dialogue;

using UnityEngine.UIElements;
using RPG.Dialogue.Core;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using RPG.Events;

namespace RPG.Dialogue
{
    [System.Serializable]
    public class ConversationTree
    {

        public const string EVENT_PREFIX = "EV_";

        /// <summary>
        /// The conversation node data
        /// </summary>
        public DialogueContainer dialogueContainer;

        /// <summary>
        /// The conversation owner name
        /// </summary>
        public string dialogueOwnerName;

        /// <summary>
        /// The conversation default cutscene camera
        /// </summary>
        public GameObject dialogueCamera;

        /// <summary>
        /// The list of events to dispatch across the conversation
        /// </summary>
        public List<E_Event> events = new List<E_Event>();

        /// <summary>
        /// The current index of the conversation
        /// </summary>
        public string currentBaseNodeGuid;

        public ConversationTree(
            DialogueContainer dialogueContainer,
            string dialogueOwnerName,
            GameObject dialogueCamera,
            List<E_Event> events
        )
        {
            this.dialogueContainer = dialogueContainer;
            this.dialogueOwnerName = dialogueOwnerName;
            this.dialogueCamera = dialogueCamera;
            this.events = events;

            if (currentBaseNodeGuid == null)
            {
                // First baseNodeGuid is the default "NEXT" whic has always a target node guid associated with it.
                // Advance the dialogue in the first entry using targetNodeGuid.
                AdvanceDialogue(dialogueContainer.NodeLinks[0].TargetNodeGuid);
            }
        }

        public void AdvanceDialogue(string nextBaseNodeGuid)
        {
            currentBaseNodeGuid = nextBaseNodeGuid;
        }

        public string GetCurrentText()
        {
            string text = dialogueContainer.DialogueNodeData.Where(x => x.Guid == currentBaseNodeGuid).FirstOrDefault().DialogueText;

            return text;
        }

        public List<Choice> GetChoices()
        {
            List<Choice> choices = new List<Choice>();

            dialogueContainer.NodeLinks.ForEach(x =>
            {
                if (x.BaseNodeGuid == currentBaseNodeGuid)
                {
                    choices.Add(new Choice(x.PortName, x.TargetNodeGuid));
                }
            });

            return choices;
        }

        public string GetEventId()
        {
            return GetCurrentText().StartsWith(EVENT_PREFIX) ? GetCurrentText() : null;
        }
    }
}
