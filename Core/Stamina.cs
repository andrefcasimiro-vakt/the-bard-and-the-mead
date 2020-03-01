using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Stats;
using RPG.Saving;

namespace RPG.Core
{
    [RequireComponent(typeof(BaseStats))]
    public class Stamina : MonoBehaviour, ISaveable 
    {
        [SerializeField]
        float staminaPoints = 0f;

        [SerializeField]
        float bonusStaminaPoints = 0f;

        [SerializeField]
        float currentStaminaPoints = 0f;

        [Header("Recovery Speed")]
        [SerializeField]
        float baseRecoverySpeed = 0.1f;

        [Header("UI Element")]
        [SerializeField]
        GameObject staminaSlider;

        BaseStats baseStats => GetComponent<BaseStats>();

        // Start is called before the first frame update
        void Start()
        {
            staminaPoints = baseStats.GetStamina() + staminaPoints;

            if (currentStaminaPoints == 0f) { 
                currentStaminaPoints = staminaPoints;
            }
        }

        private void Update()
        {
            if (staminaSlider != null)
            {
                UpdateUI();
            }
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (currentStaminaPoints < staminaPoints)
            {
                RestoreStamina();
            }
        }

        // Updates the owner stamina HUD
        void UpdateUI()
        {
            staminaSlider.GetComponent<Slider>().maxValue = staminaPoints;
            staminaSlider.GetComponent<Slider>().minValue = 0;
            staminaSlider.GetComponent<Slider>().value = currentStaminaPoints;
        }

        void RestoreStamina()
        {
            float recoverySpeed = baseRecoverySpeed * baseStats.GetAgility();
            currentStaminaPoints += Time.deltaTime * recoverySpeed;
        }

        public void DecreaseStamina(float decreaseAmount)
        {
            currentStaminaPoints = Mathf.Max((currentStaminaPoints - decreaseAmount * Time.deltaTime), -1f);
        }

        public bool HasStamina()
        {
            return currentStaminaPoints > 0f;
        }

        public bool HasStaminaAgainstCostAction(float staminaCost)
        {
            return currentStaminaPoints > staminaCost;
        }

        // SAVING SYSTEM
        public object CaptureState()
        {
            return currentStaminaPoints;
        }

        public void RestoreState(object state)
        {
            this.currentStaminaPoints = (float)state;
        }

        public void OnCleanState() {
            
        }
    }
}
