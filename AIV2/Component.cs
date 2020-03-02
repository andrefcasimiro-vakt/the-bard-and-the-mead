using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.AIV2 {

    public class Component : MonoBehaviour {

        public virtual IEnumerator Dispatch() {
            yield return null;
        }

    }

}
