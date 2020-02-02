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
    public class Choice
    {
        public string text;
        public string targetGUID;

        public Choice(string text, string targetGUID)
        {
            this.text = text;
            this.targetGUID = targetGUID;
        }
    }

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


    public class DialogueUI : MonoBehaviour
    {

        public ConversationTree conversationTree;
        public string dialogueOwnerName;

        public GameObject dialogueOwner;
        public GameObject dialogueText;
        public GameObject choicePanel;
        public GameObject choiceButtonPrefab;

        public GameObject cutsceneCamera;

        bool repaint = false;

        GameObject player;

        private void Start()
        {
            Toggle(false);
            player = GameObject.FindWithTag("Player");
        }

        void Toggle(bool v)
        {
            foreach (Transform c in this.transform)
            {
                c.gameObject.SetActive(v);
            }
        }

        public void SetConversation(ConversationTree c, string dialogueOwner, GameObject camera)
        {
            player.SetActive(false);
            conversationTree = c;
            cutsceneCamera = camera;
            dialogueOwnerName = dialogueOwner;
            cutsceneCamera.SetActive(true);


            Toggle(true);

            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;

            repaint = true;
        }

        public void EndConversation()
        {
            player.SetActive(true);

            conversationTree = null;

            cutsceneCamera.SetActive(false);
            cutsceneCamera = null;

            Toggle(false);
            repaint = false;

            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }

        private void Update()
        {
            if (conversationTree == null || conversationTree.dialogueContainer == null)
            {
                return;
            }

            if (repaint == true) { 
                ManageDialogue();
                repaint = false;
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                List<Choice> choices = conversationTree.GetChoices();

                if (choices.Count <= 1)
                {
                    string id = choices.Count == 1 ? choices[0].targetGUID : null;

                    Advance(id);
                }
            }
        }

        private void ManageDialogue()
        {
            Debug.Log("dialogueOwner" + dialogueOwnerName);
            dialogueOwner.GetComponent<Text>().text = dialogueOwnerName;
            dialogueText.GetComponent<Text>().text = conversationTree.GetCurrentText();

            List<Choice> choices = conversationTree.GetChoices();

            // Clear choice button panel:
            // Clean panel first
            foreach (Transform child in choicePanel.transform)
            {
                Destroy(child.gameObject);
            }

            if (choices.Count > 1)
            {
                // CHOICES EXIST. DRAW BUTTONS.

                foreach (Choice c in choices)
                {
                    GameObject btn = Instantiate(choiceButtonPrefab, choicePanel.transform);
                    UnityEngine.UI.Button b = btn.GetComponent<UnityEngine.UI.Button>();

                    b.transform.GetChild(0).GetComponent<Text>().text = c.text;

                    b.onClick.RemoveAllListeners();
                    b.onClick.AddListener(() => {
                        Advance(c.targetGUID);
                    });
                }

            }

        }

        void Advance(string targetGuid)
        {
            if (string.IsNullOrEmpty(targetGuid))
            {
                EndConversation();
                return;
            }

            conversationTree.AdvanceDialogue(targetGuid);
            repaint = true;
        }


    }
}