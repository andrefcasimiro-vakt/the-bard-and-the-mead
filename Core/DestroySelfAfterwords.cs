using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class DestroySelfAfterwords : MonoBehaviour
    {
        [SerializeField]
        float timeToDestroy = 3f;
        float timeElapsed = 0f;

        private void Update()
        {
            if (timeElapsed > timeToDestroy)
            {
                Destroy(this.gameObject);
            }

            timeElapsed += Time.deltaTime;
        }
    }

}

