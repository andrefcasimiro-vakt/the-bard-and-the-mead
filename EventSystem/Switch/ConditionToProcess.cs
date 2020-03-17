using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Switch {
    public class ConditionToProcess : MonoBehaviour
    {
        public ICondition test;

        public bool inverse = false;

        [Tooltip("If set, will toggle the first children at the game object")]
        public bool childrenOnly = false;

        [Tooltip("If set, will toggle a component instead of the game object")]
        public MonoBehaviour componentToToggle;

        public void Update()
        {
            bool condition = test.Check();
            bool result = false;

            if (condition) {
                 result = inverse ? false : true;

            } else {
                 result = inverse ? true : false;
            }

            if (componentToToggle != null)
            {
                componentToToggle.enabled = result;
                return;
            }
            
            if (childrenOnly)
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(result);
                return;
            }

            this.gameObject.SetActive(result);
        }
    }
}
