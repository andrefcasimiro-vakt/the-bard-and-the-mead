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

        Battler battler => GetComponent<Battler>();

        public override IEnumerator Dispatch(FightBehaviour context) {
            context.isOcurring = true;

            float chance = UnityEngine.Random.Range(0, 1);

            if (chance >= minimumChanceToAttackPlayer) {
                battler.Attack();

                // Wait Until We Trigger Attack Animation
                yield return new WaitUntil(() => battler.IsAttacking() == true);

                // Now Wait Until Attack Animation Is Over
                yield return new WaitUntil(() => battler.IsAttacking() == false);
            
            } else {
                battler.Defend();

                // Wait Until We Trigger Attack Animation
                yield return new WaitUntil(() => battler.IsDefending() == true);

                // Now Wait Until Attack Animation Is Over
                yield return new WaitUntil(() => battler.IsDefending() == false);

            }

            context.isOcurring = false;
        }

    }
}
