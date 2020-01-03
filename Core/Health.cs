using UnityEngine;
using RPG.Stats;

namespace RPG.Core {

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BaseStats))]
    public class Health: MonoBehaviour {

        [SerializeField] float healthPoints;

        [SerializeField] float bonusHealthPoints;

        [Header("Animator")]
        [SerializeField] const string DieTrigger = "Die";

        BaseStats baseStats => GetComponent<BaseStats>();
        Animator animator => GetComponent<Animator>();

        void Start()
        {
            healthPoints = baseStats.GetHealth() + bonusHealthPoints;
        }
        

        public void TakeDamage(float damageAmount)
        {
            if (IsDead())
            {
                return;
            }

            healthPoints = Mathf.Max(healthPoints - damageAmount, 0);

            if (healthPoints <= 0f)
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
            return healthPoints <= 0f;
        }
    }

}
