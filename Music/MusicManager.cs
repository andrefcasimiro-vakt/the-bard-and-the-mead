using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Music { 

    public enum MUSIC_STATE {
        FIELD,
        BATTLE,
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
                default:
                    break;
            }
        }

        public void PlayMusic(string nextState)
        {
            MUSIC_STATE _nextState = (MUSIC_STATE)Enum.Parse(typeof(MUSIC_STATE), nextState);

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
        }

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
