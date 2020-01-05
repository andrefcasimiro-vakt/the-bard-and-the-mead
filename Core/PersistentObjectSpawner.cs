using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{

    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjectPrefab;

        // Static variable lives with the application, not with the class
        static bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned)
            {
                return;
            }

            SpawnPersistentObjects();
            hasSpawned = true;
        }

        void SpawnPersistentObjects()
        {
            GameObject persistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }

    }

}
