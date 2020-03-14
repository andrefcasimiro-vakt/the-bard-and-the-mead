using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System.Linq;
using RPG.Combat;
using RPG.Weapon;

namespace RPG.AIV3 {

    public static class AI_Helpers {

        /// Move a navmesh agent to a given destination
        public static void MoveTo(
            Vector3 destination,
            float maxSpeed,
            float speed,
            NavMeshAgent navMeshAgent
        )
        {
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speed);
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }

        
        public static void FaceTarget(
            GameObject target,
            GameObject owner
        )
        {
            Vector3 targetPosition = new Vector3(target.transform.position.x,
                                                   owner.transform.position.y,
                                                   target.transform.position.z);
            owner.transform.LookAt(targetPosition);
        }

        // COMBAT Utils

        public static bool TargetIsFarAway(GameObject target, GameObject own, WeaponManager weaponManager)
        {
            return Vector3.Distance(target.transform.position, own.transform.position) > weaponManager.GetStoppingDistance();
        }
        
        public static bool TargetIsAttacking(GameObject target)
        {
            return target.GetComponent<Battler>().IsAttacking();
        }

        public static bool TargetIsDefending(GameObject target)
        {
            return target.GetComponent<Battler>().IsDefending();
        }

    }
}
