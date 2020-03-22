using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Control;

namespace RPG.EventSystem { 
    public class DisplayNotification : Template
    {
        [Header("---")]
        [Header("Displays a notification on the center of the screen")]


        public string notificationText;

        [Header("SFX")]
        public AudioClip notificationSfx;

        GameObject player;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
        }

        public void Display()
        {
            StartCoroutine(Dispatch());
        }

        public override IEnumerator Dispatch()
        {
            yield return StartCoroutine(ProcessNotification());
        }

        IEnumerator ProcessNotification()
        {

            if (notificationSfx)
            {
                AudioSource audioSource = this.gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
                audioSource.PlayOneShot(notificationSfx);
            }

            ShowPopup(notificationText);

            // End event
            yield return null;
        }

        void ShowPopup(string text) 
        {
            GameObject popup = GameObject.FindWithTag("Popup");

            if (popup == null)
            {
                return;
            }

            popup.GetComponent<Animator>().Play("Idle");

            popup.GetComponent<Animator>().Play("Show");
            popup.transform.GetChild(0).gameObject.GetComponent<Text>().text = text;

        }
    }
}
