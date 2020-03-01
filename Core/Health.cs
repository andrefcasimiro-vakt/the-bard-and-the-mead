using UnityEngine;
using UnityEngine.UI;
using RPG.Stats;
using RPG.AI;
using RPG.Saving;

namespace RPG.Core {

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BaseStats))]
    public class Health: MonoBehaviour, ISaveable {

        [SerializeField] float maxHealthPoints;
        [SerializeField] float currentHealthPoints;

        [SerializeField] float bonusHealthPoints;

        [Header("Animator")]
        [SerializeField] const string DieTrigger = "Die";

        BaseStats baseStats => GetComponent<BaseStats>();
        Animator animator => GetComponent<Animator>();
        

        [Header("UI")]
        [SerializeField] Slider healthbar;

        // AI
        AIController aiController => GetComponent<AIController>();

        void Start()
        {
            maxHealthPoints = baseStats.GetHealth() + bonusHealthPoints;

            if (currentHealthPoints == 0f) {
                currentHealthPoints = maxHealthPoints;
            }
        }

        private void Update()
        {
            UpdateHealthSlider();
        }

        void UpdateHealthSlider()
        {
            healthbar.maxValue = GetMaxHealthPoints();
            healthbar.minValue = 0f;
            healthbar.value = GetCurrentHealth();
        }

        public void Restore(float restoreAmount)
        {
            currentHealthPoints = Mathf.Min(currentHealthPoints + restoreAmount, maxHealthPoints);
        }

        public void TakeDamage(float damageAmount)
        {
            if (IsDead())
            {
                return;
            }


            currentHealthPoints = Mathf.Max(currentHealthPoints - damageAmount, 0);

            // Is AI?
            if (aiController != null)
            {
                aiController.SetState(StateMachineEnum.TAKE_DAMAGE);
            }

            if (currentHealthPoints <= 0f)
            {
                Die();
            }
        }

        public void Die()
        {
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
            // low health is 20%
            // Formula is 20 x maxhealth / 100

            return currentHealthPoints <= (20 * maxHealthPoints) / 100;
        }

        public object CaptureState()
        {
            return currentHealthPoints;
        }

        public void RestoreState(object state)
        {
            currentHealthPoints = (float)state;
        }

        public void OnCleanState() {

        }
    }

}
