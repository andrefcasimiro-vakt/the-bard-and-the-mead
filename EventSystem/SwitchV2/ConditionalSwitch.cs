using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Switch.V2 {

    public enum Action {
        ACTIVATE,
        DEACTIVATE
    }
    /// Evaluates a local switch
    public class ConditionalSwitch : MonoBehaviour
    {
        public string description = "";

        public LocalSwitch localSwitch;

        [Header("GameObject to activate / deactivate")]
        public GameObject target;

        [Header("Actions for each local switch state (ON / OFF)")]
        public Action on;
        public Action off;
        

        void Update()
        {
            if (localSwitch.GetLocalSwitch() == true)
            {
                target.SetActive(on == Action.ACTIVATE ? true : false);
            }
            else
            {
                target.SetActive(off == Action.ACTIVATE ? true : false);
            }
        }
    }
}
