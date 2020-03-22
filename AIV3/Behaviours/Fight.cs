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
using RPG.Stats;

namespace RPG.AIV3 {

    public class Fight : AI_Behaviour {

        AI_Core_V3 context;
        bool inProgress = false;

        public Fight (AI_Core_V3 context)
        {
            this.context = context;
        }

        public override void Dispatch()
        {
            if (context.target == null) {
                context.SetState(AGENT_STATE.PATROL);
                return;
            }

            if (context.target.GetComponent<Health>().IsDead())
            {
                context.SetState(AGENT_STATE.PATROL);
                return;
            }

            // If my health is low
            if (context.health.IsLowHealth())
            {
                float chanceToFlee = UnityEngine.Random.Range(0f, 1f);

                if (chanceToFlee >= context.minimumChanceToFleeIfLowHealth)
                {
                    context.SetState(AGENT_STATE.FLEE);
                    return;
                }

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

            if (inProgress)
                return;


            // Target is attacking, let's receive the attack and decide later if we should dodge or defend 
            if (context.target.GetComponent<Battler>().IsAttacking())
            {
                float chanceToNotAttack = UnityEngine.Random.Range(0, 1f);
                if (chanceToNotAttack >= 0.1f)
                    return;
            }

            float chanceToAttack = UnityEngine.Random.Range(0, 1f);

            if (chanceToAttack < context.minimumChanceToAttack)
            {
                Debug.Log("Hit twice");
                context.SetState(AGENT_STATE.CHASE);
                return;
            }

            context.StartCoroutine(Attack());
        }

        public IEnumerator Attack() {
            inProgress = true;

            context.battler.Attack();

            // Wait Until We Trigger Attack Animation
            yield return new WaitUntil(() => context.battler.IsAttacking() == true);

            // Now Wait Until Attack Animation Is Over
            yield return new WaitUntil(() => context.battler.IsAttacking() == false);

            float seconds = context.baseAttackCooldown * UnityEngine.Random.Range(1f, 2f);

            float agility = context.gameObject.GetComponent<BaseStats>().GetAgility() * 0.0001f;

            yield return new WaitForSeconds(seconds - agility);


            context.SetState(AGENT_STATE.CHASE);

            inProgress = false;
        }

    }

}
