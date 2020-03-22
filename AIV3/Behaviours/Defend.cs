using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using RPG.Core;
using RPG.Combat;
using RPG.Weapon;
using RPG.AI;
using RPG.Saving;
using System.Linq;

namespace RPG.AIV3 {

    public class Defend : AI_Behaviour {

        AI_Core_V3 context;

        bool inProgress = false;

        public Defend (AI_Core_V3 context)
        {
            this.context = context;
        }

        public override void Dispatch()
        {
            if (inProgress) {
                return;
            }

            context.StartCoroutine(DefendAction());

        }

        public IEnumerator DefendAction() {
            inProgress = true;

            context.battler.Defend();

            // Wait Until We Trigger Attack Animation
            yield return new WaitUntil(() => context.battler.IsDefending() == true);

            // Now Wait Until Attack Animation Is Over
            yield return new WaitUntil(() => context.battler.IsDefending() == false);

            inProgress = false;

            context.SetState(AGENT_STATE.FIGHTING);
        }

    }

}
