using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.AI;
using RPG.Core;
using RPG.Combat;
using RPG.Weapon;

namespace RPG.AIV2 {

    enum FightState {
        ATTACK,
        DEFEND,
        LOW_HEALTH,
        ON_PLAYER_ATTACK,
        ON_PLAYER_DEFENDING,
    }

    public class FightBehaviour : Behaviour {

        [SerializeField]
        FightState state;

        public Behaviour AttackBehaviour;
        public Behaviour DefendBehaviour;
        public Behaviour LowHealthBehaviour;
        public Behaviour OnPlayerAttackBehaviour;
        public Behaviour OnPlayerDefendingBehaviour;

        public float maxDecisionTime = 2f;

        // Private
        GameObject player;

        Animator animator => GetComponent<Animator>();
        AICore aiCore => GetComponent<AICore>();

        Battler battler => GetComponent<Battler>();
        Health health => GetComponent<Health>();

        WeaponManager weaponManager => GetComponent<WeaponManager>();

        void Start() {
            player = GameObject.FindWithTag("Player");
        }

        public override IEnumerator Dispatch() {
           
            // Force battler to strafe
            animator.SetBool("IsStrafing", true);

            yield return new WaitUntil(
                () => DecideNextMove()
            );

            yield return StartCoroutine(HandleState());


            // Give a decision time before restarting loop
            yield return new WaitForSeconds(
                UnityEngine.Random.Range(0f, maxDecisionTime)
            );

            yield return null;
        }

        public bool DecideNextMove() {

            FacePlayer();

            // Audit player first
            if (player.GetComponent<Health>().IsDead())
                return true;

            if (health.IsLowHealth()) {
                state = FightState.LOW_HEALTH;
                return true;
            }

            if (PlayerIsFarAway()) {
                aiCore.SetState(AIState.CHASE);
                return true;
            }

            if (PlayerIsAttacking()) {
                state = FightState.ON_PLAYER_ATTACK;
                return true;
            }

            if (PlayerIsDefending()) {
                state = FightState.ON_PLAYER_DEFENDING;
                return true;
            }

            return true;
        }


        IEnumerator HandleState() {
            switch (state) {
                case FightState.ATTACK:
                    StartCoroutine(AttackBehaviour.Dispatch());
                    break;
                case FightState.DEFEND:
                    StartCoroutine(DefendBehaviour.Dispatch());
                    break;
                case FightState.LOW_HEALTH:
                    StartCoroutine(LowHealthBehaviour.Dispatch());
                    break;
                case FightState.ON_PLAYER_ATTACK:
                    StartCoroutine(OnPlayerAttackBehaviour.Dispatch());
                    break;
                case FightState.ON_PLAYER_DEFENDING:
                    StartCoroutine(OnPlayerDefendingBehaviour.Dispatch());
                    break;
                default:
                    break;
            }
            yield return null;
        }

        // Helpers

        bool PlayerIsFarAway()
        {
            return Vector3.Distance(player.transform.position, transform.position) > weaponManager.GetStoppingDistance();
        }

        bool PlayerIsAttacking()
        {
            return player.GetComponent<Battler>().IsAttacking();
        }

        bool PlayerIsDefending()
        {
            return player.GetComponent<Battler>().IsDefending();
        }

        void FacePlayer()
        {
            Vector3 targetPosition = new Vector3(
                player.transform.position.x,
                this.transform.position.y,
                player.transform.position.z
            );
            
            this.transform.LookAt(targetPosition);
        }

    }
}
