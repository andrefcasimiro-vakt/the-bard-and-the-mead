using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.EventSystem {
    public class Event : MonoBehaviour
    {
        public enum ProcessType
        {
            EVENT,
            AUTOMATIC,
            PARALLEL,
        }

        [Header("Event Processing Options")]
        public ProcessType processType;

        [Header("Event Conditions")]
        public LocalSwitch localSwitch;

        [Header("List of events")]
        public List<E_Event> customEvents = new List<E_Event>();

        private bool isRunning = false;

        private void Start()
        {
            if (processType == ProcessType.AUTOMATIC)
            {
                Initialize();
            }
        }

        private void Update()
        {
            if (isRunning) return;

            if (processType == ProcessType.PARALLEL)
            {
                Initialize();
            }
        }

        public void Initialize()
        {
            if (localSwitch && localSwitch.GetLocalSwitch() == true)
            {
                // Don't run event
                return;
            }

            isRunning = true;
            StartCoroutine(DispatchEvents());

        }

        IEnumerator DispatchEvents()
        {
            print("Initializing cutscene");
            foreach(E_Event _event in customEvents)
            {
                yield return StartCoroutine(_event.Dispatch());
            }

            isRunning = false;
        }



    }
}
