using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Dialogue;

using UnityEngine.UIElements;
using UnityEditor.UIElements;
using RPG.Dialogue.Core;
using System;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

namespace RPG.Dialogue
{
    [System.Serializable]
    public class ConversationTree
    {

        public DialogueContainer dialogueContainer;
        public string currentBaseNodeGuid;

        public ConversationTree(DialogueContainer d)
        {
            this.dialogueContainer = d;

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
    }
}
