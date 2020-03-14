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

        public Flee (AI_Core_V3 context)
        {
            this.context = context;
        }

        public override void Dispatch()
        {
            Vector3 destination = context.transform.position - context.target.transform.position;
            context.MoveTo(destination);
            
            // If player is out of range, chase him
            bool isFarAwayFromPlayer = Vector3.Distance(context.transform.position, context.target.transform.position) > context.safeDistanceFromTarget;

            if (isFarAwayFromPlayer) {
                context.SetState(AGENT_STATE.PATROL);
                return;
            }
        }

    }

}
