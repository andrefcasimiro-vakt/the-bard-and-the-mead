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

    public class OnPlayerActionBehaviour : Behaviour {

        [Range(0f, 1f)]
        public float minimumChanceToAttackPlayer = 0.5f;

        [Range(0f, 1f)]
        public float minimumChanceToDefendFromPlayer = 0.3f;

        [Range(0f, 1f)]
        public float minimumChanceToCustomBehaviour = 0.0f;

        public Behaviour customBehaviour = null;

        Battler battler => GetComponent<Battler>();

        public override IEnumerator Dispatch() {

            float chance = UnityEngine.Random.Range(0, 1);

            if (chance >= minimumChanceToAttackPlayer) {
                battler.Attack();

                // Wait Until We Trigger Attack Animation
                yield return new WaitUntil(() => battler.IsAttacking() == true);

                // Now Wait Until Attack Animation Is Over
                yield return new WaitUntil(() => battler.IsAttacking() == false);
            
            } else if (chance >= minimumChanceToCustomBehaviour) {
                battler.Defend();

                // Wait Until We Trigger Attack Animation
                yield return new WaitUntil(() => battler.IsDefending() == true);

                // Now Wait Until Attack Animation Is Over
                yield return new WaitUntil(() => battler.IsDefending() == false);

            } else if (chance >= minimumChanceToCustomBehaviour) {

                if (customBehaviour != null) {
                    StartCoroutine(customBehaviour.Dispatch());
                    yield return null;
                }

            }

            yield return null;
        }

    }
}
