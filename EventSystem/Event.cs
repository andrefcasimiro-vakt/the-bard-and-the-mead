﻿using System.Collections;
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
            ON_TRIGGER_ENTER,
        }

        [Header("NOTE")]
        public string note = "If using event page, minimize this monobehaviour";

        [Header("Event Processing Options")]
        public ProcessType processType;

        [Header("Event Conditions")]
        public LocalSwitch localSwitch;

        [Header("List of events")]
        List<Template> customEvents = new List<Template>();

        private bool isRunning = false;

        // Register all events in the component 
        void Awake() {
            foreach (MonoBehaviour script in gameObject.GetComponents<MonoBehaviour>())
            {
                Template s = script as Template;
                if (s != null) {
                    customEvents.Add(s);
                }
            }
        }

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

        public void OnTriggerEnter(Collider col)
        {
            if (processType == ProcessType.ON_TRIGGER_ENTER && col.gameObject.tag == "Player")
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

        public IEnumerator DispatchEvents()
        {
            foreach(Template ev in customEvents)
            {
                yield return StartCoroutine(ev.Dispatch());
            }

            isRunning = false;
        }
        
    }
}
