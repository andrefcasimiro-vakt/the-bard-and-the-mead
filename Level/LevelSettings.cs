using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector;

namespace RPG.Level {

    public enum Environment
    {
        EXTERIOR,
        INTERIOR,
        DUNGEON,
    }

    public class LevelSettings : MonoBehaviour
    {

        public Environment environment;

        public float cameraProximity = 3f;

        // Private
        GameObject mainCamera;

        // Start is called before the first frame update
        void Start()
        {
            mainCamera = GameObject.FindWithTag("MainCamera");
        }

        void Update()
        {
            if (mainCamera != null)
            {
                mainCamera.GetComponent<vThirdPersonCamera>().defaultDistance = cameraProximity;
            }
        }


    }
}
