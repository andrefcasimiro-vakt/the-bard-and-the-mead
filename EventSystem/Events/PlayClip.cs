using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.EventSystem { 
    public class PlayClip : Template
    {
        [Header("---")]
        [Header("Plays an audioClip")]

        public AudioClip audioClip;
        public AudioSource audioSource;


        public override IEnumerator Dispatch()
        {
            yield return StartCoroutine(SetAudio());
        }

        IEnumerator SetAudio()
        {
            audioSource.clip = audioClip;

            audioSource.Play();
            audioSource.volume = 1f;

            // End event
            yield return null;
        }

    }
}
