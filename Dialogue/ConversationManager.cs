using UnityEngine;
using UnityEngine.UI;
using RPG.AI;
using RPG.Dialogue.Core;
using RPG.Events;
using System.Collections.Generic;

namespace RPG.Dialogue {

    public class ConversationManager : MonoBehaviour {

        [Header("UI")]
        [Tooltip("The dialogue canvas user-interface")]
        public DialogueUI dialogueUI;

        [Tooltip("The prompt action canvas that tells the player how to initiate the dialogue. E. g. Press E to talk.")]
        [SerializeField]  GameObject actionUI;

        [Header("Conversation")]
        public ScriptableObject dialogue;

        [Header("Conversation Settings")]
        [SerializeField] GameObject dialogueOwner;
        [SerializeField] string dialogueOwnerName;
        [SerializeField] GameObject defaultCutsceneCamera;

        [Header("Events")]
        [SerializeField]
        List<E_Event> events = new List<E_Event>();


        bool dialogueInProgress = false;
        bool playerIsNear = false;

        void Start()
        {
            defaultCutsceneCamera.SetActive(false);

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
            dialogueUI.SetConversation(
                new ConversationTree(
                    (DialogueContainer) dialogue,
                    dialogueOwnerName,
                    defaultCutsceneCamera,
                    events
                )
            );

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
                if (dialogueUI.conversationTree == null || dialogueUI.conversationTree.dialogueContainer == null)
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
