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

    public class DefendBehaviour : Behaviour {

        Battler battler => GetComponent<Battler>();

        public override IEnumerator Dispatch(FightBehaviour context) {
            context.isOcurring = true;

            battler.Defend();

            // Wait Until We Trigger Attack Animation
            yield return new WaitUntil(() => battler.IsDefending() == true);

            // Now Wait Until Attack Animation Is Over
            yield return new WaitUntil(() => battler.IsDefending() == false);

            context.isOcurring = false;
        }

    }
}
