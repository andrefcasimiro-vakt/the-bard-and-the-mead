using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Music
{
    [RequireComponent(typeof(MusicManager))]
    public class BGMPlayer : MonoBehaviour
    {
        public AudioClip bgm;
        public bool playOnStart = true;

        MusicManager musicManager;

        private void Start()
        {
            musicManager = GetComponent<MusicManager>();

            if (playOnStart)
            {
                PlayBGM();
            }
        }

        public void PlayBGM()
        {
            musicManager.SetCurrentBGM(bgm);
            musicManager.Play();
        }

        public void StopBGM()
        {
            musicManager.Stop();
        }
    }
}
