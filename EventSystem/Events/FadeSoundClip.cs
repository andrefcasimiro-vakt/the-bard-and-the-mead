using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace RPG.EventSystem
{
    public class FadeSoundClip : Template
    {
        [Header("---")]
        [Header("Fades a sound clip")]

        public AudioSource audioSource;

        [Header("Fade In Case")]
        public bool fadeIn = true;
        public AudioClip audioClip;

        [Header("Settings")]
        public float fadeSpeed = 0.5f;

        void Start() {
            if (audioSource == null)
            {
                // Attempt to find a gameobject with tag BGM_Manager
                audioSource = GameObject.FindWithTag("BGM_Manager").GetComponent<AudioSource>();
            }
        }

        public override IEnumerator Dispatch() {

            if (fadeIn) {
                audioSource.volume = 0;
                audioSource.clip = audioClip;
                audioSource.Play();

                yield return StartCoroutine(FadeIn());
            } else {
                yield return StartCoroutine(FadeOut());
            }
        }

        public IEnumerator FadeIn()
        {
            while (audioSource.volume < 1)
            {
                audioSource.volume += Time.deltaTime / fadeSpeed;
                yield return null; // Yield one frame;
            }

            yield return null;
        }

        public IEnumerator FadeOut()
        {
            while (audioSource.volume > 0)
            {
                audioSource.volume -= Time.deltaTime / fadeSpeed;
                yield return null; // Yield one frame;
            }
            yield return null; // Yield one frame;
        }
    }
}
