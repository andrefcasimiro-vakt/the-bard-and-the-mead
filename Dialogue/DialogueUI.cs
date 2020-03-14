using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using RPG.Events;

namespace RPG.Dialogue
{
    public class DialogueUI : MonoBehaviour
    {

        [HideInInspector] public ConversationTree conversationTree;
        public string dialogueOwnerName;

        public GameObject dialogueOwner;
        public GameObject dialogueText;
        public GameObject choicePanel;
        public GameObject choiceButtonPrefab;

        public GameObject arrow;

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

        public void SetConversation(ConversationTree conversationTree)
        {
            player.SetActive(false);
            this.conversationTree = conversationTree;
            this.conversationTree.dialogueCamera.SetActive(true);

            Toggle(true);

            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;

            repaint = true;
        }

        public void EndConversation()
        {
            player.SetActive(true);

            this.conversationTree.dialogueCamera.SetActive(false);

            this.conversationTree = null;

            Toggle(false);

            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;

            repaint = false;
        }

        private void Update()
        {
            if (conversationTree == null || conversationTree.dialogueContainer == null)
            {
                return;
            }

            if (repaint == true) { 
                ManageDialogue();
            }

            if (
                Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.E) ||
                Input.GetKeyDown(KeyCode.KeypadEnter) ||
                Input.GetButtonDown("Fire1")
            )
            {
                AdvanceOnClick();
            }
        }

        /// <summary>
        /// Advances conversation when there are no multiple choices
        /// </summary>
        public void AdvanceOnClick()
        {
            List<Choice> choices = conversationTree.GetChoices();

            if (choices.Count <= 1)
            {

                string id = choices.Count == 1 ? choices[0].targetGUID : null;

                Advance(id);
            }
        }


        private void ManageDialogue()
        {
            // Is Event?
            string eventId = conversationTree.GetEventId();
            if (eventId != null)
            {
                E_Event ev = null;
                foreach (E_Event e in conversationTree.events)
                {
                    if (e.eventId == eventId)
                    {
                        ev = e;
                    }
                }

                if (ev != null)
                {
                    ev.Dispatch();

                    // No choices possible in a event trigger node, so we advance the conversation.
                    AdvanceOnClick();
                    return;
                }
            }

            string currentDialogue = conversationTree.GetCurrentText();
            if (string.IsNullOrEmpty(currentDialogue))
            {
                // If no text is found in this node, it must be a ending conversation node.
                EndConversation();
                return;
            }

            dialogueOwner.GetComponent<Text>().text = conversationTree.dialogueOwnerName;
            dialogueText.GetComponent<Text>().text = currentDialogue;

            // Clear choice button panel:
            // Clean panel first
            foreach (Transform child in choicePanel.transform)
            {
                Destroy(child.gameObject);
            }

            List<Choice> choices = conversationTree.GetChoices();
            if (choices.Count > 1)
            {
                arrow.SetActive(false);

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
            else
            {
                arrow.SetActive(true);
            }

            // Careful with race conditions
            repaint = false;
        }

        void Advance(string targetGuid)
        {
            // No text left? End Conversation
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