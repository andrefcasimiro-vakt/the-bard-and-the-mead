using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using RPG.Saving;

namespace RPG.Inventory
{
    public class Chest : MonoBehaviour, ISaveable
    {
        public string displayText = "E) To open chest";
        GameObject actionPopup; 

        [SerializeField]
        ScriptableItem[] itemsToPick;

        public bool isCollected = false;

        public UnityEvent OnOpen;

        bool isInRange = false;

        void Start()
        {
            actionPopup = GameObject.FindWithTag("ActionPopup");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" && isCollected == false)
            {
                actionPopup.GetComponent<Text>().text = displayText;

                isInRange = true;
            }
        }

        void Update()
        {
            if (isCollected)
            {
                return;
            }

            if (isInRange)
            {
                if (Input.GetKeyDown(KeyCode.E)) {
                    GetComponent<Animator>().SetTrigger("OPEN");

                    isCollected = true;

                    GameObject player = GameObject.FindWithTag("Player");
                    foreach (ScriptableItem item in itemsToPick)
                    {
                        player.GetComponent<CharacterInventory>().Add(item);
                    }

                    OnOpen.Invoke();

                    Deactivate();
                }

            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                isInRange = false;
                actionPopup.GetComponent<Text>().text = "";
            }
        }

        void Deactivate()
        {

            if (actionPopup != null) actionPopup.GetComponent<Text>().text = "";

            // If item was collected, deactivate this gameObject
            GetComponent<SphereCollider>().enabled = false;

        }

        public object CaptureState()
        {
            return isCollected;
        }

        public void RestoreState(object state)
        {
            bool collected = (bool)state;

            isCollected = collected;

            if (collected == true)
            {
                GetComponent<Animator>().SetTrigger("OPEN");

                // If item was collected, deactivate this gameObject
                Deactivate();
            }
        }

        public void OnCleanState() {
            
        }
    }

}
