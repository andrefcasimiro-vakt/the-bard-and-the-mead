using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.Inventory;

namespace RPG.Events {

    public class E_AddItem : E_Event
    {
        public GameObject target;
        public ScriptableItem item;

        public override void Dispatch()
        {

            Debug.Log("Event triggered. Something happens here.");

        }

    }
}
