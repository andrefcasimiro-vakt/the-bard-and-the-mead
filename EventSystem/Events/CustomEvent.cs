using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.EventSystem { 
    public class CustomEvent : Template
    {
        [Header("---")]
        [Header("Generic event to interact with monobehaviour components")]

        public UnityEvent ev;

        public override IEnumerator Dispatch()
        {
            yield return StartCoroutine(Execute());
        }

        public IEnumerator Execute() {
            ev.Invoke();
            yield return null;
        }

    }
}
