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

    public class TakingDamage : AI_Behaviour {

        AI_Core_V3 context;
        bool inProgress = false;

        public TakingDamage (AI_Core_V3 context)
        {
            this.context = context;
        }

        public override void Dispatch()
        {  
            if (inProgress)
                return;

            context.StartCoroutine(TakeDamageAction());
        }

        public IEnumerator TakeDamageAction() {
            inProgress = true;
            
            context.transform.Translate(new Vector3(0, 0, .5f));
 
            context.GetComponent<Battler>().TakeDamage();

            yield return new WaitUntil(() => context.GetComponent<Battler>().IsTakingDamage() == true);

            yield return new WaitUntil(() => context.GetComponent<Battler>().IsTakingDamage() == false);

            // Chase Player
            context.SetState(AGENT_STATE.CHASE);

            inProgress = false;
        }

    }

}
