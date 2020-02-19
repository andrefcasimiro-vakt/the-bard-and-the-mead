using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.EventSystem { 
    public class ActivateGameObject : Template
    {
        [Header("---")]
        [Header("Activates a gameobject at runtime")]

        public GameObject gameObject;

        [Header("Settings")]
        public bool inactiveAtStart = true;

        void Start()
        {
            if (inactiveAtStart)
            {
                gameObject.SetActive(false);
            }
        }

        public override IEnumerator Dispatch()
        {
            yield return StartCoroutine(Activate());
        }

        public IEnumerator Activate() {
            gameObject.SetActive(true);
            yield return null;
        }

    }
}
