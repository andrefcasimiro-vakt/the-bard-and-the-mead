using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace RPG.AIV3 {

    public class OnTargetEnter : MonoBehaviour {

        public UnityEvent OnEnter;
        public UnityEvent OnExit;

        public string[] candidateTags;

        private void OnTriggerEnter(Collider other)
        {
            if (candidateTags.Contains(other.tag))
            {
                OnEnter.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (candidateTags.Contains(other.tag))
            {
                OnExit.Invoke();
            }
        }


    }
}
