using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Dialogue
{
    public class DialogueUI : MonoBehaviour
    {

        public ConversationTree conversationTree;
        public string dialogueOwnerName;

        public GameObject dialogueOwner;
        public GameObject dialogueText;
        public GameObject choicePanel;
        public GameObject choiceButtonPrefab;

        public GameObject arrow;

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

            if (
                Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.E) ||
                Input.GetKeyDown(KeyCode.KeypadEnter) ||
                Input.GetButtonDown("Fire1")
            )
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
            string currentDialogue = conversationTree.GetCurrentText();
            if (string.IsNullOrEmpty(currentDialogue))
            {
                // If no text is found in this node, it must be a ending conversation node.
                EndConversation();
                return;
            }


            dialogueOwner.GetComponent<Text>().text = dialogueOwnerName;
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