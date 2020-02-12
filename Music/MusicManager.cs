using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Music { 

    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour
    {
        AudioSource audioSource => GetComponent<AudioSource>();
        AudioClip currentBGM;

        public void SetCurrentBGM(AudioClip currentBGM)
        {
            this.currentBGM = currentBGM;
        }

        public void Play()
        {
            audioSource.clip = currentBGM;

            audioSource.Play();
        }

        public void Stop()
        {
            audioSource.Pause();
        }

    }
}
