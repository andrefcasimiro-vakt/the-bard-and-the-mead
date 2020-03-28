using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.EventSystem {

    /// Utility to hold sequential list of events so we don't end up having a gameobject filled with event components
    public class EventPage : MonoBehaviour
    {
        [Header("Event Page Set-up")]
        public string NOTE = "Put all your Events as children of this component";

        public enum ProcessType
        {
            EVENT,
            AUTOMATIC,
            PARALLEL,
            ON_TRIGGER_ENTER,
        }

        [Header("Event Processing Options")]
        public ProcessType processType;

        [Header("Event Conditions")]
        public LocalSwitch localSwitch;

        List<Event> events = new List<Event>();

        private bool isRunning = false;

        // Register all events in the component 
        void Awake() {
            foreach (Transform t in this.transform) {
                foreach (MonoBehaviour script in t.gameObject.GetComponents<MonoBehaviour>())
                {
                    Event e = script as Event;
                    if (e != null) {
                        events.Add(e);
                    }
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
            StartCoroutine(DispatchAllEvents());
        }

        
        public IEnumerator DispatchAllEvents()
        {
            foreach(Event ev in events)
            {
                isRunning = true;
                yield return StartCoroutine(ev.DispatchEvents());
            }

            isRunning = false;
            yield return null;
        }


    }

}
