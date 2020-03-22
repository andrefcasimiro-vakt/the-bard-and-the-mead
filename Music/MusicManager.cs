using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Music { 

    public enum MUSIC_STATE {
        FIELD,
        BATTLE,
        NONE,
    }

    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour
    {

        public MUSIC_STATE state = MUSIC_STATE.FIELD;

        public AudioClip fieldMusic;
        public AudioClip battleMusic;

        AudioSource audioSource => GetComponent<AudioSource>();
        AudioClip currentBGM;

        void Start() {
            if (state != MUSIC_STATE.NONE)
                PlayMusic();
        }

        public void PlayMusic()
        {
            switch (state)
            {
                case MUSIC_STATE.BATTLE:
                    SetCurrentBGM(battleMusic);
                    Play();
                break;
                case MUSIC_STATE.FIELD:
                    SetCurrentBGM(fieldMusic);
                    Play();
                break;
                case MUSIC_STATE.NONE:
                default:
                    break;
            }
        }

        public void PlayMusic(string nextState)
        {
            MUSIC_STATE _nextState = (MUSIC_STATE)Enum.Parse(typeof(MUSIC_STATE), nextState);

            // If states are the same, dont do nothing
            if (_nextState == this.state)
            {
                return;
            }

            switch (_nextState)
            {
                case MUSIC_STATE.BATTLE:
                    SetCurrentBGM(battleMusic);
                    Play();
                break;
                case MUSIC_STATE.FIELD:
                    SetCurrentBGM(fieldMusic);
                    Play();
                break;
                default:
                    break;
            }

            state = _nextState;
        }

        public void SetCurrentBGM(AudioClip currentBGM)
        {
            this.currentBGM = currentBGM;
        }

        public void Play()
        {
            audioSource.clip = currentBGM;

            StartCoroutine(Dispatch());
        }

        public void Stop()
        {
            state = MUSIC_STATE.NONE;
            audioSource.Stop();
        }

        public IEnumerator Dispatch() {
            yield return StartCoroutine(FadeOut());


            yield return StartCoroutine(FadeIn());

        }

        public IEnumerator FadeIn()
        {
            audioSource.Play();

            while (audioSource.volume < 1)
            {
                audioSource.volume += Time.deltaTime / 0.5f;
                yield return null; // Yield one frame;
            }

        }

        public IEnumerator FadeOut()
        {
            while (audioSource.volume > 0)
            {
                audioSource.volume -= Time.deltaTime / 0.5f;
                yield return null; // Yield one frame;
            }
        }

    }
}
