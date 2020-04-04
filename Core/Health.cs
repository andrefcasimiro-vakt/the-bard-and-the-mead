using UnityEngine;
using UnityEngine.UI;
using RPG.Stats;
using RPG.AIV3;
using RPG.Combat;
using RPG.Control;
using RPG.Saving;
using RPG.Inventory;
using RPG.Weapon;
using RPG.Stats;
using UnityEngine.Events;

namespace RPG.Core {

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BaseStats))]
    public class Health: MonoBehaviour, ISaveable {

        [SerializeField] float maxHealthPoints;
        [SerializeField] float currentHealthPoints;

        [SerializeField] float bonusHealthPoints;

        [Header("Animator")]
        [SerializeField] string DieTrigger = "Die";

        BaseStats baseStats => GetComponent<BaseStats>();
        Animator animator => GetComponent<Animator>();
        
        [Header("UI")]
        [SerializeField] Slider healthbar;

        [Header("Events")]
        public UnityEvent OnDieEvent;
        
        [Header("Defense")]
        [Tooltip("When user is defending against enemy attacks")]
        public int defenseBuff = 0;

        AudioSource audioSource => GetComponent<AudioSource>();
        GameObject damageOwner;

        void Start()
        {
            // In case we have a save game, we also need to calculate the currentHealthPoints based 
            maxHealthPoints = baseStats.GetHealth() + bonusHealthPoints;
        }

        private void FixedUpdate()
        {
            UpdateHealthSlider();
        }

        void UpdateHealthSlider()
        {
            healthbar.maxValue = GetMaxHealthPoints();
            healthbar.minValue = 0f;
            healthbar.value = currentHealthPoints;
        }

        public void Restore(float restoreAmount)
        {
            currentHealthPoints = Mathf.Min(currentHealthPoints + restoreAmount, maxHealthPoints);
        }

        public void TakeDamage(float damageAmount, GameObject damageOwner)
        {
            this.damageOwner = damageOwner;

            if (IsDead())
            {    
                return;
            }

            // Get base stats for defense
            defenseBuff = (int)GetComponent<BaseStats>().GetDefense();

            CharacterEquipmentSlot charEquipmentSlots = GetComponent<CharacterEquipmentSlot>();

            int armorRate = 0;
            if (charEquipmentSlots != null)
            {
                armorRate = (int)charEquipmentSlots.GetArmorBonus();
            }
            defenseBuff += (int)armorRate; // Influenced by char stats as well

            // If we are defending, consult the equipped weapon or shield to check how much damage it should absorb using its defenseRate property
            if (GetComponent<Battler>().IsDefending())
            {
                // Assume we don't have a shield equipped, use weapon
                defenseBuff += (int)GetComponent<WeaponManager>().weaponSlots[0].currentWeapon.defenseRate;
            }

            // Finally, update the damage we will receive
            float damage = Mathf.Clamp(damageAmount - defenseBuff, 0, maxHealthPoints);
            currentHealthPoints = Mathf.Max(currentHealthPoints - damage, 0);

            UpdateHealthSlider();

            // Now evaluate result
            if (currentHealthPoints > 0f)
            {
                AI_Core_V3 ai = GetComponent<AI_Core_V3>();

                // Non-player case
                if (ai != null)
                {
                    ai.SetState(AGENT_STATE.TAKING_DAMAGE);
                }
                else
                {
                    GetComponent<Battler>().TakeDamage();
                }
            }
            else
            {
                // Award experience to Playero nly              
                if (damageOwner.gameObject.tag == "Player")
                {
                    // Reward target with experience points
                    damageOwner.GetComponent<BaseStats>().IncreaseExperience(
                        GetComponent<Battler>().GetRewardExperience()
                    );
                }

                Die();

                OnDieEvent.Invoke();

                // Remove strafe from player
                PlayerController playerController = damageOwner.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.Strafe(false);
                }
            }
        }

        public void Die()
        {   
            if (this.gameObject.tag == "Player")
            {
                GetComponent<ComponentManager>().ToggleComponents(false);
            }

            animator.SetTrigger(DieTrigger);
        }

        public bool IsDead()
        {
            return currentHealthPoints <= 0f;
        }

        public float GetMaxHealthPoints()
        {
            return maxHealthPoints;
        }

        public float GetCurrentHealth()
        {
            return currentHealthPoints;
        }

        public bool IsLowHealth()
        {
            // maxhealth is 100
            // low health is 30%
            // Formula is 30 x maxhealth / 100

            return currentHealthPoints <= (30 * maxHealthPoints) / 100;
        }

        public object CaptureState()
        {
            return currentHealthPoints;
        }

        public void RestoreState(object state)
        {
            currentHealthPoints = (float)state;

            if (currentHealthPoints <= 0)
            {
                Die();
            }
        }

        public void OnCleanState() {
            maxHealthPoints = baseStats.GetHealth() + bonusHealthPoints;
            currentHealthPoints = maxHealthPoints;
        }
    }

}
