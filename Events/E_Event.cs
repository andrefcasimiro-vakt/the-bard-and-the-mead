using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.Inventory;

namespace RPG.Events
{

    [System.Serializable]
    public class E_Event : MonoBehaviour
    {
        public string eventId = "01";

        public virtual void Dispatch() { }
    }
}
