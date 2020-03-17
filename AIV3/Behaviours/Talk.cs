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

    public class Talk : AI_Behaviour {

        AI_Core_V3 context;

        public Talk (AI_Core_V3 context)
        {
            this.context = context;
        }

        public override void Dispatch()
        {
            // GameObject player = GameObject.FindWithTag("Player");

            /*if (player != null) {
                // Hardcode for player for now. Remember that we deactivate the player when in dialogue!! :/
                context.transform.LookAt(player.gameObject.transform);
            }*/

            context.navMeshAgent.isStopped = true;
            context.GetComponent<Animator>().SetFloat("InputVertical", 0f);
        }

    }

}
