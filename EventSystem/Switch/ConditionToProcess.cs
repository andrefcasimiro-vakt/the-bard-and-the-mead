using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.Dialogue;

namespace RPG.Switch {

    [System.Serializable]
    public class ConditionTest
    {
        /// The identifier
        public string key;
        /// If set, will invert the condition
        public bool invert;
        /// The actual test
        public ICondition test;

        public ConditionTest(string key, bool invert, ICondition test)
        {
            this.key = key;
            this.invert = invert;
            this.test = test;
        }

    }


    public class ConditionToProcess : MonoBehaviour
    {
        public ConditionTest[] tests;

        [Header("Optimizations")]
        public bool useRange = false;
        [Range(0, 100)] public float minimumRangeToPlayerBeforeBeginProcessing = 10f;

        [Header("Conditional Rendering Options")]
        [Tooltip("Start children deactivated")]
        // You won't always want this. Remember that if you have any conditions depending on evaluating localSwitches on Awake (because of the OnLoading Saved Games)
        // This evaluates only once if a switch is ON/OFF, so mind it for now.
        public bool deactivateChildrenOnAwake = false;

        [Tooltip("If set, will toggle the first children at the game object")]
        public bool childrenOnly = false;
        [Tooltip("If any of these are set, will toggle a component instead of the game object")]
        public SphereCollider colliderToToggle;
        public MonoBehaviour componentToToggle;

        [Header("Debugger")]
        public bool useDebug;

        // Private
        GameObject player;


        public ConversationManager conversationManager => GetComponent<ConversationManager>();
        [Header("Conversation Settings")]
        public bool checkConditionWhileInDialogue = false;

        void Awake()
        {
            if (deactivateChildrenOnAwake)
            {
                foreach (Transform t in transform)
                {
                    t.gameObject.SetActive(false);
                }
            }
        }

        void Start()
        {
            player = GameObject.FindWithTag("Player");
        }

        public void Update()
        {
            // If conversation is in progress, check if we want to test the condition
            if (conversationManager != null)
            {
                if (checkConditionWhileInDialogue == false)
                {
                    if (conversationManager.dialogueInProgress) return;
                }
            }
            

            if (useRange)
            {
                if (player == null)
                {
                    player = GameObject.FindWithTag("Player");

                    if (player == null) return;
                }

                if (Vector3.Distance(this.gameObject.transform.position, player.transform.position) > minimumRangeToPlayerBeforeBeginProcessing)
                {
                    return;
                }
            }

            // Condition must begin as true since we are joining it in every iteration with the next condition
            bool condition = true;

            if (tests.Length >= 1)
            {
                // Combine multiple conditions
                foreach(ConditionTest check in tests)
                {
                    condition = check.invert
                        ? condition && !check.test.Check()
                        : condition && check.test.Check();

                    if (useDebug)
                        Debug.Log(check.key + " ::: " + condition);
                }
            }

            
            // Should we toggle children instead?
            if (childrenOnly)
            {
                foreach (Transform child in gameObject.transform)
                {
                    child.gameObject.SetActive(condition);
                }
                return;
            }


            // Box, sphere collider case
            if (colliderToToggle != null)
            {
                colliderToToggle.enabled = condition;
                return;
            }


            // Should we enable / disable a MonoBehaviour component?
            if (componentToToggle != null)
            {
                componentToToggle.enabled = condition;
                return;
            }
        
            // Deactivate this component (Cant think if this will ever be useful)
            this.gameObject.SetActive(condition);
        }
    }
}
