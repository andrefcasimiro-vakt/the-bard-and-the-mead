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

namespace RPG.AIV2
{

    enum FightState
    {
        ATTACK,
        DEFEND,
        LOW_HEALTH,
        ON_PLAYER_ATTACK,
        ON_PLAYER_DEFENDING,
    }

    public class FightBehaviour : Behaviour
    {

        [SerializeField]
        FightState state;

        public Behaviour AttackBehaviour;
        public Behaviour DefendBehaviour;
        public Behaviour LowHealthBehaviour;
        public Behaviour OnPlayerAttackBehaviour;
        public Behaviour OnPlayerDefendingBehaviour;

        public float maxDecisionTime = 2f;

        [Range(0, 1)]
        public float minimumChanceToAttack = 0.5f;

        // Private
        GameObject player;

        Animator animator => GetComponent<Animator>();
        AICore aiCore => GetComponent<AICore>();

        Battler battler => GetComponent<Battler>();
        Health health => GetComponent<Health>();

        WeaponManager weaponManager => GetComponent<WeaponManager>();

        public bool isOcurring = false;

        void Start()
        {
            player = GameObject.FindWithTag("Player");
        }

        public override IEnumerator Dispatch()
        {
            yield return StartCoroutine(AttackBehaviour.Dispatch(this));
            
            yield return new WaitForSeconds(2f);
        }

        bool PlayerIsFarAway()
        {
            return Vector3.Distance(player.transform.position, transform.position) > weaponManager.GetStoppingDistance();
        }

        bool PlayerIsAttacking()
        {
            return player.GetComponent<Battler>().IsAttacking();
        }

        bool PlayerIsDefending()
        {
            return player.GetComponent<Battler>().IsDefending();
        }

        void FacePlayer()
        {
            Vector3 targetPosition = new Vector3(
                player.transform.position.x,
                this.transform.position.y,
                player.transform.position.z
            );

            this.transform.LookAt(targetPosition);
        }

    }
}
