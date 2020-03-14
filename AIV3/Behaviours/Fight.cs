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

    public class Fight : AI_Behaviour {

        AI_Core_V3 context;

        public Fight (AI_Core_V3 context)
        {
            this.context = context;
        }

        public override void Dispatch()
        {

            if (context.inProgress) {
                return;
            }

            if (context.target == null) {
                return;
            }

            if (context.target.GetComponent<Health>().IsDead())
            {
                return;
            }

            // If my health is low
            if (context.health.IsLowHealth())
            {
                context.SetState(AGENT_STATE.FLEE);
                return;
            }
        
            // If my target is far away
            if (AI_Helpers.TargetIsFarAway(
                context.target,
                context.gameObject,
                context.gameObject.GetComponent<WeaponManager>()
            ))
            {
                context.SetState(AGENT_STATE.CHASE);
                return;
            }

            // If my target is attacking me
            if (AI_Helpers.TargetIsAttacking(context.target) == true) {
                if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
                {
                    context.StartCoroutine(Defend());
                }
            }
            else // If nothing else, I will attack
            {
                context.StartCoroutine(Attack());
            }

        }

        public IEnumerator Attack() {
            context.inProgress = true;

            context.battler.Attack();

            // Wait Until We Trigger Attack Animation
            yield return new WaitUntil(() => context.battler.IsAttacking() == true);

            // Now Wait Until Attack Animation Is Over
            yield return new WaitUntil(() => context.battler.IsAttacking() == false);

            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 1f));

            context.inProgress = false;

            yield return null;
        }

        public IEnumerator Defend() {
            context.inProgress = true;

            context.battler.Defend();

            // Wait Until We Trigger Attack Animation
            yield return new WaitUntil(() => context.battler.IsDefending() == true);

            // Now Wait Until Attack Animation Is Over
            yield return new WaitUntil(() => context.battler.IsDefending() == false);

            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 1f));
            
            context.inProgress = false;

            yield return null;
        }

    }

}
