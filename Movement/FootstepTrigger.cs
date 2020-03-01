using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Movement {
    [RequireComponent(typeof(AudioSource))]
    public class FootstepTrigger : MonoBehaviour
    {
        [SerializeField]
        FootstepManager footstepManager;

        float volume = 0.2f;


        AudioSource audioSource => GetComponent<AudioSource>();


        void Start() {
            audioSource.volume = volume;
            footstepManager = GameObject.FindWithTag("FootstepManager").GetComponent<FootstepManager>();
        }

        void OnTriggerEnter(Collider col)
        {
            Ground ground = footstepManager.GetGround(col.gameObject.layer);

            if (ground != null)
            {
                AudioClip clip = ground.clips[(int)Random.Range(0, ground.clips.Length)];
                Debug.Log(clip);
                audioSource.clip = clip;
                audioSource.Play();
            }
        }

    }
}
