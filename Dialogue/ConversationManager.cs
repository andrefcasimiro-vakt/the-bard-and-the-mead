using UnityEngine;
using UnityEngine.UI;
using RPG.AIV3;
using RPG.Dialogue.Core;
using RPG.Events;
using RPG.Control;
using System.Collections.Generic;
using RPG.Saving;

namespace RPG.Dialogue {

    public class ConversationManager : MonoBehaviour, ISaveable {

        [Header("UI")]
        [Tooltip("The dialogue canvas user-interface")]
        public GameObject dialogueUi;
        GameObject instantiatedDialogueUi;
        [HideInInspector] public DialogueUI dialogueUI;

        [Tooltip("The prompt action canvas that tells the player how to initiate the dialogue. E. g. Press E to talk.")]
        [SerializeField] GameObject actionUI;
        GameObject instantiatedActionUI;

        [Header("Conversation")]
        public ScriptableObject dialogue;

        [Header("Conversation Settings")]
        [SerializeField] GameObject dialogueOwner;
        [SerializeField] string dialogueOwnerName;
        [SerializeField] GameObject defaultCutsceneCamera;

        [Header("SFX")]
        public AudioClip popupGui;

        public bool conversationHasOccurred = false;

        List<DialogueEvent> events = new List<DialogueEvent>();

        public bool dialogueInProgress = false;
        bool playerIsNear = false;

        void Awake()
        {
            foreach (MonoBehaviour script in gameObject.GetComponents<MonoBehaviour>())
            {
                DialogueEvent s = script as DialogueEvent;
                if (s != null) {
                    events.Add(s);
                }
            }
        }

        void Start()
        {
            defaultCutsceneCamera.SetActive(false);

            instantiatedDialogueUi = Instantiate(dialogueUi);
            
            // instantiatedActionUI = Instantiate(actionUI).gameObject.transform.GetChild(0).gameObject;
            instantiatedActionUI = GameObject.FindWithTag("ActionPopup");

            if (string.IsNullOrEmpty(dialogueOwnerName))
            {
                dialogueOwnerName = dialogueOwner.name;
            }
        }

        public void OnDialogueStart()
        {
            instantiatedDialogueUi.SetActive(true);
            dialogueUI = instantiatedDialogueUi.GetComponent<DialogueUI>();

            dialogueOwner.GetComponent<AI_Core_V3>().SetState(AGENT_STATE.TALKING);


            instantiatedActionUI = GameObject.FindWithTag("ActionPopup");
            instantiatedActionUI.GetComponent<Text>().text = "";

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
                        GetComponent<AudioSource>().PlayOneShot(popupGui);
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
            // TODO: Add previous state
            // Restore AI previous state that was set before the conversation took place
            dialogueOwner.GetComponent<AI_Core_V3>().SetState(AGENT_STATE.PATROL);

            instantiatedDialogueUi.SetActive(false);
            DrawGUI();

            instantiatedActionUI = GameObject.FindWithTag("ActionPopup");
            if (instantiatedActionUI != null) {
                instantiatedActionUI.GetComponent<Text>().text = "";
            }


            conversationHasOccurred = true;
            dialogueInProgress = false;
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
                if (instantiatedActionUI != null) instantiatedActionUI.GetComponent<Text>().text = "";
                playerIsNear = false;
            }
        }

        public void OnDisable() {
            if (instantiatedActionUI != null) instantiatedActionUI.GetComponent<Text>().text = "";

        }        

        public void DrawGUI()
        {
            if (instantiatedActionUI != null) {
                instantiatedActionUI.GetComponent<Text>().text = dialogueOwnerName != ""
                    ? "E) Talk with " + dialogueOwnerName
                    : "E) Talk";
            }
        }

        
        public object CaptureState()
        {
            return conversationHasOccurred;
        }

        public void RestoreState(object state)
        {
            conversationHasOccurred = (bool)state;
        }

        public void OnCleanState() {
            conversationHasOccurred = false;
        }
    }
}
