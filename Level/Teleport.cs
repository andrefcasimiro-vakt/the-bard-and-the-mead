using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;   
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine.UI;

namespace RPG.Core
{
    public class Teleport : MonoBehaviour
    {
        [Header("Don't forget that portals need to be on the root of the hierarchy")]

        [Header("UI Settings")]
        public string displayText = "E) Go to mines";
        GameObject actionPopup; 

        [Header("Next Scene Settings")]
        public int sceneToLoad = -1;
        public string destinationPortalName;

        [Header("Current Scene Settings")]
        public Transform spawnPoint;

        [Header("Transition Settings")]
        public float fadeTimeBetweenScenes = 0.25f;

        [Header("Events")]
        public UnityEvent OnAction;

        public LayerMask layersToConsider;

        Fader fader;
        SavingWrapper savingWrapper;

        bool isNear = false;

        private void Start()
        {
            fader = FindObjectOfType<Fader>();
            savingWrapper = FindObjectOfType<SavingWrapper>();

            actionPopup = GameObject.FindWithTag("ActionPopup");
        }

        void Update()
        {
            if (isNear)
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

                       if (Input.GetKeyDown(KeyCode.E))
                        {
                            OnAction.Invoke();

                            actionPopup.GetComponent<Text>().text = "";

                            StartCoroutine(Transition());
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                isNear = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                actionPopup.GetComponent<Text>().text = "";
                isNear = false;
            }
        }

        private IEnumerator Transition()
        {
            // Preserve this portal for now
            DontDestroyOnLoad(this.gameObject);

            // Fade Out
            yield return fader.FadeOut(fadeTimeBetweenScenes);

            // Save current level state
            savingWrapper.SaveMethod();

            // Load Async
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            // Load next level state
            savingWrapper.LoadMethod();

            print("He");

            // Update player position based on future portal spawnpoint
            UpdatePlayer(GetOtherPortal());

            // Save as a checkpoint
            savingWrapper.SaveMethod();

            // Fade In
            yield return fader.FadeIn(fadeTimeBetweenScenes);

            // Finally, destroy the old portal
            Destroy(this.gameObject);
        }

        void UpdatePlayer(Teleport otherPortal)
        {

            GameObject player = GameObject.FindWithTag("Player");

            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        Teleport GetOtherPortal()
        {
            // We modify this portal name before we destroy it so we can use the same portal name across
            // different scenes
            this.gameObject.name = this.gameObject.name + "_";

            return GameObject.Find(destinationPortalName).GetComponent<Teleport>();
        }
    }
}
