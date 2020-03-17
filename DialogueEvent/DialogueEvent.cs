using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.Inventory;

namespace RPG.Events
{

    [System.Serializable]
    public class DialogueEvent : MonoBehaviour
    {
        public string eventId = "01";
        public UnityEvent OnEvent;

    }
}
