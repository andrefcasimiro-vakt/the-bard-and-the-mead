using UnityEngine;
using UnityEngine.UI;
using RPG.Core;
using RPG.Control;

namespace RPG.Dialogue {
    public class ConversationManager : MonoBehaviour {
        [SerializeField]
        string dialogueOwnerName = "";

        [SerializeField]
        GameObject actionUI;

        [SerializeField]
        GameObject dialogueOwner;
        AIController aIController;
        Health dialogueOwnerHealth;

        GameObject player;
        
        bool enabled = true;
        bool dialogueInProgress = false;
        
        void Start()
        {
            player = GameObject.FindWithTag("Player");
        
            aIController = dialogueOwner.GetComponent<AIController>();
            dialogueOwnerHealth = dialogueOwner.GetComponent<Health>();
        }

        void Update()
        {
            // Disable conversation if character dies
            if (dialogueOwnerHealth.IsDead())
            {
                actionUI.GetComponent<Text>().text = "";
                this.gameObject.SetActive(false);
            }
        }

        public void OnDialogueStart()
        {
            if (dialogueInProgress)
            {
                return;
            }

            // Disable player components
            player.GetComponent<ComponentManager>().Disable();

            // Make player and dialogue owner face each other
            aIController.SetState(StateMachineEnum.CHAT);
            player.transform.LookAt(dialogueOwner.transform);

            // Cleanup Action UI popup text
            actionUI.GetComponent<Text>().text = "";

            // Set dialogueInProgress so we don't retrigger the conversation whilst it is happening
            dialogueInProgress = true;
        }

        public void OnDialogueFinish()
        {
            // Restore player components
            player.GetComponent<ComponentManager>().Enable();

            // Restore AI previous state that was set before the conversation took place
            aIController.SetPreviousState();

            // Reset dialogueInProgress to false now that the conversation has ended
            dialogueInProgress = false;
        }

        // UI Action Popup Logic
        void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.tag == "Player" && dialogueInProgress == false)
            {
                actionUI.GetComponent<Text>().text = dialogueOwnerName != "" 
                    ? "E) Talk with " + dialogueOwnerName 
                    : "E) Talk";
            }
        }

        void OnTriggerExit(Collider col)
        {
            if (col.gameObject.tag == "Player")
            {
                actionUI.GetComponent<Text>().text = "";
            }
        }

    }
}
