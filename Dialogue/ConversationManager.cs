using UnityEngine;
using UnityEngine.UI;
using RPG.AI;

using RPG.Dialogue.Core;

namespace RPG.Dialogue {

    public class ConversationManager : MonoBehaviour {

        public DialogueUI dialogueUi;

        public ScriptableObject dialogue;

        [SerializeField]
        string dialogueOwnerName;

        [SerializeField]
        GameObject actionUI;

        [SerializeField]
        GameObject dialogueOwner;

        public GameObject camera;

        bool dialogueInProgress = false;
        bool playerIsNear = false;

        void Start()
        {
            camera.SetActive(false);

            if (string.IsNullOrEmpty(dialogueOwnerName))
            {
                dialogueOwnerName = dialogueOwner.name;
            }
        }

        public void OnDialogueStart()
        {
            dialogueOwner.GetComponent<AIController>().SetState(StateMachineEnum.CHAT);

            actionUI.GetComponent<Text>().text = "";

            // Set a new dialogue for the dialogue ui
            dialogueUi.SetConversation(new ConversationTree((DialogueContainer) dialogue), dialogueOwnerName, camera);

            dialogueInProgress = true;

        }

        private void Update()
        {

            if (playerIsNear)
            {
                    if (Input.GetKeyDown(KeyCode.E) && !dialogueInProgress)
                    {
                        OnDialogueStart();
                    }
            }

            if (dialogueInProgress)
            {
                if (dialogueUi.conversationTree == null || dialogueUi.conversationTree.dialogueContainer == null)
                {
                    OnDialogueFinish();
                }
            }
        }

        public void OnDialogueFinish()
        {
            // Restore AI previous state that was set before the conversation took place
            dialogueOwner.GetComponent<AIController>().SetPreviousState();

            dialogueInProgress = false;

            DrawGUI();
        }

        // UI Action Popup Logic
        void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.tag == "Player" && dialogueInProgress == false)
            {
                playerIsNear = true;
                DrawGUI();
            }
        }

        void OnTriggerExit(Collider col)
        {
            if (col.gameObject.tag == "Player")
            {
                actionUI.GetComponent<Text>().text = "";
                playerIsNear = false;
            }
        }


        public void DrawGUI()
        {
            actionUI.GetComponent<Text>().text = dialogueOwnerName != ""
                ? "E) Talk with " + dialogueOwnerName
                : "E) Talk";
        }

    }
}
