using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using RPG.AIV3;
using RPG.Dialogue;
using RPG.Dialogue.Core;
using RPG.Events;
using RPG.Control;
using System.Collections.Generic;
using RPG.Saving;
using RPG.V2.UI.Utils.Interfaces;

namespace RPG.Dialogue {

    public class Dialogue : IDisplayVariable {

        [Header("UI")]
        [Tooltip("The dialogue canvas user-interface")]
        public DialogueUI dialogueUI;

        [Header("Conversation")]
        public RandomDialogue[] randomConversations;
        public ScriptableObject conversation;

        [Header("Conversation Settings")]
        [SerializeField] GameObject dialogueOwner;
        [SerializeField] string dialogueOwnerName;
        [SerializeField] GameObject dialogueCamera;

        [Header("Events")]
        public UnityEvent OnDialogueStart;
        public UnityEvent OnDialogueFinish;

        [Tooltip("If we provide a gameobject for the eventsGameObject, it will search on these gameObject for events instead of this gameObject")]
        public List<GameObject> eventsGameObject = new List<GameObject>();
        List<DialogueEvent> events = new List<DialogueEvent>();

        AI_Core_V3 dialogueOwnerAI;
        bool inProgress = false;

        void Awake()
        {
            List<MonoBehaviour> monoBehaviours = new List<MonoBehaviour>();

            // If eventsGameObject is provided, look for events there
            // Keeps this dialogue gameObject better organized to have events handled on a children gameObject
            if (eventsGameObject.Count <= 0)
            {
                // Search for all monobehaviours on this gameObject
                foreach (MonoBehaviour m in this.gameObject.GetComponents<MonoBehaviour>())
                {
                    monoBehaviours.Add(m);
                }
            }
            else
            {
                // Search for each possible event gameobject of the list and extract each of their monobehaviours
                foreach (GameObject g in eventsGameObject)
                {
                    foreach (MonoBehaviour m in g.GetComponents<MonoBehaviour>())
                    {
                        monoBehaviours.Add(m);
                    }
                }
            }
            
            if (monoBehaviours.Count >= 1)
            {
                foreach (MonoBehaviour script in monoBehaviours)
                {
                    DialogueEvent s = script as DialogueEvent;
                    if (s != null) {
                        events.Add(s);
                    }
                }
            }
        }

        void Start()
        {
            dialogueCamera.SetActive(false);

            dialogueOwnerAI = dialogueOwner.GetComponent<AI_Core_V3>();
        }

        void SetDialogueOwnerName()
        {
            // Setup the dialogue owner name to be displayed in the UI
            if (string.IsNullOrEmpty(dialogueOwnerName))
            {
                dialogueOwnerName = dialogueOwner.name;
            }
        }

        public void InitializeDialogue()
        {
            SetDialogueOwnerName();

            dialogueOwnerAI.SetState(AGENT_STATE.TALKING);


            ScriptableObject chosenConversation = conversation;
            // If has random dialogue
            if (randomConversations.Length >= 1)
            {
                float chance = UnityEngine.Random.Range(0, 1f);
                foreach (RandomDialogue r in randomConversations)
                {
                    if (chance <= r.maxChance && chance >= r.minChance)
                    {
                        chosenConversation = r.conversation;
                        break;
                    }
                }
            }

            dialogueUI.SetConversation(
                new ConversationTree(
                    (DialogueContainer) chosenConversation,
                    dialogueOwnerName,
                    dialogueCamera,
                    events
                )
            );

            OnDialogueStart.Invoke();
            inProgress = true;
        }

        void Update()
        {
            if (!inProgress)
            {
                return;
            }


            bool dialogueTreeHasEnded = dialogueUI.conversationTree == null || dialogueUI.conversationTree.dialogueContainer == null;
            if (dialogueTreeHasEnded)
            {
                FinishDialogue();                
            }
        }

        public void FinishDialogue()
        {
            dialogueOwnerAI.SetState(AGENT_STATE.PATROL);
            
            OnDialogueFinish.Invoke();
            inProgress = false;
        }

        /// UI
        public override string GetVariable()
        {
            return dialogueOwnerName.ToString();   
        }
    }
}
