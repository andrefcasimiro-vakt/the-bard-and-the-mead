using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Movement {
    [RequireComponent(typeof(AudioSource))]
    public class FootstepTrigger : MonoBehaviour
    {
        [SerializeField]
        FootstepManager footstepManager;

        AudioSource audioSource => GetComponent<AudioSource>();
        int soundIndex = 0;

        void OnTriggerEnter(Collider col)
        {
            Ground ground = footstepManager.GetGround(col.gameObject.layer);

            if (ground != null)
            {
                if (soundIndex >= ground.clips.Length)
                {
                    soundIndex = 0;
                }

                audioSource.clip = ground.clips[soundIndex];
                audioSource.Play();

                soundIndex++;
            }
        }

    }
}
