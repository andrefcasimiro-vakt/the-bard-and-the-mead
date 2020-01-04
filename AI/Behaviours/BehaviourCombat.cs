using UnityEngine;
using System.Collections;
using RPG.Core;
using RPG.Combat;
using RPG.Weapon;

namespace RPG.AI
{

    public class BehaviourCombat : MonoBehaviour
    {
        GameObject player;
        Animator animator => GetComponent<Animator>();
        AIController controller => GetComponent<AIController>();
        Battler battler => GetComponent<Battler>();
        Health health => GetComponent<Health>();
        Stamina stamina => GetComponent<Stamina>();
        WeaponManager weaponManager => GetComponent<WeaponManager>();

        bool inProgress = false;

        // Constants
        const string ANIMATOR_IS_STRAFING = "IsStrafing";

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
        }

        public void Dispatch()
        {
            animator.SetBool(ANIMATOR_IS_STRAFING, inProgress);

            if (inProgress == false)
            {
                inProgress = true;
                DecideNextMove();
            }
        }

        private void LateUpdate()
        {
            if (inProgress)
            {
                FacePlayer();
            }
        }

        void FacePlayer()
        {
            Vector3 targetPosition = new Vector3(player.transform.position.x,
                                                   this.transform.position.y,
                                                   player.transform.position.z);
            this.transform.LookAt(targetPosition);
        }

        void DecideNextMove()
        {
            // Audit Player First
            if (PlayerIsDead())
            {
                ExitCombat();
                return;
            }

            if (PlayerIsFarAway())
            {
                ChasePlayer();
                return;
            }

            if (PlayerIsAttacking())
            {
                RespondToAttack();
                return;
            }

            if (PlayerIsDefending())
            {
                RespondToDefense();
                return;
            }

            // If Nothing Happens, Take Initiative
            StartCoroutine(Attack());
        }

        // MAIN ACTIONS
        void ExitCombat()
        {
            inProgress = false;
            controller.SetState(StateMachineEnum.PATROL);
        }

        void ChasePlayer()
        {
            inProgress = false;
            controller.SetState(StateMachineEnum.CHASE);
        }

        void RespondToAttack()
        {
            float probability = Random.Range(0f, 1f);

            if (health.IsLowHealth())
            {
                // RUN AWAY
                if (true)
                {
                    StartCoroutine(RunAway());
                    return;
                }

                // TAKE POTION (if has a potion in inventory)

                // RESTORE MAGIC (if is mage and can use magic and has healing capability)
            }

            StartCoroutine(Attack());
            return;

            // Defend
            if (probability > 0.5f)
            {
                StartCoroutine(Defend());
                return;
            }

            // Or Dodge
            StartCoroutine(Dodge());
            return;
        }

        void RespondToDefense()
        {
            float probability = Random.Range(0f, 1f);

            // If player is low on health
            if (player.GetComponent<Health>().IsLowHealth())
            {
                // Attack
                StartCoroutine(Attack());
                return;
            }

            // Attack
            if (probability > 0.5f)
            {
                // Maybe do a power attack in the future
                StartCoroutine(Attack());
                return;
            }

            // Or Dodge
            StartCoroutine(Dodge());
            return;
        }


        // IEnumerators
        IEnumerator Attack()
        {
            // Random start delay
            float startTime = Random.Range(0f, 2f);
            yield return new WaitForSeconds(startTime);

            battler.Attack();

            // Wait Until We Trigger Attack Animation
            yield return new WaitUntil(() => battler.IsAttacking() == true);

            // Now Wait Until Attack Animation Is Over
            yield return new WaitUntil(() => battler.IsAttacking() == false);

            // Reset
            inProgress = false;
        }
        IEnumerator Defend()
        {
            battler.Defend();

            // Wait Until We Trigger Defend Animation
            yield return new WaitUntil(() => battler.IsDefending() == true);

            // Wait until we are back to Idle state
            yield return new WaitUntil(() => battler.IsDefending() == false);

            // Reset
            inProgress = false;
        }
        IEnumerator Dodge()
        {
            battler.Dodge();

            // Wait Until We Trigger Defend Animation
            yield return new WaitUntil(() => battler.IsDodging() == true);

            // Wait until we are back to Idle state
            yield return new WaitUntil(() => battler.IsDodging() == false);

            // Reset
            inProgress = false;
        }

        IEnumerator RunAway()
        {
            print("Low health. Run away!");

            yield return new WaitForEndOfFrame();

            // TODO: Change to Flee
            controller.SetState(StateMachineEnum.PATROL);
            inProgress = false;
        }

        // GETTERS
        bool PlayerIsDead()
        {
            return player.GetComponent<Health>().IsDead();
        }

        public bool PlayerIsFarAway()
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

        bool PlayerIsDodging()
        {
            return player.GetComponent<Battler>().IsDodging();
        }
    }

}
