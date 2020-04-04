using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.Saving;
using RPG.V2.Utils;

namespace RPG.V2.Pickup {

    /// Controls how an item (gold, potion, book) is picked up by the player
    public class Pickup_V2 : MonoBehaviour, ISaveable
    {

        [Header("Raycast Detection")]
        public LayerMask layersToConsider;

        [Header("Events")]
        public UnityEvent OnPointerEnter;
        public UnityEvent OnPointerExit;
        public UnityEvent OnPickup;

        [Tooltip("Sphere Collider OnEnter Event")]
        public UnityEvent OnPlayerEnter;
        [Tooltip("Sphere Collider OnExit Event")]
        public UnityEvent OnPlayerExit;

        [Header("Is Pickable?")]
        /// If picked up, object will be deactivated
        public bool deactivateOnPickup = true;

        [Header("Objects to deactivate on pickup")]
        public List<GameObject> gameObjectsToDeactivate = new List<GameObject>();

        bool isPicked = false;
        bool playerIsNear = false;

        Dictionary<string, bool> isPointerUp = new Dictionary<string, bool>();

        void Start()
        {
            isPointerUp.Add("currentState", false);
            isPointerUp.Add("previousState", false);
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.tag == "Player")
            {
                OnPlayerEnter.Invoke();
                playerIsNear = true;
            }
        }

        void OnTriggerExit(Collider col)
        {
            if (col.gameObject.tag == "Player")
            {
                OnPlayerExit.Invoke();
                playerIsNear = false;

                ResetPointers();
            }
        }

        void ResetPointers()
        {
            isPointerUp["previousState"] = false;
            isPointerUp["currentState"] = true;
        }

        void Update()
        {
            if (deactivateOnPickup && isPicked)
            {
                return;
            }

            if (playerIsNear == false)
            {
                return;
            }

            HandlePointer();

            if (isPointerUp["currentState"])
            {
                // Allow input event
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Emit On Pickup Event
                    OnPickup.Invoke();

                    if (deactivateOnPickup)
                    {
                        DeactivateComponents();
                        isPicked = true;
                    }
                }
            }
        }

        void HandlePointer()
        {
            isPointerUp["currentState"] = RaycastUtils.IsCameraLookingAtObject(this.gameObject, layersToConsider);

            // If state remains unchanged, don't invoke events
            if (isPointerUp["currentState"] == isPointerUp["previousState"])
            {
                return;
            }

            isPointerUp["previousState"] = isPointerUp["currentState"];

            if (isPointerUp["currentState"])
            {
                OnPointerEnter.Invoke();
            }
            else
            {
                OnPointerExit.Invoke();
            }
        }


        /// Deactivates sphere and mesh renderer components
        /// The first will stop player from interacting with this script
        /// The second will stop the renderer from showing the graphics
        public void DeactivateComponents()
        {
            SphereCollider sphereCollider = GetComponent<SphereCollider>();
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            
            if (sphereCollider != null)
            {
                sphereCollider.enabled = false;
            }

            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }

            // If we have any game objects that we wish to deactivate
            if (gameObjectsToDeactivate.Count >= 1)
            {
                foreach (GameObject go in gameObjectsToDeactivate)
                {
                    go.SetActive(false);
                }
            }
        }

        /// Saving
        public object CaptureState()
        {
            return isPicked;
        }
        public void RestoreState(object state)
        {
            isPicked = (bool)state;

            if (deactivateOnPickup)
            {
                if (isPicked)
                {
                    DeactivateComponents();
                }
            }
        }
        public void OnCleanState() {}
    }
}
