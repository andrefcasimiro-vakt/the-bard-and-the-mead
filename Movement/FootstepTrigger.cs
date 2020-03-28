using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Movement {
    [RequireComponent(typeof(AudioSource))]
    public class FootstepTrigger : MonoBehaviour
    {
        [SerializeField]
        FootstepManager footstepManager;

        float volume = 0.85f;


        AudioSource audioSource => GetComponent<AudioSource>();


        void Start() {
            audioSource.volume = volume;

            FootstepManager fm =  GameObject.FindWithTag("FootstepManager").GetComponent<FootstepManager>();
        
            if (fm != null)
            {
                footstepManager = fm;
            }
        }

        void OnTriggerEnter(Collider col)
        {
            Ground ground = footstepManager.GetGround(col.gameObject.layer);

            if (ground != null)
            {
                AudioClip clip = ground.clips[(int)Random.Range(0, ground.clips.Length)];
                audioSource.clip = clip;
                audioSource.Play();
            }
        }

    }
}
