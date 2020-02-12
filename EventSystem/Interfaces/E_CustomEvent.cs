using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RPG.EventSystem
{
    
    [System.Serializable]
    public class E_CustomEvent : E_Template
    {
        public UnityEvent _event;

        public override IEnumerator Dispatch()
        {
            _event.Invoke();
            yield return null;
        }
    }
}
