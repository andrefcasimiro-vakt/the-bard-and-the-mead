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

    public class AttackBehaviour : Behaviour {

        Battler battler => GetComponent<Battler>();

        public override IEnumerator Dispatch(FightBehaviour context) {

            battler.Attack();

            // Wait Until We Trigger Attack Animation
            yield return new WaitUntil(() => battler.IsAttacking() == true);

            // Now Wait Until Attack Animation Is Over
            yield return new WaitUntil(() => battler.IsAttacking() == false);

        }

    }
}
