using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Utils { 
    
    
    public class OnTrigger : MonoBehaviour
    {

        [Header("Events")]
        public UnityEvent OnTriggerEnterEvent;
        public UnityEvent OnTriggerStayEvent;
        public UnityEvent OnTriggerExitEvent;

        public string[] candidateTags;

        public void OnTriggerEnter(Collider col)
        {
            if (candidateTags.Contains(col.gameObject.tag))
            {
                OnTriggerEnterEvent.Invoke();
            }
        }
        public void OnTriggerStay(Collider col)
        {
            if (candidateTags.Contains(col.gameObject.tag))
            {
                OnTriggerStayEvent.Invoke();
            }
        }
        public void OnTriggerExit(Collider col)
        {
            if (candidateTags.Contains(col.gameObject.tag))
            {
                OnTriggerExitEvent.Invoke();
            }
        }
    }
}
