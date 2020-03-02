using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RPG.Core;

namespace RPG.AIV2 {

    public class AICore : MonoBehaviour {

        [SerializeField]
        private AIState state = AIState.PATROL;
        private AIState previousState;

        [Header("AI Behaviours")]
        // Idle
        public Behaviour PatrolBehaviour;

        // Movement
        public Behaviour ArriveAtTargetBehaviour;
        public Behaviour ChaseBehaviour;
        public Behaviour FleeBehaviour;

        // Combat
        public Behaviour FightBehaviour;

        // Custom Action
        public Behaviour CustomBehaviour;

        [Header("AI Components")]
        public Component Movement;
        public Component Vision;

        // Private
        Health health => GetComponent<Health>();

        GameObject player;

        void Start() {
            player = GameObject.FindWithTag("Player");
        }

        void Update() {
            if (health.IsDead())
                return;

            HandleComponents();
            HandleState();
        }

        void HandleComponents() {
            StartCoroutine(Vision.Dispatch());
            StartCoroutine(Movement.Dispatch());
        }

        void HandleState() {
            switch (state) {
                case AIState.PATROL:
                    StartCoroutine(PatrolBehaviour.Dispatch());
                    break;
                case AIState.CUSTOM:
                    StartCoroutine(CustomBehaviour.Dispatch());
                    break;
                case AIState.CHASE:
                    StartCoroutine(ChaseBehaviour.Dispatch());
                    break;
                case AIState.FLEE:
                    StartCoroutine(FleeBehaviour.Dispatch());
                    break;
                case AIState.ARRIVE_AT_TARGET:
                    StartCoroutine(ArriveAtTargetBehaviour.Dispatch());
                    break;
                case AIState.FIGHT:
                    StartCoroutine(FightBehaviour.Dispatch());
                    break;
                default:
                    break;
            }

            // Record changes of state
            if (previousState != state) {
                previousState = state;
            }
        }

        public AIState GetState() {
            return state;
        }


        public void SetState(AIState nextState) {
            state = nextState;
        }

        public void SetState(string nextState) {
            state = (AIState)Enum.Parse(typeof(AIState), nextState);
        }

        // Save System
        public object CaptureState()
        {
            return state;
        }

        public void RestoreState(object state)
        {
            AIState loadedState = (AIState)state;

            state = loadedState;
        }

        public void OnCleanState() {
        }
    }
}
