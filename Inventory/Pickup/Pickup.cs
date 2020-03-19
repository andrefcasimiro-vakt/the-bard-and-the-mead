using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using RPG.Saving;

namespace RPG.Inventory
{
    public class Pickup : MonoBehaviour, ISaveable
    {
        public string displayText = "E) Pick ";
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
                                    Physics.DefaultRaycastLayers))
                {
                    if (hit.transform.gameObject == this.gameObject)
                    {
                        HandlePickable();
                    }
                }
                else
                {
                                    actionPopup.GetComponent<Text>().text = "";
                }

            }
        }

        void HandlePickable()
        {
           actionPopup.GetComponent<Text>().text = displayText + gameObject.name;

            if (Input.GetKeyDown(KeyCode.E)) {

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


            Destroy(this.gameObject);

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

                // If item was collected, deactivate this gameObject
                Deactivate();
            }
        }

        public void OnCleanState() {
            
        }
    }

}
