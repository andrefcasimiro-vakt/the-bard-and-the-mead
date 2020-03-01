using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.EventSystem { 
    public class Wait : Template
    {
        [Header("---")]
        [Header("Pauses event system during the given time")]

        public float timeToWait = 1f;

        public override IEnumerator Dispatch()
        {
            yield return StartCoroutine(Execute());
        }

        public IEnumerator Execute() {
            yield return new WaitForSeconds(timeToWait);
        }

    }
}
