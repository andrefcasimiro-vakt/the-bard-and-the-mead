using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

namespace RPG.Events
{
    public class CustomEventOnKeydown : MonoBehaviour
    {
        public string displayText = "E) Action... ";
        GameObject actionPopup; 

        public UnityEvent OnFacingObject;
        public UnityEvent OnKeyDown;

        bool isInRange = false;
        bool isRunning = false;

        public LayerMask layersToConsider;

        void Start()
        {
            actionPopup = GameObject.FindWithTag("ActionPopup");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                isInRange = true;
            }
        }

        void Update()
        {
            if (isInRange)
            {
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition).origin,
                                    Camera.main.ScreenPointToRay(Input.mousePosition).direction, out hit, 100,
                                    layersToConsider))
                {
                    // Allow item to be picked since user is eyeing it
                    if (hit.transform.gameObject == this.gameObject && !isRunning)
                    {
                        HandleEvent();
                    }
                    else
                    // Make text to pick up invisible since we looked away
                    {
                        actionPopup.GetComponent<Text>().text = "";
                    }
                }
            }
        }

        void HandleEvent()
        {
            actionPopup.GetComponent<Text>().text = displayText;

            if (Input.GetKeyDown(KeyCode.E) && !isRunning) {
                isRunning = true;

                OnKeyDown.Invoke();
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                isInRange = false;
                actionPopup.GetComponent<Text>().text = "";
                
                isRunning = false;
            }
        }


    }

}
