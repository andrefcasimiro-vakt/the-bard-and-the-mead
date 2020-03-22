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

    public class Flee : AI_Behaviour {

        AI_Core_V3 context;

        Vector3 destination = Vector3.zero;

        float timer = Mathf.Infinity;
        float timeUntilWeCalculateNextDestination = 1f;

        int MINIUMUM_DISTANCE_FROM_TARGET = 3;

        public Flee (AI_Core_V3 context)
        {
            this.context = context;
        }

        public override void Dispatch()
        {
            // If player is out of range, chase him
            bool isFarAwayFromPlayer = Vector3.Distance(context.transform.position, context.target.transform.position) > context.safeDistanceFromTarget;

            if (isFarAwayFromPlayer) {
                context.SetState(AGENT_STATE.PATROL);
                return;
            }

            timer += Time.deltaTime;

            if (timer >= timeUntilWeCalculateNextDestination)
            {
                timer = 0;

                if (Vector3.Distance(context.transform.position, context.target.transform.position) >= MINIUMUM_DISTANCE_FROM_TARGET)
                {
                    return;
                }

                // If we are still close to player, reset destination to a further away one
                destination = context.transform.position - context.target.transform.position;
                context.MoveTo(destination);
            }
        }

    }

}
