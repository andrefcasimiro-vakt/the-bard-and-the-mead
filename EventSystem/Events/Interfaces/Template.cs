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
    public class Template : MonoBehaviour
    {
        public virtual IEnumerator Dispatch() {
            yield return null;
        }
    }
}
