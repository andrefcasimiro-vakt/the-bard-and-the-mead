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

        public LayerMask layersToConsider;

        bool isInRange = false;

        void Start()
        {
            actionPopup = GameObject.FindWithTag("ActionPopup");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" && isCollected == false)
            {

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
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition).origin,
                                    Camera.main.ScreenPointToRay(Input.mousePosition).direction, out hit, 100,
                                    layersToConsider))
                {
                    // Allow item to be picked since user is eyeing it
                    if (hit.transform.gameObject == this.gameObject)
                    {
                        actionPopup.GetComponent<Text>().text = displayText;

                        if (Input.GetKeyDown(KeyCode.E)) {
                            actionPopup.GetComponent<Text>().text = "";

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
                    else
                    // Make text to pick up invisible since we looked away
                    {
                        actionPopup.GetComponent<Text>().text = "";
                    }
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
