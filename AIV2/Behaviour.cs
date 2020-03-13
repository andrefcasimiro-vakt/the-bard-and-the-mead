using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.AIV2 {

    public class Behaviour : MonoBehaviour {

        public virtual IEnumerator Dispatch() {
            yield return null;
        }

        public virtual IEnumerator Dispatch(FightBehaviour context) {
            yield return null;
        }

    }

}
