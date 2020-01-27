using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace RPG.Core
{
    public class Portal : MonoBehaviour
    {

        [Header("Next Scene Settings")]
        public string destinationPortalName;
        public int sceneToLoad = -1;

        [Header("Current Scene Settings")]
        public Transform spawnPoint;

        [Header("Transition Settings")]
        public float fadeTimeBetweenScenes = 0.25f;

        Fader fader;
        SavingWrapper savingWrapper;

        private void Start()
        {
            fader = FindObjectOfType<Fader>();
            savingWrapper = FindObjectOfType<SavingWrapper>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
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

            // Update player position based on future portal spawnpoint
            UpdatePlayer(GetOtherPortal());

            // Save as a checkpoint
            savingWrapper.SaveMethod();

            // Fade In
            yield return fader.FadeIn(fadeTimeBetweenScenes);

            // Finally, destroy the old portal
            Destroy(this.gameObject);
        }

        void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");

            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        Portal GetOtherPortal()
        {
            // We modify this portal name before we destroy it so we can use the same portal name across
            // different scenes
            this.gameObject.name = this.gameObject.name + "_";

            return GameObject.Find(destinationPortalName).GetComponent<Portal>();
        }
    }
}
