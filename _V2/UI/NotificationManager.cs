using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.V2.UI {

    [System.Serializable]
    public class NotificationSound {
        public string key;
        public AudioClip clip;
        public float time;
    }

    /// Responsible for waking up the notification UI
    public class NotificationManager : MonoBehaviour
    {
        [Header("Notification Settings")]
        [Tooltip("The duration of the notification")]
        public float duration = 5f;
        [Tooltip("If true, will disappear automatically after starting. Disable if you want to trigger the Sleep state manually.")]
        public bool automatic = true;

        [Header("Animator Parameters")]
        public string SHOW_NOTIFICATION_BOOL_PARAMETER = "IsDisplaying";
        Animator animator => GetComponent<Animator>();

        [Header("SFX")]
        public List<NotificationSound> notificationSounds = new List<NotificationSound>();
        AudioSource audioSource => GetComponent<AudioSource>();

        // If defined, will use a clip when the exiting animation is triggered
        public AudioClip exitClip;

        [Header("Debug")]
        [Tooltip("If true, will test the component by calling it after 2 seconds on the start method")]
        public bool debug = false;

        void Start()
        {
            if (animator == null || audioSource == null)
            {
                Debug.LogError("Incorrect setup for Notification Manager instance");
            }

            if (debug)
            {
                Invoke("Wake", 2f);
            }
        }

        public void Wake()
        {
            animator.SetBool(SHOW_NOTIFICATION_BOOL_PARAMETER, true);

            // Iterate through the notification sound list
            StartCoroutine(HandleNotificationSounds());

            if (automatic)
            {
                // Go to sleep after the duration time has elapsed
                Invoke("Sleep", duration);
            }
        }

        public void Sleep()
        {
            if (exitClip != null)
            {
                audioSource.PlayOneShot(exitClip);
            }

            animator.SetBool(SHOW_NOTIFICATION_BOOL_PARAMETER, false);
        }

        IEnumerator HandleNotificationSounds()
        {
            foreach (NotificationSound notificationSound in notificationSounds)
            {
                yield return new WaitForSeconds(notificationSound.time);
                audioSource.PlayOneShot(notificationSound.clip);
            }
        }
    }
}
