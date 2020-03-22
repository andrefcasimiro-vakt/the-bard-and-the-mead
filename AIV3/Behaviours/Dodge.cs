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

    public class Dodge : AI_Behaviour {

        AI_Core_V3 context;

        bool inProgress = false;

        public Dodge (AI_Core_V3 context)
        {
            this.context = context;
        }

        public override void Dispatch()
        {
            if (inProgress) {
                return;
            }
            
            context.StartCoroutine(DodgeAction());

        }

        public IEnumerator DodgeAction() {
            inProgress = true;

            context.transform.Translate(new Vector3(0, 0, -0.5f));

            context.battler.Dodge();

            // Wait Until We Trigger Attack Animation
            yield return new WaitUntil(() => context.battler.IsDodging() == true);

            // Now Wait Until Attack Animation Is Over
            yield return new WaitUntil(() => context.battler.IsDodging() == false);

            inProgress = false;

            context.SetState(AGENT_STATE.FIGHTING);
        }

    }

}
